using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace PERI.Agenda.Test
{
    public class EventCategoryTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IEventCategory eventCategoryBusiness;

        private Mock<Repository.IRepository<EF.EventCategory>> mockEventCategoryRepo;

        public EventCategoryTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockEventCategoryRepo = new Mock<Repository.IRepository<EF.EventCategory>>();
            var mockIQueryableEventCategory = new TestRepo().EventCategories.AsQueryable().BuildMock();
            mockEventCategoryRepo.Setup(x => x.Entities).Returns(mockIQueryableEventCategory.Object);

            mockUnitOfWork.Setup(x => x.EventCategoryRepository).Returns(mockEventCategoryRepo.Object);

            eventCategoryBusiness = new BLL.EventCategory(mockUnitOfWork.Object);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 1, 2 } })]
        [InlineData(new object[] { new int[] { 3, 4 } })]
        [InlineData(new object[] { new int[] { 5, 6 } })]
        public void GetEventCategoriesByIds_HasResults(int[] eventCategoryIds)
        {
            var res = eventCategoryBusiness.GetByIds(eventCategoryIds).Result;

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 91, 92 } })]
        [InlineData(new object[] { new int[] { 93, 94 } })]
        [InlineData(new object[] { new int[] { 95, 96 } })]
        public void GetEventCategoriesByIds_NoResult(int[] eventCategoryIds)
        {
            var res = eventCategoryBusiness.GetByIds(eventCategoryIds).Result;

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindEventCategory_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindEventCategory_HasResult(EF.EventCategory args)
        {
            var res = eventCategoryBusiness.Find(args);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindEventCategory_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindEventCategory_HasNoResult(EF.EventCategory args)
        {
            var res = eventCategoryBusiness.Find(args);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddEventCategory_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddEventCategory_Success(EF.EventCategory args)
        {
            var list = mockEventCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                list.Add(args);
            });

            var id = eventCategoryBusiness.Add(args).Result;

            Assert.True(list.Count > count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditEventCategory_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void EditEventCategory_Success(EF.EventCategory args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            eventCategoryBusiness.Edit(args);

            Assert.True(completed);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditEventCategory_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void EditEventCategory_Failed(EF.EventCategory args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            eventCategoryBusiness.Edit(args);

            Assert.True(!completed);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void DeleteEventCategory_Success(params int[] args)
        {
            var list = mockEventCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            eventCategoryBusiness.Delete(args);

            Assert.True(list.Count < count);
        }

        [Theory]
        [InlineData(31, 32, 33)]
        [InlineData(31, 32)]
        [InlineData(32, 33)]
        public void DeleteEventCategory_Failed(params int[] args)
        {
            var list = mockEventCategoryRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            eventCategoryBusiness.Delete(args);

            Assert.True(list.Count == count);
        }
    }
}
