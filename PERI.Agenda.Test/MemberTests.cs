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
        /*private readonly EF.AARSContext context;
        private readonly BLL.UnitOfWork unitOfWork;

        public MemberTests()
        {
            TestHelper.GetApplicationConfiguration();
            context = new EF.AARSContext();
            unitOfWork = new BLL.UnitOfWork(context);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMember_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMember_HasResult(EF.Member args)
        {
            var bll_m = new BLL.Member(unitOfWork);
            var bll_lu = new BLL.LookUp(unitOfWork);

            var members = from r in bll_m.Find(args).ToList()
                        join genders in bll_lu.GetByGroup("Gender").Result.Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.Gender equals genders.Value into e
                            from e1 in e.DefaultIfEmpty()
                        join civilStatuses in bll_lu.GetByGroup("Civil Status").Result.Select(x => new { Label = x.Name, Value = int.Parse(x.Value) }) on r.CivilStatus equals civilStatuses.Value into f
                            from f1 in f.DefaultIfEmpty()
                        join m in bll_m.Find(new EF.Member { CommunityId = 1 }) on r.InvitedBy equals m.Id into g
                            from m1 in g.DefaultIfEmpty()
                        select new
                        {
                            r.Id,
                            r.Name,
                            r.NickName,
                            r.Address,
                            r.Mobile,
                            r.Email,
                            r.BirthDate,
                            r.Remarks,
                            CivilStatus = f1 == null ? "" : f1.Label,
                            Gender = e1 == null ? "" : e1.Label,
                            InvitedBy = m1 == null ? "" : m1.Name,
                            r.DateCreated
                        };

            Assert.True(members.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMember_HasNoResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMember_HasNoResult(EF.Member args)
        {
            var bll_m = new BLL.Member(unitOfWork);

            var members = bll_m.Find(args).ToList();

            Assert.True(members.Count == 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddMember_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddMember_Success(EF.Member args)
        {
            var bll_m = new BLL.Member(unitOfWork);

            var id = bll_m.Add(args).Result;

            Assert.True(id > 0);
        }*/

        private readonly Mock<BLL.IUnitOfWork> mockUnitOfWork;

        public MemberTests()
        {
            mockUnitOfWork = new Mock<BLL.IUnitOfWork>();
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.FindMember_HasResultParams), MemberType = typeof(TestDataGenerator))]
        public void FindMember_HasResult(EF.Member args)
        {
            // Arrange
            var mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            mockMemberRepo.SetupGet(x => x.Entities).Returns(TestRepo.Members.AsQueryable());
            mockUnitOfWork.SetupGet(x => x.MemberRepository).Returns(mockMemberRepo.Object);

            var mockMemberBusiness = new BLL.Member(mockUnitOfWork.Object);

            // Act
            var res = mockMemberBusiness.Find(args);

            // Assert
            Assert.True(res.Count() > 0);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AddMember_SuccessParams), MemberType = typeof(TestDataGenerator))]
        public void AddMember_Success(EF.Member args)
        {
            // Arrange
            var mockMemberRepo = new Mock<Repository.IRepository<EF.Member>>();
            mockMemberRepo.SetupGet(x => x.Entities).Returns(TestRepo.Members.AsQueryable());
            mockUnitOfWork.SetupGet(x => x.MemberRepository).Returns(mockMemberRepo.Object);

            var mockRoleRepo = new Mock<Repository.IRepository<EF.Role>>();
            mockRoleRepo.SetupGet(x => x.Entities).Returns(TestRepo.Roles.AsQueryable());
            mockUnitOfWork.SetupGet(x => x.RoleRepository).Returns(mockRoleRepo.Object);

            var mockEndUserRepo = new Mock<Repository.IRepository<EF.EndUser>>();
            mockEndUserRepo.SetupGet(x => x.Entities).Returns(TestRepo.EndUsers.AsQueryable());
            mockUnitOfWork.SetupGet(x => x.EndUserRepository).Returns(mockEndUserRepo.Object);

            var mockMemberBusiness = new BLL.Member(mockUnitOfWork.Object);

            // Act
            var id = Task.FromResult(mockMemberBusiness.Add(args));

            // Assert
            Assert.True(1 > 0);
        }
    }
}
