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

        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var context = new EF.AARSContext();
            var bll_eventCategory = new BLL.EventCategory(context);
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

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.EventCategory args)
        {
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.EventCategory>(args);
            o.CommunityId = user.Member.CommunityId;
            o.DateTimeCreated = DateTime.Now;
            o.CreatedBy = user.Member.Name;

            var id = await bll_ec.Add(o);

            return Ok(id);
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.EventCategory obj)
        {
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.EventCategory>(obj);
            o.DateTimeModified = DateTime.Now;
            o.ModifiedBy = user.Member.Name;

            await bll_ec.Edit(o);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_ec.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_ec.Delete(ids);

            return Json("Success!");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Download()
        {
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);
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

        [HttpGet("[action]")]
        [Route("Stats/{id}")]
        public async Task<Models.Graph.GraphDataSet> Stats(int id)
        {
            var context = new EF.AARSContext();
            var bll_event = new BLL.Event(context);
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
    }
}