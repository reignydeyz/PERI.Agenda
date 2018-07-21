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
            var bll_m = new BLL.Member(unitOfWork);
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
                          Members = r.GroupMember.Count
                      };

            return Json(res);
        }

        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] Models.Group obj, int id)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Group>(obj);

            o.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            // Get group leader id
            var bll_m = new BLL.Member(unitOfWork);
            var glid = await bll_m.GetIdByName(obj.Leader ?? "", user.Member.CommunityId.Value);

            o.GroupLeader = glid;

            var res = from r in bll_g.Find(o)
                      join m in bll_m.Find(new EF.Member { CommunityId = user.Member.CommunityId }) on r.GroupLeader equals m.Id
                      select new
                      {
                          r.Id,
                          r.GroupCategoryId,
                          Category = r.GroupCategory.Name,
                          r.Name,
                          Members = r.GroupMember.Count,
                          Leader = m.Name,
                          LeaderMemberId = m.Id,
                          isMember = r.GroupMember.Count(x => x.MemberId == user.MemberId) > 0,
                          isLeader = user.MemberId == m.Id
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
            var bll_m = new BLL.Member(unitOfWork);
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_g.Get(new EF.Group { Id = id });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.groupCategoryId = r.GroupCategoryId;
            obj.category = r.GroupCategory.Name;
            obj.name = r.Name;
            obj.members = r.GroupMember.Count;
            obj.leader = bll_m.GetById(r.GroupLeader.Value).Result.Name;
            obj.leaderMemberId = r.GroupLeader;
            obj.isMember = r.GroupMember.Count(x => x.MemberId == user.MemberId) > 0;
            obj.isLeader = r.GroupLeader == user.MemberId;

            return Json(obj);
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.Group obj)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Group>(obj);

            // Get group leader id
            var bll_m = new BLL.Member(unitOfWork);
            var glid = await bll_m.GetIdByName(obj.Leader ?? user.Member.Name, user.Member.CommunityId.Value);

            if (glid == null)
            {
                return new ObjectResult("Leader is not found.")
                {
                    StatusCode = 403,
                    Value = "Leader is not found."
                };
            }
            else
            {
                o.GroupLeader = glid;
                o.DateCreated = DateTime.Now;
                o.CreatedBy = user.Member.Name;

                return Ok(await bll_g.Add(o));
            }
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> Edit([FromBody] Models.Group obj)
        {
            var bll_g = new BLL.Group(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Group>(obj);

            // Get group leader id
            var bll_m = new BLL.Member(unitOfWork);
            var glid = await bll_m.GetIdByName(obj.Leader ?? user.Member.Name, user.Member.CommunityId.Value);

            if (glid == null)
            {
                return new ObjectResult("Leader is not found.")
                {
                    StatusCode = 403,
                    Value = "Leader is not found."
                };
            }
            else
            {
                o.GroupLeader = glid;

                await bll_g.Edit(o);

                return Ok();
            }
        }
    }
}