using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
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
    }
}