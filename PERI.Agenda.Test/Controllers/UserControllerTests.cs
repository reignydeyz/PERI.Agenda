using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using PERI.Agenda.Web.Controllers;
using System.Linq;
using Xunit;

namespace PERI.Agenda.Test.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IEndUser endUserBusiness;

        private readonly Mock<Repository.IRepository<EF.EndUser>> mockEndUserRepo;

        private readonly UserController controller;

        public UserControllerTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockEndUserRepo = new Mock<Repository.IRepository<EF.EndUser>>();
            var mockIQueryableEndUser = new TestRepo().EndUsers.AsQueryable().BuildMock();
            mockEndUserRepo.Setup(x => x.Entities).Returns(mockIQueryableEndUser.Object);

            mockUnitOfWork.Setup(x => x.EndUserRepository).Returns(mockEndUserRepo.Object);

            endUserBusiness = new BLL.EndUser(mockUnitOfWork.Object);

            controller = new UserController(endUserBusiness);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items.Add("EndUser", new EF.EndUser { MemberId = 1 });
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.UserController_Update_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Success(Web.Models.Member obj)
        {
            var complete = controller.UpdateRole(obj).IsCompletedSuccessfully;

            Assert.True(complete);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.UserController_Update_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void Update_Failed(Web.Models.Member obj)
        {
            var isFaulted = controller.UpdateRole(obj).IsFaulted;

            Assert.True(isFaulted);
        }
    }
}
