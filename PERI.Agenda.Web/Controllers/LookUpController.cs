using Microsoft.AspNetCore.Mvc;
using PERI.Agenda.BLL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/LookUp")]
    public class LookUpController : Controller
    {
        private readonly ILookUp lookUpBusiness;

        public LookUpController(ILookUp lookUp)
        {
            this.lookUpBusiness = lookUp;
        }

        [HttpGet("[action]")]
        [Route("Get/{group}")]
        public async Task<IEnumerable<EF.LookUp>> Get(string group)
        {
            var bll_lookup = lookUpBusiness;

            return await bll_lookup.GetByGroup(group);
        }
    }
}