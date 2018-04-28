using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Location")]
    public class LocationController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.Location args)
        {
            var context = new EF.AARSContext();
            var bll_location = new BLL.Location(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId;

            var res = from r in (await bll_location.Find(args).ToListAsync())
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