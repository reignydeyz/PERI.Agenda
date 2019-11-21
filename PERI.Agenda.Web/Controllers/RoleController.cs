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
        private readonly IRole roleBusiness;

        public RoleController(IRole role)
        {
            this.roleBusiness = role;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EF.Role>> GetAll()
        {
            return await roleBusiness.GetAll();
        }
    }
}