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
            var bll_r = new BLL.Report(unitOfWork);

            var categories = bll_r.Checklist(reportId).ToList();

            Assert.True(categories.Where(x => x.EventCategoryReport != null).Count() > 0);
        }
    }
}
