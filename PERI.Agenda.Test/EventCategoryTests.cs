using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
