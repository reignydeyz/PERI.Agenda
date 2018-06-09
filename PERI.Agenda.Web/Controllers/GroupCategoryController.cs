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
    [Route("api/GroupCategory")]
    public class GroupCategoryController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GroupCategoryController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody]EF.GroupCategory args)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;

            var res = from r in (await bll_gc.Find(args).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.Name,
                          Groups = r.Group.Count(),
                          Members = r.Group.Sum(x => x.GroupMember.Select(y => y.MemberId).Distinct().Count())
                      };

            return Json(res);
        }

        [HttpPost("[action]")]
        public async Task<int> New([FromBody]EF.GroupCategory args)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;

            return await bll_gc.Add(args);
        }
    }
}