using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public class Statistics
        {
            public int[] Values { get; set; }
            public string[] Labels { get; set; }
        }

        public class GraphData
        {
            public int[] Data { get; set; }
            public string Label { get; set; }
        }

        public class GraphDataSet
        {
            public List<GraphData> DataSet { get; set; }
            public string[] ChartLabels { get; set; }
        }

        [HttpGet("[action]")]
        public async Task<Statistics> Member()
        {
            var context = new EF.AARSContext();
            var bll_member = new BLL.Member(context);

            var members = await bll_member.Find(new EF.Member());

            return new Statistics
            {
                Values = new int[] { members.Where(x => !x.IsActive).Count(), members.Where(x => x.IsActive).Count() },
                Labels = new string[] { "Inactive", "Active" }
            };
        }

        [HttpGet("[action]")]
        public async Task<Statistics> EventCategories()
        {
            var context = new EF.AARSContext();
            var bll_ec = new BLL.EventCategory(context);

            var ecs = from r in (await bll_ec.Find(new EF.EventCategory()))
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

            return new Statistics
            {
                Values = ecs.Select(x => x.AverageAttendees).ToArray(),
                Labels = ecs.Select(x => x.Name).ToArray()
            };
        }

        [HttpGet("[action]")]
        public async Task<Statistics> Locations()
        {
            var context = new EF.AARSContext();
            var bll_loc = new BLL.Location(context);

            var locs = from r in (await bll_loc.Find(new EF.Location()))
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

            return new Statistics
            {
                Values = locs.Select(x => x.AverageAttendees).ToArray(),
                Labels = locs.Select(x => x.Name).ToArray()
            };
        }

        [HttpGet("[action]")]
        public async Task<GraphDataSet> GroupCategories()
        {
            var context = new EF.AARSContext();
            var bll_gc = new BLL.GroupCategory(context);

            var gcs = from r in (await bll_gc.Find(new EF.GroupCategory()))
                      select new
                      {
                          r.Id,
                          r.Name,
                          Groups = r.Group.Count()
                      };

            var list = new List<GraphData>();
            list.Add(new GraphData { Label = "Groups", Data = gcs.Select(x => x.Groups).ToArray() });

            var res = new GraphDataSet
            {
                DataSet = list,
                ChartLabels = gcs.Select(x => x.Name).ToArray()
            };

            return res;
        }
    }
}