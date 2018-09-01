using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTemplateController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public ReportTemplateController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }
        
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.ReportTemplate args)
        {
            var bll_rt = new BLL.Report(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Report>(args);
            o.CommunityId = user.Member.CommunityId.Value;
            o.DateCreated = DateTime.Now;
            o.CreatedBy = user.Member.Name;

            var id = await bll_rt.Add(o);
            return Ok(id);
        }
        
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.ReportTemplate args)
        {
            var bll_rt = new BLL.Report(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = AutoMapper.Mapper.Map<EF.Report>(args);
            o.CommunityId = user.Member.CommunityId.Value;
            o.DateModified = DateTime.Now;
            o.ModifiedBy = user.Member.Name;

            await bll_rt.Edit(o);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.Report args)
        {
            var bll_rt = new BLL.Report(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            args.CommunityId = user.Member.CommunityId.Value;

            var res = from r in (await bll_rt.Find(args).ToListAsync())
                      select new
                      {
                          r.ReportId,
                          r.Name
                      };

            return Json(res);
        }

        [HttpGet("[action]")]
        [Route("Checklist/{id}")]
        public async Task<IActionResult> Checklist(int id)
        {
            var bll_rt = new BLL.Report(unitOfWork);

            var res = from r in (await bll_rt.Checklist(id)
                      .OrderBy(x => x.Name)
                      .ToListAsync())
                      select new
                      {
                          r.Id,
                          r.Name,
                          IsSelected = r.EventCategoryReport.Count > 0
                      };

            return Json(res);
        }
    }
}