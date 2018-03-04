using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/LookUp")]
    public class LookUpController : Controller
    {
        [HttpGet("[action]")]
        public async Task<IEnumerable<EF.LookUp>> FindByGroup()
        {
            var group = Request.Query["group"].ToString();

            var context = new EF.AARSContext();
            var bll_lookup = new BLL.LookUp(context);

            return await bll_lookup.GetByGroup(group);
        }
    }
}