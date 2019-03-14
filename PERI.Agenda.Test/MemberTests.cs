using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PERI.Agenda.Test
{
    public class MemberTests
    {
        private Mock<BLL.IUnitOfWork> mockUnitOfWork;

        private readonly BLL.IMember memberBusiness;

        private Mock<Repository.IRepository<EF.Member>> mockMemberRepo;

        public MemberTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();

            memberBusiness = new BLL.Member(mockUnitOfWork.Object);

            mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            mockMemberRepo.Setup(x => x.Entities).Returns(TestRepo.Members.AsQueryable());

            mockUnitOfWork.Setup(x => x.MemberRepository).Returns(mockMemberRepo.Object);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMember_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMember_HasResult(EF.Member args)
        {           
            var res = memberBusiness.Find(args);

            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMember_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMember_NoResult(EF.Member args)
        {
            var res = memberBusiness.Find(args);

            Assert.True(res.Count() == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddMember_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddMember_Success(EF.Member args)
        {
            var list = TestRepo.Members;
            var count = TestRepo.Members.Count;

            mockUnitOfWork.Setup(x => x.Commit()).Callback(() =>
            {
                list.Add(args);
            });

            var id = memberBusiness.Add(args).Result;

            Assert.True(list.Count > count);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void DeleteMember_Success(params int[] args)
        {
            var list = TestRepo.Members;
            var count = TestRepo.Members.Count;

            mockUnitOfWork.Setup(x => x.CommitAsync()).Callback(() =>
            {
                var objs = list.Where(x => args.Contains(x.Id)).ToList();
                foreach (var obj in objs)
                    list.Remove(obj);
            });

            memberBusiness.Delete(args);

            Assert.True(list.Count < count);
        }
    }
}
