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
    [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
    [Produces("application/json")]
    [Route("api/Attendance")]
    public class AttendanceController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public AttendanceController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        /// <summary>
        /// Gets the Registrants from Event Id
        /// </summary>
        /// <param name="id">Event Id</param>
        /// <returns>List of registered Members</returns>
        [HttpGet("[action]")]
        [Route("{id}")]
        public async Task<IActionResult> Registrants(int id)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            var res = from r in (await bll_a.Registrants(id))
                      select new
                      {
                            r.Member.Name,
                            r.MemberId,
                            r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        [HttpGet("[action]")]
        [Route("{id}/Attendees")]
        public async Task<IActionResult> Attendees(int id)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

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
        /// Search or filter Registrants
        /// </summary>
        /// <param name="member">Member Name</param>
        /// <param name="id">Event Id</param>
        /// <returns>List of registered Members</returns>
        [HttpPost("[action]")]
        [Route("{id}/Search")]
        public async Task<IActionResult> Search([FromBody] string member, int id)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            var res = from r in (await bll_a.Registrants(id, member))
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }
        
        [HttpPost("[action]")]
        [Route("{id}/Search/Page/{p}")]
        public async Task<IActionResult> Page([FromBody] string member, int id, int p)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

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

        [HttpGet("[action]")]
        [Route("{id}/Attendees/Page/{p}")]
        public async Task<IActionResult> PageAttendees(int id, int p)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

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

        [HttpPut("[action]")]
        [BLL.ValidateModelState]
        [Route("{id}/Add")]
        public async Task<int> Add([FromBody] Models.Attendance obj, int id)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            return await bll_a.Add(new EF.Attendance { EventId = id, MemberId = obj.MemberId.Value, DateTimeLogged = obj.DateTimeLogged ?? DateTime.Now });
        }
        
        [HttpPost("[action]")]
        [Route("{id}/Delete")]
        public async Task Delete([FromBody] EF.Attendance obj, int id)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            await bll_a.Delete(new EF.Attendance { EventId = id, MemberId = obj.MemberId });
        }
        
        [HttpGet("[action]")]
        [Route("{id}/Total/{status}")]
        public async Task<int> Total(int id, string status)
        {
            var bll_a = new BLL.Attendance(unitOfWork);
            var res = await bll_a.Registrants(id);

            if (status.ToLower() == "attendees")
                return await bll_a.Find(new EF.Attendance { EventId = id }).CountAsync();
            else
                return await res.Where(x => x.DateTimeLogged == null).CountAsync();
        }
    }
}