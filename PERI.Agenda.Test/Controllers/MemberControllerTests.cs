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

            mockUnitOfWork.Setup(x => x.MemberRepository).Returns(mockMemberRepo.Object);
            mockUnitOfWork.Setup(x => x.EndUserRepository).Returns(mockEndUserRepo.Object);

            memberBusiness = new BLL.Member(mockUnitOfWork.Object);
            endUserBusiness = new BLL.EndUser(mockUnitOfWork.Object);

            var mockOptions = new Mock<IOptions<Core.Emailer>>();
            var mockLookUpBusiness = new Mock<BLL.ILookUp>();
            var mockDBContext = new Mock<EF.AARSContext>();

            controller = new MemberController(mockOptions.Object,
                memberBusiness,
                endUserBusiness,
                mockLookUpBusiness.Object,
                null)
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1, Member = new EF.Member { CommunityId = 1 } });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.MemberController_Find_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasResult(Web.Models.Member obj)
        {
            Mapper.Configuration.CreateMapper();
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
    }
}
