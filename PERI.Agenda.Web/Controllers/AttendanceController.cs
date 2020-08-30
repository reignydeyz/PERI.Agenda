using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;
using PERI.Agenda.Core;
using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiVersion("1.0")]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Attendance")]
    public class AttendanceController : Controller
    {
        private readonly IAttendance attendanceBusiness;
        private readonly IEvent eventBusiness;
        private readonly IFirstTimer firstTimerBusiness;
        private IHubContext<SignalRHub, ITypedHubClient> _hubContext;

        public AttendanceController(IHubContext<SignalRHub, 
            ITypedHubClient> hubContext, 
            IAttendance attendance,
            IEvent eventBusiness,
            IFirstTimer firstTimer)
        {
            this.attendanceBusiness = attendance;
            this.eventBusiness = eventBusiness;
            this.firstTimerBusiness = firstTimer;

            _hubContext = hubContext;
        }

        /// <summary>
        /// Gets the Registrants from Event Id
        /// </summary>
        /// <param name="id">Event Id</param>
        /// <returns>List of registered Members</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Registrants(int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in (await bll_a.Registrants(id))
                      select new
                      {
                            r.Member.Name,
                            r.MemberId,
                            r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Gets the attendees of the event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/Attendees")]
        public async Task<IActionResult> Attendees(int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in bll_a.Find(new EF.Attendance { EventId = id }).OrderBy(x => x.Member.Name)
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Gets the fisrt-timers from the event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/FirstTimers")]
        public async Task<IActionResult> FirstTimers(int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in bll_a.Find(new EF.Attendance { EventId = id }).OrderBy(x => x.Member.Name)
                      where r.FirstTimer != null
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Search or filter Registrants
        /// </summary>
        /// <param name="member">Member Name</param>
        /// <param name="id">Event Id</param>
        /// <returns>List of registered Members</returns>
        [HttpPost]
        [Route("{id}/Search")]
        public async Task<IActionResult> Search([FromBody] string member, int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in (await bll_a.Registrants(id, member))
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Search or filter Registrants (with pagination)
        /// </summary>
        /// <param name="member"></param>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Search/Page/{p}")]
        public async Task<IActionResult> Page([FromBody] string member, int id, int p)
        {
            var bll_a = attendanceBusiness;

            var res = from r in (await bll_a.Registrants(id, member))
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            var page = p;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.registrants = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        /// <summary>
        /// Gets the attendees of the event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/Attendees/Page/{p}")]
        public async Task<IActionResult> PageAttendees(int id, int p)
        {
            var bll_a = attendanceBusiness;

            var res = from r in bll_a.Find(new EF.Attendance { EventId = id }).OrderBy(x => x.Member.Name)
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            var page = p;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.attendees = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        /// <summary>
        /// Logs an attendee of an event
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [BLL.ValidateModelState]
        [Route("{id}/Add")]
        public async Task<int> Add([FromBody] Models.Attendance obj, int id)
        {
            var bll_event = eventBusiness;
            var bll_a = attendanceBusiness;
            var bll_ft = firstTimerBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_event.IsSelectedIdsOk(new int[] { id }, user))
                throw new ArgumentException("Event Id is invalid.");

            var attendanceid = await bll_a.Add(new EF.Attendance { EventId = id, MemberId = obj.MemberId.Value, DateTimeLogged = obj.DateTimeLogged ?? DateTime.Now });

            await bll_ft.ValidateThenAdd(new EF.FirstTimer { AttendanceId = attendanceid });

            await _hubContext.Clients.Group(id.ToString()).AttendanceBroadcast(new Models.Attendance { MemberId = obj.MemberId, EventId = id, DateTimeLogged = obj.DateTimeLogged ?? DateTime.Now });

            return attendanceid;
        }

        /// <summary>
        /// Deletes an attendee of an event
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Delete")]
        public async Task Delete([FromBody] EF.Attendance obj, int id)
        {
            var bll_event = eventBusiness;
            var bll_a = attendanceBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_event.IsSelectedIdsOk(new int[] { id }, user))
                throw new ArgumentException("Event Id is invalid.");

            await bll_a.Delete(new EF.Attendance { EventId = id, MemberId = obj.MemberId });

            await _hubContext.Clients.Group(id.ToString()).AttendanceBroadcast(new Models.Attendance { MemberId = obj.MemberId, EventId = id });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("{id}/Total/{status}")]
        public async Task<int> Total(int id, string status)
        {
            var bll_a = attendanceBusiness;
            var res = await bll_a.Registrants(id);

            if (status.ToLower() == "attendees")
                return await bll_a.Find(new EF.Attendance { EventId = id }).CountAsync();
            else if (status.ToLower() == "pending")
                return await res.Where(x => x.DateTimeLogged == null).CountAsync();
            else
                return await res.CountAsync();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("{id}/DownloadAttendees")]
        public async Task<IActionResult> DownloadAttendees(int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in bll_a.Find(new EF.Attendance { EventId = id }).OrderBy(x => x.Member.Name)
                      select new
                      {
                          r.Member.Name,
                          r.DateTimeLogged
                      };

            var bytes = Encoding.ASCII.GetBytes((await res.ToListAsync()).ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("{id}/DownloadFirstTimers")]
        public async Task<IActionResult> DownloadFirstTimers(int id)
        {
            var bll_a = attendanceBusiness;

            var res = from r in bll_a.Find(new EF.Attendance { EventId = id }).OrderBy(x => x.Member.Name)
                      where r.FirstTimer != null
                      select new
                      {
                          r.Member.Name,
                          r.DateTimeLogged
                      };

            var bytes = Encoding.ASCII.GetBytes((await res.ToListAsync()).ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }
    }
}