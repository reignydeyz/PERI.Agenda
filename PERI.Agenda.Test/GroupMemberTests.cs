using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class GroupMemberTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public GroupMemberTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.Checklist_IsMemberParams), MemberType = typeof(TestDataGenerator))]
        public void Checklist_IsMember(EF.Member obj, int groupId)
        {
            var bll_gm = new BLL.GroupMember(unitOfWork);

            var checklist = bll_gm.Checklist(obj, groupId)
                .Where(x => x.GroupId != 0);

            Assert.True(checklist.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.Checklist_IsNotMemberParams), MemberType = typeof(TestDataGenerator))]
        public void Checklist_IsNotMember(EF.Member obj, int groupId)
        {
            var bll_gm = new BLL.GroupMember(unitOfWork);

            var checklist = bll_gm.Checklist(obj, groupId)
                .Where(x => x.GroupId != 0);

            Assert.True(checklist.Count() == 0);
        }
    }
}
