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

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser(AllowedRoles ="Admin,Developer")]
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Core.Setting smtpSettings { get; set; }

        public MemberController(IOptions<Core.Setting> settings)
        {
            smtpSettings = settings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<EF.Member>> Find([FromBody] Models.Member obj)
        {
            obj = obj ?? new Models.Member();

            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = await bll_member.Find(o).Where(x => x.IsActive == (obj.IsActive ?? x.IsActive)).ToListAsync();
            return res;
        }

        [HttpPost("[action]")]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] Models.Member obj, int id)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId.Value;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

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
        public async Task<IActionResult> New([FromBody] Models.Member obj)
        {
            var context = new EF.AARSContext();

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    var bll_member = new BLL.Member(context);
                    var user = HttpContext.Items["EndUser"] as EF.EndUser;

                    // Validate if existing
                    var exist = await bll_member.Search(new EF.Member {
                        Name = obj.Name,
                        Email = obj.Email,
                        CommunityId = user.Member.CommunityId
                    }).CountAsync();
                    
                    if (exist > 0)
                    {
                        return new ObjectResult("Existing name or email")
                        {
                            StatusCode = 403,
                            Value = "Existing name or email"
                        };
                    }

                    obj.Name = obj.Name.ToUpper();
                    obj.CommunityId = user.Member.CommunityId;

                    var o = AutoMapper.Mapper.Map<EF.Member>(obj);

                    var id = await bll_member.Add(o);

                    // Add to User
                    if (obj.Email != null)
                    {
                        var bll_user = new BLL.EndUser(context);
                        await bll_user.Add(new EF.EndUser
                        {
                            MemberId = id
                        });
                    }

                    txn.Commit();

                    return Ok(id);
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
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> AddRange([FromBody] List<Models.Member> obj)
        {
            // Email must be unique
            var duplicateEmails = obj.Select(x => x.Email).GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);
            
            if (duplicateEmails.Count() > 0)
            {
                return new ObjectResult("Duplicate email found")
                {
                    StatusCode = 403,
                    Value = "Duplicate email found"
                };
            }

            var context = new EF.AARSContext();

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    var bll_member = new BLL.Member(context);
                    var user = HttpContext.Items["EndUser"] as EF.EndUser;

                    // Validate if existing
                    var map = AutoMapper.Mapper.Map<List<EF.Member>>(obj);

                    var list = from x in map
                               select new EF.Member
                               {
                                   Name = x.Name,
                                   Email = x.Email,
                                   CommunityId = user.Member.CommunityId
                               };
                    var exist = await bll_member.Search(list.ToArray()).CountAsync();
                    if (exist > 0)
                    {
                        return new ObjectResult("Existing name or email")
                        {
                            StatusCode = 403,
                            Value = "Existing name or email"
                        };
                    }

                    foreach (var ob in obj)
                    {
                        ob.Name = ob.Name.ToUpper();
                        ob.CommunityId = user.Member.CommunityId;
                    }

                    var o = AutoMapper.Mapper.Map<List<EF.Member>>(obj);

                    // Gets the newly added members
                    var members = await bll_member.Add(o);

                    // Add new users
                    var bll_user = new BLL.EndUser(context);
                    foreach (var member in members.Where(x => x.Email != null && x.Email != ""))
                    {
                        await bll_user.Add(new EF.EndUser
                        {
                            MemberId = member.Id
                        });
                    }

                    txn.Commit();

                    return Ok();
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

        [HttpGet("[action]")]
        [Route("Get/{id}")]
        public async Task<EF.Member> Get(int id)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            return await bll_member.Get(new EF.Member { Id = id, CommunityId = user.Member.CommunityId });
        }

        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public async Task Edit([FromBody] Models.Member obj)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            await bll_member.Edit(o);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Delete(ids);

            return Json("Success!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_member.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_member.Activate(ids);

            return Json("Success!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
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
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
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
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.CommunityId = user.Member.CommunityId;

            var o = AutoMapper.Mapper.Map<EF.Member>(obj);

            var res = await bll_member.Find(o).ToListAsync();

            var bytes = Encoding.ASCII.GetBytes(res.ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }
    }
}