using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using PERI.Agenda.BLL;
using PERI.Agenda.Web.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName ="General")]
    public class AuthenticationController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Core.Emailer smtp;
        private readonly Core.GoogleReCaptcha captcha;
        private readonly IEndUser endUserBusiness;
        private readonly IMember memberBusiness;
        private readonly ICommunity communityBusiness;
        private readonly EF.AARSContext context;

        private readonly IMapper mapper;

        public AuthenticationController(IOptions<Core.Emailer> settingsOptions,
            IOptions<Core.GoogleReCaptcha> options,
            IEndUser endUser,
            IMember member,
            ICommunity community,
            EF.AARSContext context,
            IMapper mapper)
        {
            smtp = settingsOptions.Value;
            captcha = options.Value;
            this.context = context;
            this.endUserBusiness = endUser;
            this.memberBusiness = member;
            this.communityBusiness = community;
            this.mapper = mapper;
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
                        new Claim(ClaimTypes.UserData, Core.JWT.GenerateToken(args.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"),
                            Core.Setting.Configuration.GetValue<int>("JWT:MinutesToExpire"))),

                        // Role
                        new Claim(ClaimTypes.Role, args.Role.Name),
                    }, "MyCookieMiddlewareInstance");

            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.AddIdentity(ci);

            await HttpContext.SignInAsync("MyCookieMiddlewareInstance", principal);
        }       

        /// <summary>
        /// Authenticates the client
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Signin")]
        //[ApiExplorerSettings(GroupName = "Authentication")]
        [ProducesResponseType(typeof(Token), 200)]
        public async Task<IActionResult> SignIn([FromBody] Models.Login args)
        {
            var buser = endUserBusiness;

            var user = await buser.GetByEmail(args.Email);

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

                    var accessTokenMinutes = Core.Setting.Configuration.GetValue<int>("JWT:AccessTokenMinutesToExpire");
                    var refreshTokenMinutes = Core.Setting.Configuration.GetValue<int>("JWT:RefreshTokenMinutesToExpire");

                    var accessToken = Core.JWT.GenerateToken(user.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"), accessTokenMinutes);
                    var refreshToken = Core.JWT.GenerateToken(user.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"), refreshTokenMinutes);

                    return Json(new Token
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Re-authenticates client with refresh-token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        [BLL.VerifyUser]
        [ProducesResponseType(typeof(Token), 200)]
        public async Task<IActionResult> Token()
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            // Successful log in
            user.LastSessionId = Guid.NewGuid().ToString();
            user.LastLoginDate = DateTime.Now;
            await endUserBusiness.Edit(user);

            var accessTokenMinutes = Core.Setting.Configuration.GetValue<int>("JWT:AccessTokenMinutesToExpire");
            var refreshTokenMinutes = Core.Setting.Configuration.GetValue<int>("JWT:RefreshTokenMinutesToExpire");

            var accessToken = Core.JWT.GenerateToken(user.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"), accessTokenMinutes);
            var refreshToken = Core.JWT.GenerateToken(user.UserId, Core.Setting.Configuration.GetValue<string>("JWT:Secret"), refreshTokenMinutes);

            return Json(new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        
    }
}