using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            // Convert DataTable into a generic list
            // https://stackoverflow.com/questions/208532/how-do-you-convert-a-datatable-into-a-generic-list
            var res1 = res.AsEnumerable().ToList();

            Assert.True(res.Rows.Count > 0);
        }
    }
}
