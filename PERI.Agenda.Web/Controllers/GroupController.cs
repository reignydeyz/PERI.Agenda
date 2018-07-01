using System;
using System.Collections.Generic;
using System.Dynamic;
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
    [Route("api/Group")]
    public class GroupController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GroupController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody] EF.Group obj)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            var res = from r in (await bll_g.Find(obj).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.GroupCategoryId,
                          Category = r.GroupCategory.Name,
                          r.Name,
                          Members = r.GroupMember.Count,
                      };

            return Json(res);
        }

        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] EF.Group obj, int id)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_g.Find(obj)
                      select new
                      {
                          r.Id,
                          r.GroupCategoryId,
                          Category = r.GroupCategory.Name,
                          r.Name,
                          Members = r.GroupMember.Count,
                      };
            var page = id;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.groups = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }
        
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_g.Get(new EF.Group { Id = id });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.groupCategoryId = r.GroupCategoryId;
            obj.category = r.GroupCategory.Name;
            obj.name = r.Name;
            obj.members = r.GroupMember.Count;

            return Json(obj);
        }
    }
}