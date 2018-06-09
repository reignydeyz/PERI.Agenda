using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.Core;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public EventController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody] EF.Event obj)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in (await bll_event.Find(obj).ToListAsync())
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

        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] EF.Event obj, int id)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_event.Find(obj)
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
            var page = id;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.events = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Download([FromBody] EF.Event obj)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in (await bll_event.Find(obj).ToListAsync())
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

            var bytes = Encoding.ASCII.GetBytes(res.ToList().ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<int> New([FromBody] Models.Event obj)
        {
            var bll_event = new BLL.Event(unitOfWork);

            var o = AutoMapper.Mapper.Map<EF.Event>(obj);

            return await bll_event.Add(o);
        }

        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_event.Get(new EF.Event { Id = id, EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId } });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.eventCategoryId = r.EventCategoryId;
            obj.category = r.EventCategory.Name;
            obj.name = r.Name;
            obj.isActive = r.IsActive;
            obj.dateTimeStart = r.DateTimeStart;
            obj.locationId = r.LocationId;
            obj.location = (r.Location == null ? "" : r.Location.Name);
            obj.attendance = r.Attendance.Count;

            return Json(obj);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task Edit([FromBody] EF.Event obj)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            await bll_event.Edit(obj);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_event = new BLL.Event(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_event.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_event.Delete(ids);

            return Json("Success!");
        }
    }
}