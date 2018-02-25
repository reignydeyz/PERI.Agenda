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
        public async Task<IEnumerable<EF.EventCategory>> Find(EF.EventCategory args)
        {
            var context = new EF.aarsdbContext();
            var bll_eventCategory = new BLL.EventCategory(context);

            var res = await bll_eventCategory.Find(args);
            return res;
        }
    }
}