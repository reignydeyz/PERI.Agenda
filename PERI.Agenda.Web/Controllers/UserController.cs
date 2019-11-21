using Microsoft.AspNetCore.Mvc;
using PERI.Agenda.BLL;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IEndUser endUserBusiness;

        public UserController(IEndUser endUser)
        {
            this.endUserBusiness = endUser;
        }

        /// <summary>
        /// Updates the user's role
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task UpdateRole([FromBody] Models.Member args)
        {
            var bll_u = endUserBusiness;

            var u = new EF.EndUser();
            u.MemberId = args.Id;
            u.RoleId = args.RoleId.Value;

            await bll_u.UpdateRole(u);
        }
    }
}