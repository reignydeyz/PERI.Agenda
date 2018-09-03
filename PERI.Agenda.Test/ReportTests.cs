using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class ReportTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public ReportTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void GetEventCategoryReportByReportId_HasResult(int reportId)
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);
            var bll_r = new BLL.Report(unitOfWork);

            var categories = bll_ec.Find(new EF.EventCategory { CommunityId = 1 });
            var report = bll_r.GetById(reportId).Result;

            var res = from c in categories
                      join ecr in report.EventCategoryReport on c.Id equals ecr.EventCategoryId into ps
                      from ecr in ps.DefaultIfEmpty()
                      select new
                      {
                          c.Id,
                          c.Name,
                          IsSelected = ecr != null
                      };

            Assert.True(res.Where(x => x.IsSelected == true).Count() > 0);
        }
    }
}
