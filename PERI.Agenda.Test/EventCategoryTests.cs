using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class EventCategoryTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public EventCategoryTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 1, 2 } })]
        [InlineData(new object[] { new int[] { 3, 4 } })]
        [InlineData(new object[] { new int[] { 5, 6 } })]
        public void GetEventCategoriesByIds_HasResults(int[] eventCategoryIds)
        {
            var bll_ec = new BLL.EventCategory(unitOfWork);

            var res = bll_ec.GetByIds(eventCategoryIds).Result;

            Assert.True(res.Count() > 0);
        }
    }
}
