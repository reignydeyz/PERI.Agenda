using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody] EF.Event obj)
        {
            var context = new EF.AARSContext();
            var bll_event = new BLL.Event(context);

            var res = from r in (await bll_event.Find(obj)).Take(1000)
                      select new
                        {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count
                        };

            return Json(res);
        }
    }
}