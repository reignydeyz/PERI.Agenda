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
    [Route("api/LookUp")]
    public class LookUpController : Controller
    {
        [HttpGet("[action]")]
        [Route("Get/{group}")]
        public async Task<IEnumerable<EF.LookUp>> Get(string group)
        {
            var context = new EF.AARSContext();
            var bll_lookup = new BLL.LookUp(context);

            return await bll_lookup.GetByGroup(group);
        }
    }
}