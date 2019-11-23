using MockQueryable.Moq;
using Moq;
using System.Linq;
using Xunit;


namespace PERI.Agenda.Test
{
    public class GroupMemberTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IGroupMember groupMemberBusiness;
        private readonly BLL.IGroup groupBusiness;

        private Mock<Repository.IRepository<EF.GroupMember>> mockGroupMemberRepo;
        private Mock<Repository.IRepository<EF.Group>> mockGroupRepo;
        private Mock<Repository.IRepository<EF.Member>> mockMemberRepo;

        public GroupMemberTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            mockGroupMemberRepo = new Mock<Repository.IRepository<EF.GroupMember>>();
            var mockIQueryableGroupMember = new TestRepo().GroupMembers.AsQueryable().BuildMock();
            mockGroupMemberRepo.Setup(x => x.Entities).Returns(mockIQueryableGroupMember.Object);

            mockGroupRepo = new Mock<Repository.IRepository<EF.Group>>();
            var mockIQueryableGroup = new TestRepo().Groups.AsQueryable().BuildMock();
            mockGroupRepo.Setup(x => x.Entities).Returns(mockIQueryableGroup.Object);

            mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            var mockIQueryableMember = new TestRepo().Members.AsQueryable().BuildMock();
            mockMemberRepo.Setup(x => x.Entities).Returns(mockIQueryableMember.Object);

            mockUnitOfWork.Setup(x => x.GroupMemberRepository).Returns(mockGroupMemberRepo.Object);
            mockUnitOfWork.Setup(x => x.GroupRepository).Returns(mockGroupRepo.Object);
            mockUnitOfWork.Setup(x => x.MemberRepository).Returns(mockMemberRepo.Object);

            groupMemberBusiness = new BLL.GroupMember(mockUnitOfWork.Object);
            groupBusiness = new BLL.Group(mockUnitOfWork.Object);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroupMember_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroupMember_HasResult(EF.Member args, int eventId)
        {
            var res = groupMemberBusiness.Members(args, eventId);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindGroupMember_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindGroupMember_HasNoResult(EF.Member args, int eventId)
        {
            var res = groupMemberBusiness.Members(args, eventId);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddGroupMember_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddGroupMember_Success(EF.GroupMember args)
        {
            var list = mockGroupMemberRepo.Object.Entities.ToList();
            var count = list.Count();

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                list.Add(args);
            });

            groupMemberBusiness.Add(args);

            Assert.True(list.Count > count);
        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.DeleteGroupMember_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void DeleteGroupMember_Success(EF.GroupMember args)
        {
            var list = mockGroupMemberRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                var obj = list.First(x => x.GroupId == args.GroupId && x.MemberId == args.MemberId);
                list.Remove(obj);
            });

            groupMemberBusiness.Delete(args);

            Assert.True(list.Count < count);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DeleteGroupMember_FailedParams), MemberType = typeof(TestDataGenerator))]
        public void DeleteGroupMember_Failed(EF.GroupMember args)
        {
            var list = mockGroupMemberRepo.Object.Entities.ToList();
            var count = list.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                var obj = list.First(x => x.GroupId == args.GroupId && x.MemberId == args.MemberId);
                list.Remove(obj);
            });

            groupMemberBusiness.Delete(args);

            Assert.True(list.Count == count);
        }
    }
}
