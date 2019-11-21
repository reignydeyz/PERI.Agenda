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
    public class EventTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IEvent eventBusiness;

        private Mock<Repository.IRepository<EF.Event>> mockEventRepo;

        public EventTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockEventRepo = new Mock<Repository.IRepository<EF.Event>>();
            var mockIQueryableEvent = new TestRepo().Events.AsQueryable().BuildMock();
            mockEventRepo.Setup(x => x.Entities).Returns(mockIQueryableEvent.Object);

            mockUnitOfWork.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);

            eventBusiness = new BLL.Event(mockUnitOfWork.Object);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindEvent_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindEvent_HasResult(EF.Event args)
        {
            var res = eventBusiness.Find(args);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindEvent_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindEvent_NoResult(EF.Event args)
        {
            var res = eventBusiness.Find(args);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddNonExclusiveEvent_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddEvent_Success(EF.Event args)
        {
            var list = mockEventRepo.Object.Entities.ToList();
            var count = list.Count();

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                list.Add(args);
            });

            var id = eventBusiness.Add(args).Result;

            Assert.True(list.Count > count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditEvent_Success), MemberType = typeof(TestDataGenerator))]
        public void EditEvent_Success(EF.Event args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            eventBusiness.Edit(args);

            Assert.True(completed);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.EditEvent_Failed), MemberType = typeof(TestDataGenerator))]
        public void EditEvent_Failed(EF.Event args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            eventBusiness.Edit(args);

            Assert.True(!completed);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void DeleteEvent_Success(params int[] args)
        {
            var list = mockEventRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            eventBusiness.Delete(args);

            Assert.True(list.Count < count);
        }

        [Theory]
        [InlineData(1001, 1002, 1003)]
        [InlineData(1001, 1002)]
        [InlineData(1002, 1003)]
        public void DeleteEvent_Failed(params int[] args)
        {
            var completed = false;
            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                completed = true;
            });

            eventBusiness.Delete(args);

            Assert.True(!completed);
        }
    }
}
