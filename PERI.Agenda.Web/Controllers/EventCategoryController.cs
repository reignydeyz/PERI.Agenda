using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using PERI.Agenda.Core;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/EventCategory")]
    public class EventCategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public EventCategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Searches event categories
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public async Task<IActionResult> Find([FromBody]EF.EventCategory args)
        {
            var bll_eventCategory = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (args == null)
                args = new EF.EventCategory();

            args.CommunityId = user.Member.CommunityId;

            var res = from r in (await bll_eventCategory.Find(args).ToListAsync())
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_eventCategory = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_eventCategory.Get(new EF.EventCategory { Id = id, CommunityId = user.Member.CommunityId });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;           
            obj.name = r.Name;
            obj.events = r.Event.Count;
            obj.minAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count);
            obj.averageAttendees = Convert.ToInt16(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count));
            obj.maxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count);

            return Json(obj);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.EventCategory args)
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.EventCategory>(args);
            o.CommunityId = user.Member.CommunityId;
            o.DateTimeCreated = DateTime.Now;
            o.CreatedBy = user.Member.Name;

            var id = await bll_ec.Add(o);

            return Ok(id);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.EventCategory obj)
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.EventCategory>(obj);
            o.DateTimeModified = DateTime.Now;
            o.ModifiedBy = user.Member.Name;

            await bll_ec.Edit(o);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_ec.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_ec.Delete(ids);

            return Json("Success!");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> Download()
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var res = from r in (await bll_ec.Find(new EF.EventCategory { CommunityId = user.Member.CommunityId }).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.Name,
                          Events = r.Event.Count,
                          MinAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count),
                          AverageAttendees = Convert.ToInt16(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count)),
                          MaxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count)
                      };

            var bytes = Encoding.ASCII.GetBytes(res.ToList().ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpGet("[action]")]
        [Route("Stats/{id}")]
        public async Task<Models.Graph.GraphDataSet> Stats(int id)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var res = bll_event.Find(new EF.Event { EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId }, EventCategoryId = id }).Take(20);
            
            var list = new List<Models.Graph.GraphData>();
            list.Add(new Models.Graph.GraphData { Label = "Attendance", Data = await res.OrderBy(x => x.DateTimeStart).Select(x => x.Attendance.Count).ToArrayAsync() });

            return new Models.Graph.GraphDataSet
            {
                DataSet = list,
                ChartLabels = await res.OrderBy(x => x.DateTimeStart).Select(x => x.Name).ToArrayAsync()
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpGet("[action]")]
        [Route("Events/{id}")]
        public async Task<IActionResult> Events(int id)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var res = from r in (await bll_event.Find(new EF.Event { EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId }, EventCategoryId = id }).Take(20).ToListAsync())
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