using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace PERI.Agenda.Test
{
    public class GroupTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IGroup groupBusiness;

        private Mock<Repository.IRepository<EF.Group>> mockGroupRepo;

        public GroupTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockGroupRepo = new Mock<Repository.IRepository<EF.Group>>();
            var mockIQueryableGroup = new TestRepo().Groups.AsQueryable().BuildMock();
            mockGroupRepo.Setup(x => x.Entities).Returns(mockIQueryableGroup.Object);

            mockUnitOfWork.Setup(x => x.GroupRepository).Returns(mockGroupRepo.Object);

            groupBusiness = new BLL.Group(mockUnitOfWork.Object);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroup_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroup_HasResult(EF.Group args)
        {
            var res = groupBusiness.Find(args);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroup_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroup_HasNoResult(EF.Group args)
        {
            var res = groupBusiness.Find(args);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void DeleteGroup_Success(params int[] args)
        {
            var list = mockGroupRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            groupBusiness.Delete(args);

            Assert.True(list.Count < count);
        }

        [Theory]
        [InlineData(31, 32, 33)]
        [InlineData(31, 32)]
        [InlineData(32, 33)]
        public void DeleteGroup_Failed(params int[] args)
        {
            var list = mockGroupRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            groupBusiness.Delete(args);

            Assert.True(list.Count == count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddGroup_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddGroup_Success(EF.Group args)
        {
            var list = mockGroupRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                list.Add(args);
            });

            var id = groupBusiness.Add(args).Result;

            Assert.True(list.Count > count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditGroup_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void EditGroup_Success(EF.Group args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            groupBusiness.Edit(args);

            Assert.True(completed);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditGroup_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void EditGroup_Failed(EF.Group args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            groupBusiness.Edit(args);

            Assert.True(!completed);
        }
    }
}
