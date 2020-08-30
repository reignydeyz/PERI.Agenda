using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using PERI.Agenda.BLL;
using PERI.Agenda.Core;
using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [ApiVersion("1.0")]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IEventCategory eventCategoryBusiness;
        private readonly IEvent eventBusiness;
        private readonly IRegistrant registrantBusiness;
        private readonly IGroup groupBusiness;
        private readonly EF.AARSContext context;
        private readonly IMapper mapper;

        public EventController(IEventCategory eventCategory,
            IEvent eventBusiness,
            IRegistrant registrant,
            IGroup group,
            EF.AARSContext context,
            IMapper mapper)
        {
            this.context = context;
            this.eventCategoryBusiness = eventCategory;
            this.eventBusiness = eventBusiness;
            this.registrantBusiness = registrant;
            this.groupBusiness = group;
            this.mapper = mapper;
        }

        /// <summary>
        /// Searches events
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public async Task<IActionResult> Find([FromBody] EF.Event obj)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in (await bll_event.Find(obj).ToListAsync())
                      select new
                        {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                        };

            return Json(res);
        }

        /// <summary>
        /// Searches events (with pagination)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Find/Page/{id}")]
        public async Task<IActionResult> Page([FromBody] EF.Event obj, int id)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_event.Find(obj)
                      select new
                      {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                      };
            var page = id;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.events = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        /// <summary>
        /// Searches events that are created by the logged in user/member
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Find/MyPage/{id}")]
        public async Task<IActionResult> MyPage([FromBody] EF.Event obj, int id)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_event.Find(obj)
                      where r.CreatedBy == user.Member.Name
                      select new
                      {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                      };
            var page = id;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.events = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        /// <summary>
        /// Searches events that are created by the logged in user/member
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="groupId"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Find/Group/{groupId}/{pageNum}")]
        public async Task<IActionResult> MyPage([FromBody] EF.Event obj, int groupId, int pageNum)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in bll_event.Find(obj)
                      where r.EventGroup.Any(x => x.GroupId == groupId)
                      select new
                      {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                      };
            var page = pageNum;
            var pager = new Core.Pager(await res.CountAsync(), page == 0 ? 1 : page, 100);

            dynamic obj1 = new ExpandoObject();
            obj1.events = await res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
            obj1.pager = pager;

            return Json(obj1);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("download")]
        public async Task<IActionResult> Download([FromBody] EF.Event obj)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            var res = from r in (await bll_event.Find(obj).ToListAsync())
                      select new
                      {
                          r.Id,
                          r.EventCategoryId,
                          Category = r.EventCategory.Name,
                          r.Name,
                          r.IsActive,
                          r.DateTimeStart,
                          Location = (r.Location == null ? "" : r.Location.Name),
                          Attendance = r.Attendance.Count,
                          r.IsExclusive
                      };

            var bytes = Encoding.ASCII.GetBytes(res.ToList().ExportToCsv().ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost]
        [Route("new")]
        [BLL.ValidateModelState]
        public async Task<int> New([FromBody] Models.Event obj)
        {
            var bll_event = eventBusiness;

            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var o = this.mapper.Map<EF.Event>(obj);
            o.CreatedBy = user.Member.Name;
            o.DateTimeCreated = DateTime.Now;

            return await bll_event.Add(o);
        }

        /// <summary>
        /// Adds an exclusive event for a group
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("New/Exclusive/{groupId}")]
        [BLL.ValidateModelState]
        public async Task<IActionResult> NewExclusive([FromBody] Models.Event obj, int groupId)
        {
            var bll_e = eventBusiness;
            var bll_r = registrantBusiness;
            var bll_g = groupBusiness;

            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    var o = this.mapper.Map<EF.Event>(obj);
                    o.IsExclusive = true;
                    o.CreatedBy = user.Member.Name;
                    o.DateTimeCreated = DateTime.Now;
                    var eventId = bll_e.Add(o, groupId).Result;

                    // Gets members from a group
                    var gr = await bll_g.Get(new EF.Group { Id = groupId });
                    var registrants = (from r in gr.GroupMember
                                       select new EF.Registrant
                                       {
                                           EventId = eventId,
                                           MemberId = r.MemberId.Value,
                                           DateCreated = DateTime.Now,
                                           CreatedBy = user.Member.Name
                                       }).ToList();

                    // Also add the group leader
                    registrants.Add(new EF.Registrant
                    {
                        EventId = eventId,
                        MemberId = gr.GroupLeader.Value,
                        DateCreated = DateTime.Now,
                        CreatedBy = user.Member.Name
                    });

                    // Add registrants
                    await bll_r.Add(registrants);

                    txn.Commit();

                    return Ok(eventId);
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

        /// <summary>
        /// Gets the event detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var r = await bll_event.Get(new EF.Event { Id = id, EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId } });

            dynamic obj = new ExpandoObject();
            obj.id = r.Id;
            obj.eventCategoryId = r.EventCategoryId;
            obj.category = r.EventCategory.Name;
            obj.name = r.Name;
            obj.isActive = r.IsActive;
            obj.dateTimeStart = r.DateTimeStart;
            obj.locationId = r.LocationId;
            obj.location = (r.Location == null ? "" : r.Location.Name);
            obj.attendance = r.Attendance.Count;
            obj.isExclusive = r.IsExclusive == true;

            return Json(obj);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task Edit([FromBody] EF.Event obj)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            obj.EventCategory = new EF.EventCategory { CommunityId = user.Member.CommunityId };

            await bll_event.Edit(obj);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [BLL.VerifyUser(AllowedRoles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bll_event = eventBusiness;
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            if (!await bll_event.IsSelectedIdsOk(ids, user))
                return BadRequest();

            await bll_event.Delete(ids);

            return Json("Success!");
        }
    }
}