using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser(AllowedRoles = "Admin")]
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.Statistics> Member()
        {
            var bll_member = new BLL.Member(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var members = bll_member.Find(new EF.Member { CommunityId = user.Member.CommunityId });

            return new Models.Graph.Statistics
            {
                Values = new int[] { await members.Where(x => !x.IsActive).CountAsync(), await members.Where(x => x.IsActive).CountAsync() },
                Labels = new string[] { "Inactive", "Active" }
            };
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.Statistics> EventCategories()
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var ecs = from r in await bll_ec.Find(new EF.EventCategory { CommunityId = user.Member.CommunityId }).ToListAsync()
                      select new
                      {
                          r.Id,
                          r.Name,
                          Events = r.Event.Count,
                          MinAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count),
                          AverageAttendees = Convert.ToInt32(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count)),
                          MaxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count)
                      };

            ecs = ecs.OrderByDescending(x => x.AverageAttendees).Take(10);

            return new Models.Graph.Statistics
            {
                Values = ecs.Select(x => x.AverageAttendees).ToArray(),
                Labels = ecs.Select(x => x.Name).ToArray()
            };
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.Statistics> Locations()
        {
            var bll_loc = new BLL.Location(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var locs = from r in await bll_loc.Find(new EF.Location { CommunityId = user.Member.CommunityId }).ToListAsync()
                       select new
                       {
                           r.Id,
                           r.Name,
                           Events = r.Event.Count,
                           MinAttendees = r.Event.Count <= 0 ? 0 : r.Event.Min(x => x.Attendance.Count),
                           AverageAttendees = Convert.ToInt32(r.Event.Count <= 0 ? 0 : r.Event.Average(x => x.Attendance.Count)),
                           MaxAttendees = r.Event.Count <= 0 ? 0 : r.Event.Max(x => x.Attendance.Count)
                       };

            locs = locs.OrderByDescending(x => x.AverageAttendees).Take(10);

            return new Models.Graph.Statistics
            {
                Values = locs.Select(x => x.AverageAttendees).ToArray(),
                Labels = locs.Select(x => x.Name).ToArray()
            };
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.GraphDataSet> GroupCategories()
        {
            var bll_gc = new BLL.GroupCategory(unitOfWork);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var gcs = from r in bll_gc.Find(new EF.GroupCategory { CommunityId = user.Member.CommunityId })
                      select new
                      {
                          r.Id,
                          r.Name,
                          Groups = r.Group.Count()
                      };

            var list = new List<Models.Graph.GraphData>();
            list.Add(new Models.Graph.GraphData { Label = "Groups", Data = await gcs.Select(x => x.Groups).ToArrayAsync() });

            var res = new Models.Graph.GraphDataSet
            {
                DataSet = list,
                ChartLabels = await gcs.Select(x => x.Name).ToArrayAsync()
            };

            return res;
        }
    }
}