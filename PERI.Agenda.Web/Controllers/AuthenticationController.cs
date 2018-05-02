﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;

namespace PERI.Agenda.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "Forgot Password";
            return View();
        }
    }
}