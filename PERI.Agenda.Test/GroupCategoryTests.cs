using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace PERI.Agenda.Test
{
    public class GroupCategoryTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IGroupCategory groupCategoryBusiness;

        private Mock<Repository.IRepository<EF.GroupCategory>> mockGroupCategoryRepo;

        public GroupCategoryTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockGroupCategoryRepo = new Mock<Repository.IRepository<EF.GroupCategory>>();
            var mockIQueryableGroupCategory = new TestRepo().GroupCategories.AsQueryable().BuildMock();
            mockGroupCategoryRepo.Setup(x => x.Entities).Returns(mockIQueryableGroupCategory.Object);

            mockUnitOfWork.Setup(x => x.GroupCategoryRepository).Returns(mockGroupCategoryRepo.Object);

            groupCategoryBusiness = new BLL.GroupCategory(mockUnitOfWork.Object);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroupCategory_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroupCategory_HasResult(EF.GroupCategory args)
        {
            var res = groupCategoryBusiness.Find(args);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroupCategory_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroupCategory_HasNoResult(EF.GroupCategory args)
        {
            var res = groupCategoryBusiness.Find(args);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddGroupCategory_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddGroupCategory_Success(EF.GroupCategory args)
        {
            var list = mockGroupCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                list.Add(args);
            });

            var id = groupCategoryBusiness.Add(args).Result;

            Assert.True(list.Count > count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditGroupCategory_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void EditGroupCategory_Success(EF.GroupCategory args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            groupCategoryBusiness.Edit(args);

            Assert.True(completed);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditGroupCategory_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void EditGroupCategory_Failed(EF.GroupCategory args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            groupCategoryBusiness.Edit(args);

            Assert.True(!completed);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void DeleteGroupCategory_Success(params int[] args)
        {
            var list = mockGroupCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            groupCategoryBusiness.Delete(args);

            Assert.True(list.Count < count);
        }

        [Theory]
        [InlineData(31, 32, 33)]
        [InlineData(31, 32)]
        [InlineData(32, 33)]
        public void DeleteGroupCategory_Failed(params int[] args)
        {
            var list = mockGroupCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            groupCategoryBusiness.Delete(args);

            Assert.True(list.Count == count);
        }
    }
}
