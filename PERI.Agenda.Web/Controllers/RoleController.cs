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
    [BLL.VerifyUser]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EF.AARSContext context;
        private readonly UnitOfWork unitOfWork;

        public RoleController()
        {
            context = new EF.AARSContext();
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EF.Role>> GetAll()
        {
            var bll_r = new BLL.Role(unitOfWork);
            return await bll_r.GetAll();
        }
    }
}