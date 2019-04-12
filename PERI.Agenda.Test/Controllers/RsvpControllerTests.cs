using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using PERI.Agenda.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PERI.Agenda.Test.Controllers
{
    public class RsvpControllerTests
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IRsvp rsvpBusiness;

        private readonly Mock<Repository.IRepository<EF.Rsvp>> mockRsvpRepo;

        private readonly RsvpController rsvpController;

        public RsvpControllerTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockRsvpRepo = new Mock<Repository.IRepository<EF.Rsvp>>();
            var mockIQueryableRsvp = new TestRepo().Rsvps.AsQueryable().BuildMock();
            mockRsvpRepo.Setup(x => x.Entities).Returns(mockIQueryableRsvp.Object);
            
            mockUnitOfWork.Setup(x => x.RsvpRepository).Returns(mockRsvpRepo.Object);
                       
            rsvpBusiness = new BLL.Rsvp(mockUnitOfWork.Object);

            rsvpController = new RsvpController(rsvpBusiness);
            rsvpController.ControllerContext = new ControllerContext();
            rsvpController.ControllerContext.HttpContext = new DefaultHttpContext();
            rsvpController.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1 });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RsvpController_Find_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasResult(Web.Models.Rsvp obj)
        {
            var res = await rsvpController.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RsvpController_Find_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasNoResult(Web.Models.Rsvp obj)
        {
            var res = await rsvpController.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountLessOrEqualTo(0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RsvpController_Add_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Add_Success(Web.Models.Rsvp obj)
        {
            var complete = rsvpController.Add(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RsvpController_Update_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Success(Web.Models.Rsvp obj)
        {
            var complete = rsvpController.Add(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RsvpController_Delete_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Delete_Success(Web.Models.Rsvp obj)
        {
            var complete = rsvpController.Delete(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }
    }
}
