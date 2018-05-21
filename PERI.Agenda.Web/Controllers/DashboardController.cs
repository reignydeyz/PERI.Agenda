using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet("[action]")]
        public async Task<Models.Graph.Statistics> Member()
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);
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
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var ecs = from r in bll_ec.Find(new EF.EventCategory { CommunityId = user.Member.CommunityId })
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
                Values = await ecs.Select(x => x.AverageAttendees).ToArrayAsync(),
                Labels = await ecs.Select(x => x.Name).ToArrayAsync()
            };
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.Statistics> Locations()
        {
            var context = new EF.AARSContext();
            var bll_loc = new BLL.Location(context);
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var locs = from r in bll_loc.Find(new EF.Location { CommunityId = user.Member.CommunityId })
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
                Values = await locs.Select(x => x.AverageAttendees).ToArrayAsync(),
                Labels = await locs.Select(x => x.Name).ToArrayAsync()
            };
        }

        [HttpGet("[action]")]
        public async Task<Models.Graph.GraphDataSet> GroupCategories()
        {
            var context = new EF.AARSContext();
            var bll_gc = new BLL.GroupCategory(context);
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