using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTemplateController : Controller
    {
        private readonly IReport reportBusiness;
        private readonly IEventCategory eventCategoryBusiness;
        private readonly IMapper mapper;

        public ReportTemplateController(IReport report, IEventCategory eventCategory, IMapper mapper)
        {
            this.reportBusiness = report;
            this.eventCategoryBusiness = eventCategory;
            this.mapper = mapper;
        }
        
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.ReportTemplate args)
        {
            var bll_rt = reportBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Report>(args);
            o.CommunityId = user.Member.CommunityId.Value;
            o.DateCreated = DateTime.Now;
            o.CreatedBy = user.Member.Name;

            var id = await bll_rt.Add(o);
            return Ok(id);
        }
        
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> Edit([FromBody] Models.ReportTemplate args) 
        {
            var bll_rt = reportBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Report>(args);
            o.CommunityId = user.Member.CommunityId.Value;
            o.DateModified = DateTime.Now;
            o.ModifiedBy = user.Member.Name;

            await bll_rt.Edit(o);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_rt = reportBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_rt.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_rt.Delete(ids);

            return Json("Success!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Find(EF.Report args)
        {
            var bll_rt = reportBusiness;
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
            var bll_ec = eventCategoryBusiness;
            var bll_rt = reportBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var categories = await bll_ec.Find(new EF.EventCategory { CommunityId = user.Member.CommunityId }).ToListAsync();
            var report = await bll_rt.GetById(id);

            var res = from c in categories
                      join ecr in report.EventCategoryReport on c.Id equals ecr.EventCategoryId into ps
                      from ecr in ps.DefaultIfEmpty()
                      select new
                      {
                          c.Id,
                          c.Name,
                          IsSelected = ecr != null
                      };

            return Json(res);
        }
    }
}