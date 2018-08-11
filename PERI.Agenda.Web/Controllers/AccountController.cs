using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PERI.Agenda.Core;
using Microsoft.Extensions.Options;
using System.Dynamic;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Emailer smtp;
        private readonly UnitOfWork unitOfWork; 

        public AccountController(IOptions<Core.Emailer> settingsOptions)
        {
            smtp = settingsOptions.Value;
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpGet("[action]")]
        public async Task<Models.Member> Profile()
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var bll_member = new BLL.Member(unitOfWork);

            var r = await bll_member.Get(new EF.Member { Id = user.MemberId, CommunityId = user.Member.CommunityId });
            return new Models.Member
            {
                Address = r.Address,
                BirthDate = r.BirthDate,
                CivilStatus = r.CivilStatus,
                CommunityId = r.CommunityId,
                Email = r.Email,
                Gender = r.Gender,
                Id = r.Id,
                InvitedBy = r.InvitedBy,
                InvitedByMemberName = r.InvitedBy > 0 ? bll_member.GetById(r.InvitedBy.Value).Result.Name : "",
                IsActive = r.IsActive,
                Mobile = r.Mobile,
                Name = r.Name,
                NickName = r.NickName,
                Remarks = r.Remarks
            };
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> ChangePassword([FromBody] Models.ChangePassword args)
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            var bll_user = new BLL.EndUser(unitOfWork);

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

        [HttpGet("[action]")]
        public IActionResult Role()
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            dynamic obj1 = new ExpandoObject();
            obj1.roleId = user.Role.RoleId;
            obj1.name = user.Role.Name;

            return Json(obj1);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Deactivate([FromBody] Models.Login args)
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            var bll_user = new BLL.EndUser(unitOfWork);

            // Validate current password
            var salt = user.PasswordSalt;
            var saltBytes = Convert.FromBase64String(salt);

            if (Core.Crypto.Hash(args.Password, saltBytes) == user.PasswordHash)
            {
                await bll_user.Delete(user);

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