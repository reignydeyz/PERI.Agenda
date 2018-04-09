using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            return Json(res);
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

            return Json(res);
        }

        [HttpPut("[action]")]
        [Route("{id}/Add")]
        public async Task<int> Add([FromBody] EF.Attendance obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

            return await bll_a.Add(new EF.Attendance { EventId = id, MemberId = obj.MemberId, DateTimeLogged = obj.DateTimeLogged ?? DateTime.Now });
        }

        [HttpPost("[action]")]
        [Route("{id}/Delete")]
        public async Task Delete([FromBody] EF.Attendance obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_a = new BLL.Attendance(context);

            await bll_a.Delete(new EF.Attendance { EventId = id, MemberId = obj.MemberId });
        }
    }
}