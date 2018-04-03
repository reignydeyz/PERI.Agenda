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
    [Route("api/GroupCategory")]
    public class GroupCategoryController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.GroupCategory args)
        {
            var context = new EF.AARSContext();
            var bll_gc = new BLL.GroupCategory(context);

            var res = from r in (await bll_gc.Find(args))
                      select new
                      {
                          r.Id,
                          r.Name,
                          Groups = r.Group.Count()
                      };

            return Json(res);
        }
    }
}