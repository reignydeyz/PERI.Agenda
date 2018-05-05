using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;

namespace PERI.Agenda.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Core.Emailer smtp;
        private readonly Core.GoogleReCaptcha captcha;

        public AuthenticationController(IOptions<Core.Emailer> settingsOptions, IOptions<Core.GoogleReCaptcha> options)
        {
            smtp = settingsOptions.Value;
            captcha = options.Value;
        }

        private async Task AddClaimsAndSignIn(EF.EndUser args)
        {
            var ci = new ClaimsIdentity(
                    new[]
                    {
                        // User info
                        new Claim(ClaimTypes.NameIdentifier, args.UserId.ToString()),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                        new Claim(ClaimTypes.Name, args.Member.Name),
                        new Claim(ClaimTypes.Email, args.Member.Email),
                        new Claim(ClaimTypes.UserData, Core.JWT.GenerateToken(args.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"))),

                        // Role
                        new Claim(ClaimTypes.Role, args.Role.Name),
                    }, "MyCookieMiddlewareInstance");

            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.AddIdentity(ci);

            await HttpContext.SignInAsync("MyCookieMiddlewareInstance", principal);
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home", null);
            else
            {
                ViewData["Title"] = "Sign-in";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Models.Login args)
        {
            ViewData["Title"] = "Sign-in";

            var context = new EF.AARSContext();

            var buser = new BLL.EndUser(context);

            var user = await buser.Get(new EF.EndUser { Member = new EF.Member { Email = args.Email } });

            if (user != null)
            {
                // Check if active
                if (user.DateInactive != null)
                {
                    ModelState.AddModelError(string.Empty, "Account is inactive.");
                    TempData["notice"] = "Account is inactive.";
                    return View(args);
                }

                // Check password
                var salt = user.PasswordSalt;
                var saltBytes = Convert.FromBase64String(salt);

                if (Core.Crypto.Hash(args.Password, saltBytes) == user.PasswordHash)
                {
                    // Successful log in
                    user.LastSessionId = Guid.NewGuid().ToString();
                    user.LastLoginDate = DateTime.Now;
                    await buser.Edit(user);

                    await AddClaimsAndSignIn(user);

                    return Redirect("~/");
                }
            }

            ModelState.AddModelError(string.Empty, "Access denied.");
            TempData["notice"] = "Access denied.";
            return View(args);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] Models.Login args)
        {
            var context = new EF.AARSContext();

            var buser = new BLL.EndUser(context);

            var user = await buser.Get(new EF.EndUser { Member = new EF.Member { Email = args.Email } });

            if (user != null)
            {
                // Check if active
                if (user.DateInactive != null)
                {
                    ModelState.AddModelError(string.Empty, "Account is inactive.");
                    TempData["notice"] = "Account is inactive.";
                    return View(args);
                }

                // Check password
                var salt = user.PasswordSalt;
                var saltBytes = Convert.FromBase64String(salt);

                if (Core.Crypto.Hash(args.Password, saltBytes) == user.PasswordHash)
                {
                    // Successful log in
                    user.LastSessionId = Guid.NewGuid().ToString();
                    user.LastLoginDate = DateTime.Now;
                    await buser.Edit(user);

                    return Json(Core.JWT.GenerateToken(user.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret")));
                }
            }

            return Unauthorized();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("MyCookieMiddlewareInstance");

            return RedirectToAction("Index", "Authentication");
        }

        public async Task<IActionResult> SignUp()
        {
            ViewData["Title"] = "Sign-up";

            var context = new EF.AARSContext();

            ViewBag.Communities = new SelectList(await new BLL.Community(context).Get(), "Id", "Name");
            ViewBag.ReCaptcha = captcha;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(BLL.ValidateReCaptchaAttribute))]
        public async Task<IActionResult> SignUp(Models.SignUp args)
        {
            ViewData["Title"] = "Sign-up";

            var context = new EF.AARSContext();

            using (var txn = context.Database.BeginTransaction())
            {
                ViewBag.Communities = new SelectList(await new BLL.Community(context).Get(), "Id", "Name");
                ViewBag.ReCaptcha = captcha;

                try
                {
                    if (!ModelState.IsValid)
                        return View(args);
                    else
                    {
                        var bll_member = new BLL.Member(context);

                        var m = new EF.Member
                        {
                            Name = (args.FirstName + " " + args.LastName).ToUpper(),
                            NickName = args.NickName,
                            Address = args.Address,
                            Mobile = args.Mobile,
                            Email = args.Email,
                            BirthDate = args.BirthDate,
                            CommunityId = args.CommunityId
                        };

                        var id = await bll_member.Add(m);

                        // Add to User
                        if (args.Email != null)
                        {
                            // Generate ConfirmationCode
                            Guid g = Guid.NewGuid();
                            string guidString = Convert.ToBase64String(g.ToByteArray());
                            guidString = guidString.Replace("=", "");
                            guidString = guidString.Replace("+", "");

                            var bll_user = new BLL.EndUser(context);
                            var newId = await bll_user.Add(new EF.EndUser
                            {
                                MemberId = id,
                                ConfirmationCode = guidString
                            });

                            // Send email
                            await smtp.SendEmail(args.Email,
                                "Your Agenda Credentials",
                                "Please click the link below to validate and change your password:<br/>http://" + Request.Host.Value + "/authentication/newpassword/?userid=" + newId + "&code=" + guidString);
                        }

                        txn.Commit();

                        TempData["notice"] = "Thank you. Please check your email for confirmation.";
                        return Redirect("~/");
                    }
                }
                catch (DbUpdateException ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    ModelState.AddModelError(string.Empty, "Existing name or email");

                    return View(args);
                }
                catch (Exception ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    ModelState.AddModelError(string.Empty, ex.Message);

                    return View(args);
                }
            }
        }

        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "Forgot Password";
            ViewBag.ReCaptcha = captcha;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(BLL.ValidateReCaptchaAttribute))]
        public async Task<IActionResult> ForgotPassword(Models.ForgotPassword args)
        {
            ViewData["Title"] = "Forgot Password";
            ViewBag.ReCaptcha = captcha;

            if (!ModelState.IsValid)
                return View();

            var context = new EF.AARSContext();

            var bll_user = new BLL.EndUser(context);

            var user = await bll_user.Get(new EF.EndUser { Member = new EF.Member { Email = args.Email } });

            if (user != null)
            {
                // Generate ConfirmationCode
                Guid g = Guid.NewGuid();
                string guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");

                user.ConfirmationCode = guidString;
                user.ConfirmationExpiry = DateTime.Now.AddHours(12);

                await bll_user.Edit(user);

                // Send email
                await smtp.SendEmail(args.Email,
                    "Your Agenda Credentials",
                    "Please click the link below to validate and change your password:<br/>http://" + Request.Host.Value + "/authentication/newpassword/?userid=" + user.UserId + "&code=" + guidString);

                TempData["notice"] = "Thank you. Please check your email for confirmation.";

                return Redirect("~/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email is not found");
                return View();
            }
        }

        public async Task<IActionResult> NewPassword()
        {
            ViewData["Title"] = "New password";

            var userId = Request.Query["userid"].ToString();
            var code = Request.Query["code"].ToString();

            using (var context = new EF.AARSContext())
            {
                var bll_user = new BLL.EndUser(context);

                var user = await bll_user.Get(new EF.EndUser { UserId = Convert.ToInt32(userId) });

                if (user != null)
                {
                    if (user.ConfirmationCode == code && user.ConfirmationExpiry > DateTime.Now)
                        return View(new Models.NewPassword() { EndUser = new EF.EndUser { UserId = user.UserId } });
                    else
                        return new NotFoundResult();
                }
                else
                    return new NotFoundResult();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(Models.NewPassword args)
        {
            ViewData["Title"] = "New Password";

            if (!ModelState.IsValid)
                return View();

            var context = new EF.AARSContext();

            var bll_user = new BLL.EndUser(context);

            var user = await bll_user.Get(new EF.EndUser { UserId = args.EndUser.UserId });

            var salt = Core.Crypto.GenerateSalt();
            var enc = Core.Crypto.Hash(args.Password, salt);

            user.PasswordHash = enc;
            user.PasswordSalt = Convert.ToBase64String(salt);
            user.LastPasswordChanged = DateTime.Now;
            user.DateConfirmed = user.LastPasswordChanged;
            user.DateConfirmed = DateTime.Now;
            user.ConfirmationCode = null;
            user.ConfirmationExpiry = null;
            user.DateInactive = null;

            await bll_user.Edit(user);

            return await Index(new Models.Login { Email = user.Member.Email, Password = args.Password });
        }
    }
}