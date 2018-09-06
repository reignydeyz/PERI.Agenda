using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public UserController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        /// <summary>
        /// Updates the user's role
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task UpdateRole([FromBody] Models.Member args)
        {
            var bll_u = new BLL.EndUser(unitOfWork);

            var u = new EF.EndUser();
            u.MemberId = args.Id;
            u.RoleId = args.RoleId.Value;

            await bll_u.UpdateRole(u);
        }
    }
}