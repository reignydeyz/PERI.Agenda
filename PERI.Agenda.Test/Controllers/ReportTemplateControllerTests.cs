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
using PERI.Agenda.Web;

namespace PERI.Agenda.Test.Controllers
{
    [Collection("MyCollection")]
    public class ReportTemplateControllerTests
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IReport reportBusiness;
        private readonly BLL.IEventCategory eventCategoryBusiness;

        private readonly Mock<Repository.IRepository<EF.Report>> mockReportRepo;
        private readonly Mock<Repository.IRepository<EF.EventCategory>> mockEventCategoryRepo;

        private readonly ReportTemplateController controller;

        public ReportTemplateControllerTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockReportRepo = new Mock<Repository.IRepository<EF.Report>>();
            var mockIQueryableReport = new TestRepo().Reports.AsQueryable().BuildMock();
            mockReportRepo.Setup(x => x.Entities).Returns(mockIQueryableReport.Object);

            mockEventCategoryRepo = new Mock<Repository.IRepository<EF.EventCategory>>();
            var mockIQueryableEventCategory = new TestRepo().EventCategories.AsQueryable().BuildMock();
            mockEventCategoryRepo.Setup(x => x.Entities).Returns(mockIQueryableEventCategory.Object);

            mockUnitOfWork.Setup(x => x.ReportRepository).Returns(mockReportRepo.Object);
            mockUnitOfWork.Setup(x => x.EventCategoryRepository).Returns(mockEventCategoryRepo.Object);

            eventCategoryBusiness = new BLL.EventCategory(mockUnitOfWork.Object);
            reportBusiness = new BLL.Report(mockUnitOfWork.Object);

            var profile = new AutoMapperProfileConfiguration();
            var mapperConfig = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(mapperConfig);

            controller = new ReportTemplateController(reportBusiness, eventCategoryBusiness, mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1, Member = new EF.Member { CommunityId = 1 } });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ReportTemplateController_New_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Add_Success(Web.Models.ReportTemplate obj)
        {
            var complete = controller.New(obj).IsCompletedSuccessfully;
            
            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ReportTemplateController_Edit_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Success(Web.Models.ReportTemplate obj)
        {
            var complete = controller.Edit(obj).IsCompletedSuccessfully;
            
            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ReportTemplateController_Edit_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Failed(Web.Models.ReportTemplate obj)
        {
            var isFaulted = controller.Edit(obj).IsFaulted;

            Assert.True(isFaulted);
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
        [InlineData(6, 7, 8)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        public async Task Delete_HasResturnedBadRequest(params int[] args)
        {
            var res = await controller.Delete(args) as StatusCodeResult;
            
            res.StatusCode.Should().Be(400);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ReportTemplateController_Find_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasResult(EF.Report obj)
        {
            var res = await controller.Find(obj) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ReportTemplateController_Find_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public async Task Find_HasNoResult(EF.Report obj)
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
        public async Task Checklist_HasResult(int id)
        {
            var res = await controller.Checklist(id) as JsonResult;

            var json = JsonConvert.SerializeObject(res.Value);

            var items = JArray.Parse(json);
            items.Should().HaveCountGreaterThan(0);
        }
    }
}
