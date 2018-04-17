﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/EventCategory")]
    public class EventCategoryController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody]EF.EventCategory args)
        {
            var context = new EF.AARSContext();
            var bll_eventCategory = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (args == null)
                args = new EF.EventCategory();

            args.CommunityId = user.CommunityId;

            var res = from r in (await bll_eventCategory.Find(args))
                      select new
                      {
                          r.Id,
                          r.Name,
                          Events = r.Event.Count,
                          MinAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count),
                          AverageAttendees = Convert.ToInt16(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count)),
                          MaxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count)
                      };

            return Json(res);
        }

        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var context = new EF.AARSContext();
            var bll_eventCategory = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_eventCategory.Get(new EF.EventCategory { Id = id, CommunityId = user.CommunityId });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;           
            obj.name = r.Name;
            obj.events = r.Event.Count;
            obj.minAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count);
            obj.averageAttendees = Convert.ToInt16(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count));
            obj.maxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count);

            return Json(obj);
        }
    }
}