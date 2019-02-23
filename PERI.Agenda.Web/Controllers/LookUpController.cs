using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/LookUp")]
    public class LookUpController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public LookUpController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        [Route("Get/{group}")]
        public async Task<IEnumerable<EF.LookUp>> Get(string group)
        {
            var bll_lookup = new BLL.LookUp(unitOfWork);

            return await bll_lookup.GetByGroup(group);
        }
    }
}