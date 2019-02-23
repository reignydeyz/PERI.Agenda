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
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventCategoryReportController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public EventCategoryReportController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("[action]")]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] int[] ids, int id)
        {
            var bll_ecr = new BLL.EventCategoryReport(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var ecrs = new List<EF.EventCategoryReport>();

            foreach (var i in ids)
                ecrs.Add(new EF.EventCategoryReport { ReportId = id, EventCategoryId = i });

            await bll_ecr.Update(ecrs);
            return Ok();
        }

        [HttpPost("[action]")]
        [Route("AddRange/{id}")]
        public async Task<IActionResult> AddRange([FromBody] int[] ids, int id)
        {
            var bll_ecr = new BLL.EventCategoryReport(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var ecrs = new List<EF.EventCategoryReport>();

            foreach (var i in ids)
                ecrs.Add(new EF.EventCategoryReport { ReportId = id, EventCategoryId = i });

            await bll_ecr.AddRange(ecrs);
            return Ok();
        }
    }
}