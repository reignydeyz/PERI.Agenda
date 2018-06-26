using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class GroupTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public GroupTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(30)]
        [InlineData(79)]
        public void FindEvents_HasResult(int groupId)
        {
            var bll_g = new BLL.Group(unitOfWork);

            var events = from r in bll_g.Activities(groupId)
                         group r by new
                         {
                             r.Event.Id,
                             r.Event.EventCategoryId,
                             Category = r.Event.EventCategory.Name,
                             r.Event.Name,
                             r.Event.IsActive,
                             r.Event.DateTimeStart,
                             r.Event.IsExclusive,
                             r.Event.Location
                         } into g
                         select new
                         {
                             g.Key.Id,
                             g.Key.EventCategoryId,
                             g.Key.Category,
                             g.Key.Name,
                             g.Key.IsActive,
                             g.Key.DateTimeStart,
                             Location = (g.Key.Location == null ? "" : g.Key.Location.Name),
                             Attendance = g.Count(),
                             g.Key.IsExclusive
                         };

            Assert.True(events.Count() > 0);
        }
    }
}
