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
        public async Task<IEnumerable<EF.Member>> Find([FromBody] Models.Member obj)
        {
            obj = obj ?? new Models.Member();

            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = await bll_member.Find(o).Where(x => x.IsActive == (obj.IsActive ?? x.IsActive)).ToListAsync();
            return res;
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

            var res = bll_member.Find(o);
            var page = id;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.members = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
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

        [BLL.VerifyUser(AllowedRoles = "Admin,Developer")]
        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<EF.Member> Get(int id)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            return await bll_member.Get(new EF.Member { Id = id, CommunityId = user.Member.CommunityId });
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.Member obj)
        {
            
            var bll_member = new BLL.Member(unitOfWork);
            var bll_user = new BLL.EndUser(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            await bll_member.Edit(o);

            // Add to User if member is not yet User
            if (!String.IsNullOrEmpty(obj.Email) && bll_user.Get(new EF.EndUser { Member = new EF.Member { Email = obj.Email } }) == null)
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
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = await bll_member.Find(o).ToListAsync();

            var bytes = Encoding.ASCII.GetBytes(res.ExportToCsv().ToString());

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