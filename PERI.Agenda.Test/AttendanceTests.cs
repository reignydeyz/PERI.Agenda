using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class AttendanceTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public AttendanceTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData(12545)]
        public void FindRegistrantsByEventId_Exclusive_Success(int eventId)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            var res = bll_a.Registrants(eventId).Result.ToList();

            Assert.True(res.Count > 0);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 1, 2 } })]
        public void FindAttendanceByEventCategoryIds_HasResults(int[] eventCategoryIds)
        {
            var bll_a = new BLL.Attendance(unitOfWork);

            var res = bll_a.FindByEventCategoryIds(eventCategoryIds);

            Assert.True(res.Count() > 0);
        }
    }
}
