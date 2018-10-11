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
        private readonly IUnitOfWork unitOfWork;

        public CalendarController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [NonAction]
        private bool? IsGoing(EF.Rsvp args)
        {
            if (args == null)
                return null;
            else
            {
                return args.IsGoing;
            }
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
                          Event = r.Name,
                          Category = r.EventCategory.Name,
                          Location = r.Location.Name,
                          IsGoing = IsGoing(r.Rsvp.FirstOrDefault(x => x.MemberId == user.Member.Id))
                      };

            return Json(await res.OrderBy(x => x.DateTimeStart).ToListAsync());
        }
    }
}