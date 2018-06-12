using System;
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

            var res = from r in bll_e.Calendar(user.Member.Id, user.Member.CommunityId.Value)
                      select new
                      {
                          r.Id,
                          r.DateTimeStart,
                          r.Name,
                          Category = r.EventCategory.Name,
                          Location = r.Location.Name
                      };

            return Json(await res.OrderBy(x => x.DateTimeStart).ToListAsync());
        }
    }
}