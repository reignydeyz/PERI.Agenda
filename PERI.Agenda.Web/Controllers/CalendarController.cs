﻿using System;
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
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public CalendarController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Events()
        {
            var bll_e = new BLL.Event(unitOfWork);

            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var ev = new EF.Event
            {
                DateTimeStart = DateTime.Today,
                DateTimeEnd = DateTime.Today.AddDays(30),
                EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId }
            };

            var res = from r in (await bll_e.Find(ev).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                      };

            return Json(res);
        }
    }
}