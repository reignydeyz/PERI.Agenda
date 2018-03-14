using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Location")]
    public class LocationController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.Location args)
        {
            var context = new EF.AARSContext();
            var bll_location = new BLL.Location(context);

            var res = from r in (await bll_location.Find(args))
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