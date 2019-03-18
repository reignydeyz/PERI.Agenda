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
    public class AttendanceTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IAttendance attendanceBusiness;

        private Mock<Repository.IRepository<EF.Attendance>> mockAttendanceRepo;
        private Mock<Repository.IRepository<EF.Member>> mockMemberRepo;
        private Mock<Repository.IRepository<EF.Event>> mockEventRepo;
        private Mock<Repository.IRepository<EF.Registrant>> mockRegistrantRepo;

        public AttendanceTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockAttendanceRepo = new Mock<Repository.IRepository<EF.Attendance>>();
            var mockIQueryableAttendance = new TestRepo().Attendances.AsQueryable().BuildMock();
            mockAttendanceRepo.Setup(x => x.Entities).Returns(mockIQueryableAttendance.Object);

            mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            var mockIQueryableMember = new TestRepo().Members.AsQueryable().BuildMock();
            mockMemberRepo.Setup(x => x.Entities).Returns(mockIQueryableMember.Object);

            mockEventRepo = new Mock<Repository.IRepository<EF.Event>>();
            var mockIQueryableEvent = new TestRepo().Events.AsQueryable().BuildMock();
            mockEventRepo.Setup(x => x.Entities).Returns(mockIQueryableEvent.Object);

            mockRegistrantRepo = new Mock<Repository.IRepository<EF.Registrant>>();
            var mockIQueryableRegistrant = new TestRepo().Registrants.AsQueryable().BuildMock();
            mockRegistrantRepo.Setup(x => x.Entities).Returns(mockIQueryableRegistrant.Object);

            mockUnitOfWork.Setup(x => x.AttendanceRepository).Returns(mockAttendanceRepo.Object);
            mockUnitOfWork.Setup(x => x.MemberRepository).Returns(mockMemberRepo.Object);
            mockUnitOfWork.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
            mockUnitOfWork.Setup(x => x.RegistrantRepository).Returns(mockRegistrantRepo.Object);

            attendanceBusiness = new BLL.Attendance(mockUnitOfWork.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetRegistrantsByEventId_NonExclusive_Success(int eventId)
        {
            var res = attendanceBusiness.Registrants(eventId).Result.ToList();

            Assert.True(res.Count > 0);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void GetRegistrantsByEventId_Exclusive_Success(int eventId)
        {
            var res = attendanceBusiness.Registrants(eventId).Result.ToList();

            Assert.True(res.Count > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetAttendance_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void GetAttendance_Success(EF.Attendance args)
        {
            var res = attendanceBusiness.Get(args).Result;

            Assert.True(res != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetAttendance_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void GetAttendance_HasNoResult(EF.Attendance args)
        {
            var res = attendanceBusiness.Get(args).Result;

            Assert.True(res == null);
        }

        [Theory]
        [InlineData(1, "ALVIN")]
        [InlineData(1, "CHUA")]
        [InlineData(3, "CHUA")]
        public void SearchRegistrants_Sucess(int eventId, string name)
        {
            var res = attendanceBusiness.Registrants(eventId, name).Result.ToList();

            Assert.True(res.Count > 0);
        }

        [Theory]
        [InlineData(1, "ALVIN21")]
        [InlineData(2, "CHUA21")]
        [InlineData(3, "CHUA21")]
        public void SearchRegistrants_HasNoResult(int eventId, string name)
        {
            var res = attendanceBusiness.Registrants(eventId, name).Result.ToList();

            Assert.True(res.Count == 0);
        }
    }
}
