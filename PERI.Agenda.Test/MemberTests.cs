using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PERI.Agenda.Test
{
    public class MemberTests
    {
        private readonly EF.AARSContext context;
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

            var members = bll_m.Find(args).ToList();

            Assert.True(members.Count > 0);
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
        }
    }
}
