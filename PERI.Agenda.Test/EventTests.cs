using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class EventTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public EventTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData(1484, 1)]
        [InlineData(1031, 1)]
        [InlineData(4148, 1)]
        public void FindCalendarByMemberId_HasResult(int memberId, int communityId)
        {
            var bll_e = new BLL.Event(unitOfWork);

            var events = bll_e.Calendar(memberId, communityId).ToList();

            Assert.True(events.Count > 0);
        }

        [Theory]
        [InlineData(407, 1)]
        [InlineData(795, 1)]
        [InlineData(1343, 1)]
        public void FindCalendarByMemberId_HasResultAndMemberIsRegistrant(int memberId, int communityId)
        {
            var bll_e = new BLL.Event(unitOfWork);

            var events = bll_e.Calendar(memberId, communityId).ToList();

            Assert.True(events.Where(x => x.Registrant.Select(y => y.MemberId).Contains(memberId)).Count() > 0);
        }

        [Theory]
        [InlineData(1484, 1)]
        [InlineData(1031, 1)]
        [InlineData(4148, 1)]
        public void FindCalendarByMemberId_HasResultAndMemberIsNotRegistrant(int memberId, int communityId)
        {
            var bll_e = new BLL.Event(unitOfWork);

            var events = bll_e.Calendar(memberId, communityId).ToList();

            Assert.True(events.Where(x => x.Registrant.Select(y => y.MemberId).Contains(memberId)).Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddExclusiveEvent_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddExclusiveEvent_Success(EF.Event args)
        {
            var bll_e = new BLL.Event(unitOfWork);
            var bll_r = new BLL.Registrant(unitOfWork);
            var bll_g = new BLL.Group(unitOfWork);

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    var eventId = bll_e.Add(args).Result;

                    // Gets members from a group
                    // Example: I selected group Id: 30
                    var gr = bll_g.Get(new EF.Group { Id = 30 }).Result;
                    var registrants = from r in gr.GroupMember
                                      select new EF.Registrant
                                      {
                                          EventId = eventId,
                                          MemberId = r.MemberId.Value,
                                          DateCreated = DateTime.Now,
                                          CreatedBy = "TEST"
                                      };

                    // Add registrants
                    bll_r.Add(registrants).Wait();

                    txn.Commit();
                }
                catch(Exception ex)
                {
                    txn.Rollback();

                    Assert.True(false);
                }
            }
        }
    }
}
