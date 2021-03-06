﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;
using PERI.Agenda.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiVersion("1.0")]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Group")]
    public class GroupController : Controller
    {
        private readonly IGroup groupBusiness;
        private readonly IGroupCategory groupCategoryBusiness;
        private readonly IMember memberBusiness;
        private readonly IMapper mapper;

        public GroupController(IGroup group, IGroupCategory groupCategory, IMember member, IMapper mapper)
        {
            this.groupBusiness = group;
            this.groupCategoryBusiness = groupCategory;
            this.memberBusiness = member;
            this.mapper = mapper;
        }

        /// <summary>
        /// Searches members
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public async Task<IActionResult> Find([FromBody] Models.Group obj)
        {
            var bll_g = groupBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Group>(obj);

            o.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            // Get group leader id
            var bll_m = memberBusiness;
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

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Searches members (with pagination)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] Models.Group obj, int id)
        {
            var bll_g = groupBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Group>(obj);

            o.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            // Get group leader id
            var bll_m = memberBusiness;
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


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_m = memberBusiness;
            var bll_g = groupBusiness;
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

        /// <summary>
        /// Adds a new group
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("new")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.Group obj)
        {
            var bll_g = groupBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Group>(obj);

            // Get group leader id
            var bll_m = memberBusiness;
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> Edit([FromBody] Models.Group obj)
        {
            var bll_g = groupBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Group>(obj);

            // Get group leader id
            var bll_m = memberBusiness;
            var glid = await bll_m.GetIdByName(obj.Leader ?? user.Member.Name, user.Member.CommunityId.Value);

            if (!await bll_g.IsSelectedIdsOk(new int[] { obj.Id }, user))
                return BadRequest();

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

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {

            var bll_g = groupBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_g.Delete(ids);

            return Json("Success!");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Download([FromBody] EF.Group obj)
        {
            var bll_g = groupBusiness;
            var bll_m = memberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.GroupCategory = new EF.GroupCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_g.Find(obj)
                      join m in bll_m.Find(new EF.Member { CommunityId = user.Member.CommunityId }) on r.GroupLeader equals m.Id
                      select new
                      {
                          Category = r.GroupCategory.Name,
                          r.Name,
                          Leader = m.Name,
                          Members = r.GroupMember.Count
                      };

            var bytes = Encoding.ASCII.GetBytes((await res.ToListAsync()).ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }
    }
}