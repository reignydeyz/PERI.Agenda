using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/ActivityReport")]
    public class ActivityReportController : Controller
    {
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public IActionResult GenerateReport([FromBody] Models.ActivityReport args)
        {
            return View();
        }
    }
}