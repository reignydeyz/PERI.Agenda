using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EF.AARSContext context;
        private readonly IUnitOfWork unitOfWork;

        public RoleController(IUnitOfWork unitOfWork, EF.AARSContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EF.Role>> GetAll()
        {
            var bll_r = new BLL.Role(unitOfWork);
            return await bll_r.GetAll();
        }
    }
}