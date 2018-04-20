﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IEnumerable<EF.Member>> Find([FromBody] EF.Member obj)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            
            obj.CommunityId = user.CommunityId;

            return  await bll_member.Find(obj).ToListAsync();
        }

        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] EF.Member obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.CommunityId;

            var res = bll_member.Find(obj);
            var page = id;
            var pager = new Core.Pager(res.Count(), page == 0 ? 1 : page);

            dynamic obj1 = new ExpandoObject();
            obj1.members = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        [HttpPost("[action]")]
        public async Task<int> New([FromBody] EF.Member obj)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.Name = obj.Name.ToUpper();
            obj.CommunityId = user.CommunityId;

            return await bll_member.Add(obj);
        }
        
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<EF.Member> Get(int id)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            return await bll_member.Get(new EF.Member { Id = id, CommunityId = user.CommunityId });
        }

        [HttpPost("[action]")]
        public async Task Edit([FromBody] EF.Member obj)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.CommunityId;

            await bll_member.Edit(obj);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Delete(ids);

            return Json("Success!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Activate(ids);

            return Json("Success!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Deactivate(ids);

            return Json("Success!");
        }

        [HttpGet("[action]")]
        [Route("Total/{status}")]
        public async Task<int> Total(string status)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var members = await bll_member.Find(new EF.Member { CommunityId = user.CommunityId }).ToListAsync();

            if (status.ToLower() == "active")
                members = members.Where(x => x.IsActive == true).ToList();
            else if (status.ToLower() == "inactive")
                members = members.Where(x => x.IsActive == false).ToList();

            return members.Count();
        }
    }
}