using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PERI.Agenda.Core;
using Microsoft.Extensions.Options;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Emailer smtp;

        public AccountController(IOptions<Core.Emailer> settingsOptions)
        {
            smtp = settingsOptions.Value;
        }

        [HttpGet("[action]")]
        public EF.Member Profile()
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            return new EF.Member
            {
                Name = user.Member.Name,
                NickName = user.Member.NickName,
                BirthDate = user.Member.BirthDate,
                Gender = user.Member.Gender,
                Email = user.Member.Email,
                Address = user.Member.Address,
                Mobile = user.Member.Mobile
            };
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> ChangePassword([FromBody] Models.ChangePassword args)
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            var context = new EF.AARSContext();
            var bll_user = new BLL.EndUser(context);

            // Validate current password
            var salt = user.PasswordSalt;
            var saltBytes = Convert.FromBase64String(salt);

            if (Core.Crypto.Hash(args.CurrentPassword, saltBytes) == user.PasswordHash)
            {
                var newSalt = Core.Crypto.GenerateSalt();
                user.PasswordSalt = Convert.ToBase64String(newSalt);
                user.PasswordHash = Core.Crypto.Hash(args.NewPassword, newSalt);
                
                await bll_user.Edit(user);

                return Ok();
            }
            else
            {
                return new ObjectResult("Invalid password.")
                {
                    StatusCode = 403,
                    Value = "Invalid password."
                };
            }
        }
    }
}