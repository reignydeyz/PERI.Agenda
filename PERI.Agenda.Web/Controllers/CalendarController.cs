using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiVersion("1.0")]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {
        private readonly IEvent eventBusiness;

        public CalendarController(IEvent eventBusiness)
        {
            this.eventBusiness = eventBusiness;
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

        /// <summary>
        /// Gets the happening/upcoming events
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> Events()
        {
            var bll_e = eventBusiness;

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