﻿using System;
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

        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_gc.Get(new EF.GroupCategory { Id = id, CommunityId = user.Member.CommunityId });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.name = r.Name;
            obj.groups = r.Group.Count();
            obj.members = r.Group.Sum(x => x.GroupMember.Select(y => y.MemberId).Distinct().Count());

            return Json(obj);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<int> New([FromBody]EF.GroupCategory args)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;

            return await bll_gc.Add(args);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Edit([FromBody]EF.GroupCategory args)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;
            args.DateTimeModified = DateTime.Now;
            args.ModifiedBy = user.Member.Name;

            await bll_gc.Edit(args);

            return Json("Success!");
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_gc.AreSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_gc.Delete(ids);

            return Json("Success!");
        }
    }
}