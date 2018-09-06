using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PERI.Agenda.Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using NLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Emailer smtp;
        private readonly EF.AARSContext context;
        private readonly UnitOfWork unitOfWork;

        public MemberController(IOptions<Core.Emailer> settingsOptions)
        {
            smtp = settingsOptions.Value;
            context = new EF.AARSContext();
            unitOfWork = new UnitOfWork(context);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Find([FromBody] Models.Member obj)
        {
            obj = obj ?? new Models.Member();

            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = from r in await bll_member.Find(o).Where(x => x.IsActive == (obj.IsActive ?? x.IsActive)).ToListAsync()
                      join m in bll_member.Find(new EF.Member { CommunityId = obj.CommunityId }) on r.InvitedBy equals m.Id into g
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
            return Json(res);
        }

        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] Models.Member obj, int id)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId.Value;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            if (obj.RoleId != null)
                o.EndUser = new EF.EndUser { RoleId = obj.RoleId.Value };

            var members = bll_member.Find(o).Where(x => x.IsActive == (obj.IsActive ?? x.IsActive));
            var page = id;
            var pager = new Core.Pager(await members.CountAsync(), page == 0 ? 1 : page, 100);

            var res = from r in await members.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync()
                      join m in bll_member.Find(new EF.Member { CommunityId = obj.CommunityId }) on r.InvitedBy equals m.Id into g
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
                          r.IsActive,
                          RoleId = r.EndUser == null ? 0 : r.EndUser.RoleId
                      };

            dynamic obj1 = new ExpandoObject();
            obj1.members = res;
            obj1.pager = pager;

            return Json(obj1);
        }

        [BLL.VerifyUser]
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> New([FromBody] Models.Member args)
        {
            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    var bll_member = new BLL.Member(unitOfWork);
                    var user = HttpContext.Items["EndUser"] as EF.EndUser;

                    var m = AutoMapper.Mapper.Map<EF.Member>(args);
                    m.IsActive = true;
                    m.CommunityId = user.Member.CommunityId.Value;
                    m.CreatedBy = user.Member.Name;
                    m.DateCreated = DateTime.Now;

                    // Check and get invited by
                    if (!String.IsNullOrEmpty(args.InvitedByMemberName))
                    {
                        var ibid = await bll_member.GetIdByName(args.InvitedByMemberName ?? "", user.Member.CommunityId.Value);

                        if (ibid == null)
                        {
                            return new ObjectResult("Invalid Invited by.")
                            {
                                StatusCode = 403,
                                Value = "Invalid Invited by."
                            };
                        }
                        else
                            m.InvitedBy = ibid;
                    }

                    var memberId = await bll_member.Add(m);

                    // Add to User
                    if (args.Email != null && args.Email != "")
                    {
                        // Generate ConfirmationCode
                        Guid g = Guid.NewGuid();
                        string guidString = Convert.ToBase64String(g.ToByteArray());
                        guidString = guidString.Replace("=", "");
                        guidString = guidString.Replace("+", "");

                        var bll_user = new BLL.EndUser(unitOfWork);
                        var newId = await bll_user.Add(new EF.EndUser
                        {
                            MemberId = memberId,
                            ConfirmationCode = guidString
                        });

                        // Send email
                        await smtp.SendEmail(args.Email,
                            "Your Agenda Credentials",
                            "Please click the link below to validate and change your password:<br/>http://" + Request.Host.Value + "/authentication/newpassword/?userid=" + newId + "&code=" + guidString);
                    }

                    txn.Commit();

                    return Ok(memberId);
                }
                catch (ArgumentException ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    return new ObjectResult(ex.Message)
                    {
                        StatusCode = 403,
                        Value = "Existing name or email."
                    };
                }
                catch (DbUpdateException ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    return new ObjectResult(ex.Message)
                    {
                        StatusCode = 403,
                        Value = "Existing name or email."
                    };
                }
                catch (Exception ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    return new ObjectResult(ex.Message)
                    {
                        StatusCode = 403,
                        Value = "Entry is invalid."
                    };
                }
            }
        }

        [BLL.VerifyUser]
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(new int[] { id }, user))
                return BadRequest();

            var r = await bll_member.Get(new EF.Member { Id = id, CommunityId = user.Member.CommunityId });
            return Json(new Models.Member
                {
                    Address = r.Address,
                    BirthDate = r.BirthDate,
                    CivilStatus = r.CivilStatus,
                    CommunityId = r.CommunityId,
                    Email = r.Email,
                    Gender = r.Gender,
                    Id = r.Id,
                    InvitedBy = r.InvitedBy,
                    InvitedByMemberName = r.InvitedBy > 0 ? (await bll_member.GetById(r.InvitedBy.Value)).Name : "",
                    IsActive = r.IsActive,
                    Mobile = r.Mobile,
                    Name = r.Name,
                    NickName = r.NickName,
                    Remarks = r.Remarks
                });
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> Edit([FromBody] Models.Member obj)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var bll_user = new BLL.EndUser(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            if (!await bll_member.IsSelectedIdsOk(new int[] { obj.Id }, user))
                return BadRequest();

            // Check and get invited by
            if (!String.IsNullOrEmpty(obj.InvitedByMemberName))
            {
                var ibid = await bll_member.GetIdByName(obj.InvitedByMemberName ?? "", user.Member.CommunityId.Value);

                if (ibid != null)
                    o.InvitedBy = ibid;
            }

            await bll_member.Edit(o);

            // Add to User if member is not yet User
            if (!String.IsNullOrEmpty(obj.Email) && await bll_user.GetByEmail(obj.Email) == null)
            {
                // Generate ConfirmationCode
                Guid g = Guid.NewGuid();
                string guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                
                var newId = await bll_user.Add(new EF.EndUser
                {
                    MemberId = obj.Id,
                    ConfirmationCode = guidString
                });

                // Send email
                await smtp.SendEmail(obj.Email,
                    "Your Agenda Credentials",
                    "Please click the link below to validate and change your password:<br/>http://" + Request.Host.Value + "/authentication/newpassword/?userid=" + newId + "&code=" + guidString);
            }

            return Ok();
        }

        [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Delete(ids);

            return Json("Success!");
        }

        [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Activate(ids);

            return Json("Success!");
        }

        [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Deactivate(ids);

            return Json("Success!");
        }

        [HttpGet("[action]")]
        [Route("{id}/Leading")]
        public async Task<int> Leading(int id)
        {
            var bll_member = new BLL.Member(unitOfWork);
            return await bll_member.Leading(id);
        }

        [HttpGet("[action]")]
        [Route("{id}/Following")]
        public async Task<int> Following(int id)
        {
            var bll_member = new BLL.Member(unitOfWork);
            return await bll_member.Following(id);
        }

        [HttpGet("[action]")]
        [Route("{id}/Invites")]
        public async Task<int> Invites(int id)
        {
            var bll_member = new BLL.Member(unitOfWork);
            return await bll_member.Invites(id);
        }

        [HttpGet("[action]")]
        [Route("{id}/Activities")]
        public async Task<IActionResult> Activities(int id)
        {
            var bll_member = new BLL.Member(unitOfWork);
            var res = from a in bll_member.Activities(id)
                      .OrderByDescending(x => x.Event.DateTimeStart)
                      .Take(20)
                      select new
                      {
                          a.EventId,
                          Category = a.Event.EventCategory.Name,
                          Event = a.Event.Name,
                          EventDate = a.Event.DateTimeStart,
                          TimeLogged = a.DateTimeLogged
                      };

            return Json(await res.ToListAsync());
        }

        [HttpGet("[action]")]
        [Route("Total/{status}")]
        public async Task<int> Total(string status)
        {

            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var members = bll_member.Find(new EF.Member { CommunityId = user.Member.CommunityId });

            if (status.ToLower() == "active")
                return await members.Where(x => x.IsActive == true).CountAsync();
            else if (status.ToLower() == "inactive")
                return await members.Where(x => x.IsActive == false).CountAsync();
            else
                return await members.CountAsync();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Download([FromBody] Models.Member obj)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var bll_lu = new BLL.LookUp(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = from r in await bll_member.Find(o).Where(x => x.IsActive == (obj.IsActive ?? x.IsActive)).ToListAsync()
                      join genders in (await bll_lu.GetByGroup("Gender")).Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.Gender equals genders.Value into e
                      from e1 in e.DefaultIfEmpty()
                      join civilStatuses in (await bll_lu.GetByGroup("Civil Status")).Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.CivilStatus equals civilStatuses.Value into f
                      from f1 in f.DefaultIfEmpty()
                      join m in bll_member.Find(new EF.Member { CommunityId = user.Member.CommunityId }) on r.InvitedBy equals m.Id into g
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
                          CivilStatus = f1 == null ? "" : f1.Label,
                          Gender = e1 == null ? "" : e1.Label,
                          InvitedBy = m1 == null ? "" : m1.Name,
                          r.IsActive,
                          r.DateCreated
                      };

            var bytes = Encoding.ASCII.GetBytes(res.ToList().ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AllNames()
        {
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var res = await bll_member.Find(new EF.Member { CommunityId = user.Member.CommunityId }).Select(x => x.Name).ToListAsync();

            return Json(res);
        }
    }
}