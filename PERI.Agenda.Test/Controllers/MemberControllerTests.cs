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
using AutoMapper;
using Microsoft.Extensions.Options;
using PERI.Agenda.Web;

namespace PERI.Agenda.Test.Controllers
{
    [Collection("MyCollection")]
    public class MemberControllerTests
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IMember memberBusiness;
        private readonly BLL.EndUser endUserBusiness;

        private readonly Mock<Repository.IRepository<EF.Member>> mockMemberRepo;
        private readonly Mock<Repository.IRepository<EF.EndUser>> mockEndUserRepo;
        private readonly Mock<Repository.IRepository<EF.Group>> mockGroupRepo;
        private readonly Mock<Repository.IRepository<EF.GroupMember>> mockGroupMemberRepo;
        private readonly Mock<Repository.IRepository<EF.Attendance>> mockAttendanceRepo;
        private readonly Mock<Repository.IRepository<EF.Event>> mockEventRepo;
        private readonly Mock<Repository.IRepository<EF.EventCategory>> mockEventCategoryRepo;

        private readonly MemberController controller;

        public MemberControllerTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            var mockIQueryableMember = new TestRepo().Members.AsQueryable().BuildMock();
            mockMemberRepo.Setup(x => x.Entities).Returns(mockIQueryableMember.Object);

            mockEndUserRepo = new Mock<Repository.IRepository<EF.EndUser>>();
            var mockIQueryableEndUser = new TestRepo().EndUsers.AsQueryable().BuildMock();
            mockEndUserRepo.Setup(x => x.Entities).Returns(mockIQueryableEndUser.Object);

            mockGroupRepo = new Mock<Repository.IRepository<EF.Group>>();
            var mockIQueryableGroup = new TestRepo().Groups.AsQueryable().BuildMock();
            mockGroupRepo.Setup(x => x.Entities).Returns(mockIQueryableGroup.Object);

            mockGroupMemberRepo = new Mock<Repository.IRepository<EF.GroupMember>>();
            var mockIQueryableGroupMember = new TestRepo().GroupMembers.AsQueryable().BuildMock();
            mockGroupMemberRepo.Setup(x => x.Entities).Returns(mockIQueryableGroupMember.Object);

            mockAttendanceRepo = new Mock<Repository.IRepository<EF.Attendance>>();
            var mockIQueryableAttendance = new TestRepo().Attendances.AsQueryable().BuildMock();
            mockAttendanceRepo.Setup(x => x.Entities).Returns(mockIQueryableAttendance.Object);

            mockEventRepo = new Mock<Repository.IRepository<EF.Event>>();
            var mockIQueryableEvent = new TestRepo().Events.AsQueryable().BuildMock();
            mockEventRepo.Setup(x => x.Entities).Returns(mockIQueryableEvent.Object);

            mockEventCategoryRepo = new Mock<Repository.IRepository<EF.EventCategory>>();
            var mockIQueryableEventCategory = new TestRepo().EventCategories.AsQueryable().BuildMock();
            mockEventCategoryRepo.Setup(x => x.Entities).Returns(mockIQueryableEventCategory.Object);

            mockUnitOfWork.Setup(x => x.MemberRepository).Returns(mockMemberRepo.Object);
            mockUnitOfWork.Setup(x => x.EndUserRepository).Returns(mockEndUserRepo.Object);
            mockUnitOfWork.Setup(x => x.GroupRepository).Returns(mockGroupRepo.Object);
            mockUnitOfWork.Setup(x => x.GroupMemberRepository).Returns(mockGroupMemberRepo.Object);
            mockUnitOfWork.Setup(x => x.AttendanceRepository).Returns(mockAttendanceRepo.Object);
            mockUnitOfWork.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
            mockUnitOfWork.Setup(x => x.EventCategoryRepository).Returns(mockEventCategoryRepo.Object);

            memberBusiness = new BLL.Member(mockUnitOfWork.Object);
            endUserBusiness = new BLL.EndUser(mockUnitOfWork.Object);

            var mockOptions = new Mock<IOptions<Core.Emailer>>();
            var mockLookUpBusiness = new Mock<BLL.ILookUp>();
            var mockDBContext = new Mock<EF.AARSContext>();

            var profile = new AutoMapperProfileConfiguration();
            var mapperConfig = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(mapperConfig);

            controller = new MemberController(mockOptions.Object,
                memberBusiness,
                endUserBusiness,
                mockLookUpBusiness.Object,
                null,
                mapper)
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1, Member = new EF.Member { CommunityId = 1 }, RoleId = 1 });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Find_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasResult(Web.Models.Member obj)
        {
            var res = await controller.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Find_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasNoResult(Web.Models.Member obj)
        {
            var res = await controller.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task Get_HasResult(int id)
        {
            var res = await controller.Get(id) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var item = JObject.Parse(json);
            item.Should().NotBeNull();
        }

        [Theory]
        [InlineData(91)]
        [InlineData(92)]
        [InlineData(93)]
        [InlineData(94)]
        [InlineData(95)]
        public async Task Get_HasReturnedBadRequest(int id)
        {
            var res = await controller.Get(id) as StatusCodeResult;

            res.StatusCode.Should().Be(400);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Edit_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Success(Web.Models.Member obj)
        {
            var complete = controller.Edit(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Edit_FailedParams), MemberType = typeof(TestDataGenerator))]
        public async Task Update_HasReturnedBadRequest(Web.Models.Member obj)
        {
            var res = await controller.Edit(obj) as StatusCodeResult;

            res.StatusCode.Should().Be(400);
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
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task Activate_Success(params int[] args)
        {
            var res = await controller.Activate(args) as JsonResult;

            res.Value.Should().Be("Success!");
        }

        [Theory]
        [InlineData(96, 97, 98)]
        [InlineData(96, 97)]
        [InlineData(97, 98)]
        public async Task Activate_HasResturnedBadRequest(params int[] args)
        {
            var res = await controller.Activate(args) as StatusCodeResult;

            res.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task Deactivate_Success(params int[] args)
        {
            var res = await controller.Deactivate(args) as JsonResult;

            res.Value.Should().Be("Success!");
        }

        [Theory]
        [InlineData(96, 97, 98)]
        [InlineData(96, 97)]
        [InlineData(97, 98)]
        public async Task Deactivate_HasResturnedBadRequest(params int[] args)
        {
            var res = await controller.Deactivate(args) as StatusCodeResult;

            res.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Leading_Success(int obj)
        {
            var complete = controller.Leading(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Following_Success(int obj)
        {
            var complete = controller.Following(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Invites_Success(int obj)
        {
            var complete = controller.Invites(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [InlineData("active")]
        [InlineData("inactive")]
        [InlineData("all")]
        public void Total_Success(string obj)
        {
            var complete = controller.Total(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Activities_Success(int obj)
        {
            var complete = controller.Activities(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Download_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Download_Success(Web.Models.Member obj)
        {
            var complete = controller.Download(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Fact]
        public void AllNames_Success()
        {
            var complete = controller.AllNames().IsCompletedSuccessfully;

            Assert.True(complete);
        }
    }
}
