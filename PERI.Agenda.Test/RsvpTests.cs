using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    public class RsvpTests
    {
        private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public RsvpTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [InlineData("ALVIN", 12535, null)]
        public void FindRsvpByEventId_HasResult(string name, int eventId, bool? isGoing)
        {
            var bll_r = new BLL.Rsvp(unitOfWork);

            var res = bll_r.Find(name, eventId, isGoing);

            Assert.True(res.Count() > 0);
        }
    }
}
