using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/EventCategory")]
    public class EventCategoryController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.EventCategory args)
        {
            var context = new EF.aarsdbContext();
            var bll_eventCategory = new BLL.EventCategory(context);

            var res = from r in (await bll_eventCategory.Find(args))
                      select new
                      {
                          r.Id,
                          r.Name,
                          Events = r.Event.Count
                      };

            return Json(res);
        }
    }
}