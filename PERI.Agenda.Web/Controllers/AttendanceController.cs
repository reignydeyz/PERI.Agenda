using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Attendance")]
    public class AttendanceController : Controller
    {
        [HttpGet("[action]")]
        [Route("{id}")]
        public async Task<IActionResult> Registrants(int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

            var res = from r in (await bll_a.Registrants(id))
                      select new
                      {
                            r.Member.Name,
                            r.MemberId,
                            r.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        [HttpPost("[action]")]
        [Route("{id}/Search")]
        public async Task<IActionResult> Search([FromBody] string member, int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

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
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

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

        [HttpPut("[action]")]
        [BLL.ValidateModelState]
        [Route("{id}/Add")]
        public async Task<int> Add([FromBody] Models.Attendance obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

            return await bll_a.Add(new EF.Attendance { EventId = id, MemberId = obj.MemberId.Value, DateTimeLogged = obj.DateTimeLogged ?? DateTime.Now });
        }

        [HttpPost("[action]")]
        [Route("{id}/Delete")]
        public async Task Delete([FromBody] EF.Attendance obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

            await bll_a.Delete(new EF.Attendance { EventId = id, MemberId = obj.MemberId });
        }

        [HttpGet("[action]")]
        [Route("{id}/Total/{status}")]
        public async Task<int> Total(int id, string status)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);
            var res = await bll_a.Registrants(id);

            if (status.ToLower() == "attendees")
                res = res.Where(x => x.DateTimeLogged != null);
            else if (status.ToLower() == "pending")
                res = res.Where(x => x.DateTimeLogged == null);

            return await res.CountAsync();
        }
    }
}