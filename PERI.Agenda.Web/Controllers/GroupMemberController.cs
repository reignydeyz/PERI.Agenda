using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/GroupMember")]
    public class GroupMemberController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GroupMemberController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [Route("Find/{id}")]
        public async Task<IActionResult> Members([FromBody] EF.Member obj, int id)
        {
            var bll_gm = new BLL.GroupMember(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            return Json(await bll_gm.Members(obj, id).ToListAsync());
        }
    }
}