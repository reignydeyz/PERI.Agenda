using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PERI.Agenda.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static PERI.Agenda.Web.Models.Graph;

namespace PERI.Agenda.Test.Controllers
{
    [Collection("MyCollection")]
    public class LocationControllerTest
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.ILocation locationBusiness;
        private readonly BLL.IEvent eventBusiness;

        private readonly Mock<Repository.IRepository<EF.Location>> mockLocationRepo;

        private readonly LocationController controller;

        public LocationControllerTest()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockLocationRepo = new Mock<Repository.IRepository<EF.Location>>();
            var mockIQueryableLocation = new TestRepo().Locations.AsQueryable().BuildMock();
            mockLocationRepo.Setup(x => x.Entities).Returns(mockIQueryableLocation.Object);

            mockUnitOfWork.Setup(x => x.LocationRepository).Returns(mockLocationRepo.Object);

            locationBusiness = new BLL.Location(mockUnitOfWork.Object);
            eventBusiness = new BLL.Event(mockUnitOfWork.Object);

            controller = new LocationController(locationBusiness, eventBusiness);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1, Member = new EF.Member { CommunityId = 1 } });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LocationController_Add_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Add_Success(Web.Models.Location obj)
        {
            var complete = controller.New(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LocationController_Edit_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Success(Web.Models.Location obj)
        {
            var complete = controller.Edit(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LocationController_Edit_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Failed(Web.Models.Location obj)
        {
            var complete = controller.Edit(obj).IsFaulted;

            Assert.True(complete);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task Delete_Success(params int[] args)
        {
            var res = await controller.Delete(args) as JsonResult;

            res.Value.Should().Be("Success!");
        }

        [Theory]
        [InlineData(96, 97, 98)]
        [InlineData(96, 97)]
        [InlineData(97, 98)]
        public async Task Delete_HasResturnedBadRequest(params int[] args)
        {
            var res = await controller.Delete(args) as StatusCodeResult;

            res.StatusCode.Should().Be(400);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LocationController_Find_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasResult(EF.Location obj)
        {
            var res = await controller.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LocationController_Find_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasNoResult(EF.Location obj)
        {
            var res = await controller.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public async Task Get_Success(int args)
        {
            var res = await controller.Get(args) as JsonResult;

            res.Value.Should().BeOfType<ExpandoObject>();
        }

        [Theory]
        [InlineData(91)]
        [InlineData(92)]
        [InlineData(93)]
        [InlineData(94)]
        [InlineData(95)]
        [InlineData(96)]
        [InlineData(97)]
        [InlineData(98)]
        [InlineData(99)]
        public void Get_Failed(int args)
        {
            var complete = controller.Get(args).IsFaulted;

            Assert.True(complete);
        }
    }
}
