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
    public class RoleControllerTests
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IRole roleBusiness;

        private readonly Mock<Repository.IRepository<EF.Role>> mockRoleRepo;

        private readonly RoleController controller;

        public RoleControllerTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockRoleRepo = new Mock<Repository.IRepository<EF.Role>>();
            var mockIQueryableRole = new TestRepo().Roles.AsQueryable().BuildMock();
            mockRoleRepo.Setup(x => x.Entities).Returns(mockIQueryableRole.Object);

            mockUnitOfWork.Setup(x => x.RoleRepository).Returns(mockRoleRepo.Object);

            roleBusiness = new BLL.Role(mockUnitOfWork.Object);

            controller = new RoleController(roleBusiness);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1 });
        }

        [Fact]
        public async Task GetAll_Success()
        {
            var res = await controller.GetAll();

            res.Count().Should().Be(mockRoleRepo.Object.Entities.Count());
        }
    }
}
