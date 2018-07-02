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

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroup_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroup_HasResult(EF.Group args)
        {
            var bll_g = new BLL.Group(unitOfWork);

            var groups = bll_g.Find(args).ToList();

            Assert.True(groups.Count > 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetGroupsByGroupCategoryId_HasResult(int groupCategoryId)
        {
            var bll_g = new BLL.Group(unitOfWork);

            // CommunityId
            var gc = new EF.GroupCategory { CommunityId = 1 };

            var groups = bll_g.Find(new EF.Group { GroupCategoryId = groupCategoryId, GroupCategory = gc }).ToList();

            Assert.True(groups.Count > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMembers_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMembers_HasResult(EF.Member obj, int groupId)
        {
            var bll_gm = new BLL.GroupMember(unitOfWork);

            var members = bll_gm.Members(obj, groupId);

            Assert.True(members.Count() > 0);
        }
    }
}
