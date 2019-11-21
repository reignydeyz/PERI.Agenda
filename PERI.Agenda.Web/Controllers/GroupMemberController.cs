using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;
using PERI.Agenda.Core;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/GroupMember")]
    public class GroupMemberController : Controller
    {
        private readonly IGroup groupBusiness;
        private readonly IGroupMember groupMemberBusiness;
        private readonly IMember memberBusiness;
        private readonly ILookUp lookUpBusiness;

        public GroupMemberController(IGroup group, 
            IGroupMember groupMember,
            IMember member,
            ILookUp lookUp)
        {
            this.groupBusiness = group;
            this.groupMemberBusiness = groupMember;
            this.memberBusiness = member;
            this.lookUpBusiness = lookUp;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        [Route("Find/{id}")]
        public async Task<IActionResult> Members([FromBody] EF.Member obj, int id)
        {
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            return Json(await bll_gm.Members(obj, id).ToListAsync());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        [Route("{id}/Checklist/Page/{p}")]
        public async Task<IActionResult> Page([FromBody] string member, int id, int p)
        {
            var bll_g = groupBusiness;
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(new int[] { id }, user))
                return Unauthorized();

            var m = new EF.Member
            {
                Name = member,
                CommunityId = user.Member.CommunityId
            };

            var res = from r in bll_gm.Checklist(m, id)
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.GroupId
                      };

            var page = p;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.checklist = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        /// <summary>
        /// Checklist of the group (for member assignment)
        /// </summary>
        /// <param name="member"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Checklist")]
        public async Task<IActionResult> Checklist([FromBody] string member, int id)
        {
            var bll_g = groupBusiness;
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(new int[] { id }, user))
                return Unauthorized();

            var m = new EF.Member
            {
                Name = member,
                CommunityId = user.Member.CommunityId
            };

            var res = from r in bll_gm.Checklist(m, id)
                      select new
                      {
                          r.Member.Name,
                          r.MemberId,
                          r.GroupId
                      };

            return Json(await res.ToListAsync());
        }

        /// <summary>
        /// Download members of the group
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CSV</returns>
        [HttpGet]
        [Route("{id}/Download")]
        public async Task<IActionResult> Download(int id)
        {
            var bll_g = groupBusiness;
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(new int[] { id }, user))
                return Unauthorized();

            var res = from r in await bll_gm.Members(new EF.Member(), id).ToListAsync()
                      join genders in (await lookUpBusiness.GetByGroup("Gender")).Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.Gender equals genders.Value into e
                      from e1 in e.DefaultIfEmpty()
                      join civilStatuses in (await lookUpBusiness.GetByGroup("Civil Status")).Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.CivilStatus equals civilStatuses.Value into f
                      from f1 in f.DefaultIfEmpty()
                      join m in memberBusiness.Find(new EF.Member { CommunityId = user.Member.CommunityId }) on r.InvitedBy equals m.Id into g
                      from m1 in g.DefaultIfEmpty()
                      select new
                      {
                          r.Id,
                          r.Name,
                          r.NickName,
                          r.Address,
                          r.Mobile,
                          r.Email,
                          r.BirthDate,
                          r.Remarks,
                          r.CivilStatus,
                          r.Gender,
                          InvitedByMemberName = m1 == null ? "" : m1.Name,
                          r.IsActive
                      };

            var bytes = Encoding.ASCII.GetBytes(res.ToList().ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        /// <summary>
        /// Adds a member to a group
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [BLL.ValidateModelState]
        [Route("Add")]
        public async Task<int> Add([FromBody] Models.GroupMember obj)
        {
            var bll_g = groupBusiness;
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(new int[] { obj.GroupId }, user))
                throw new ArgumentException("Group Id is invalid.");

            return await bll_gm.Add(new EF.GroupMember {
                GroupId = obj.GroupId,
                MemberId = obj.MemberId == null || obj.MemberId == 0 ? user.MemberId : obj.MemberId
            });
        }

        /// <summary>
        /// Removes a member from a group
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [BLL.ValidateModelState]
        [Route("Delete")]
        public async Task Delete([FromBody] Models.GroupMember obj)
        {
            var bll_g = groupBusiness;
            var bll_gm = groupMemberBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_g.IsSelectedIdsOk(new int[] { obj.GroupId }, user))
                throw new ArgumentException("Group Id is invalid.");

            await bll_gm.Delete(new EF.GroupMember {
                GroupId = obj.GroupId,
                MemberId = obj.MemberId == null || obj.MemberId == 0 ? user.MemberId : obj.MemberId
            });
        }
    }
}