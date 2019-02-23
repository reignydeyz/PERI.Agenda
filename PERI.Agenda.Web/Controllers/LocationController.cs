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
    [Route("api/Location")]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.Location args)
        {
            var bll_l = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Location>(args);
            o.CommunityId = user.Member.CommunityId;
            o.DateTimeCreated = DateTime.Now;
            o.CreatedBy = user.Member.Name;

            var id = await bll_l.Add(o);

            return Ok(id);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.Location obj)
        {
            var bll_l = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Location>(obj);
            o.DateTimeModified = DateTime.Now;
            o.ModifiedBy = user.Member.Name;

            await bll_l.Edit(o);
        }
        
        [HttpPost]
        [Route("find")]
        public async Task<IActionResult> Find(EF.Location args)
        {
            var bll_location = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;

            var res = from r in (await bll_location.Find(args).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.Name,
                          r.Address,
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
            var bll_l = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_l.Get(new EF.Location { Id = id, CommunityId = user.Member.CommunityId });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.name = r.Name;
            obj.address = r.Address;
            obj.events = r.Event.Count;
            obj.minAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count);
            obj.averageAttendees = Convert.ToInt16(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count));
            obj.maxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count);

            return Json(obj);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_l = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_l.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_l.Delete(ids);

            return Json("Success!");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpGet("[action]")]
        [Route("Stats/{id}")]
        public async Task<Models.Graph.GraphDataSet> Stats(int id)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var res = bll_event.Find(new EF.Event { EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId }, LocationId = id }).Take(20);

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

            var res = from r in (await bll_event.Find(new EF.Event { EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId }, LocationId = id }).Take(20).ToListAsync())
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