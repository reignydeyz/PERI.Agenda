using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class GroupActivityReportTests
    {
        public GroupActivityReportTests()
        {
            TestHelper.GetApplicationConfiguration();
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MonitoringReport_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void MonitoringReport_HasResult(BLL.GroupReport args)
        {
            var res = args.GetTable();

            Assert.True(res.Rows.Count > 0);
        }
    }
}
