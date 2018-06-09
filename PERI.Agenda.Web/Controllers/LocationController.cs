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
    [Route("api/Location")]
    public class LocationController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public LocationController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.Location args)
        {
            var bll_location = new BLL.Location(unitOfWork);
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