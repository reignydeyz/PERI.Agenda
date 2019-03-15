using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Test
{
    public class TestDataGenerator
    {
        #region MemberTests
        public static IEnumerable<object[]> FindMember_HasResultParams()
        {
            yield return new object[] { new EF.Member { Name = "ALVIN", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "CHUA", CommunityId = 1 } };
        }

        public static IEnumerable<object[]> FindMember_HasNoResultParams()
        {
            yield return new object[] { new EF.Member { Name = "ALVINXXYZ123456", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "CHUAXXYZ123456", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "TESTINGXXYZ123456", CommunityId = 1 } };
        }

        public static IEnumerable<object[]> AddMember_SuccessParams()
        {
            yield return new object[] { new EF.Member { Name = "JANE O. DOE", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "BRAD PIT", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "Erika Arellano", CommunityId = 1 } };
        }

        public static IEnumerable<object[]> EditMember_ExistingEmailParams()
        {
            yield return new object[] { new EF.Member { Name = "JANE O. DOE", Email = "abc@y.com", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "BRAD PIT", Email = "bcd@y.com", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Name = "Erika Arellano", Email = "cde@y.com", CommunityId = 1 } };
        }

        public static IEnumerable<object[]> EditMember_Success()
        {
            yield return new object[] { new EF.Member { Id=1, Name = "JANE O. DOE", Email = "abc@y.com", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Id=2, Name = "BRAD PIT", Email = "bcd@y.com", CommunityId = 1 } };
            yield return new object[] { new EF.Member { Id=3, Name = "Erika Arellano", Email = "cde@y.com", CommunityId = 1 } };
        }
        #endregion

        #region EventTests
        public static IEnumerable<object[]> AddExclusiveEvent_SuccessParams()
        {
            yield return new object[] { new EF.Event { Name = "EVENT0001", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(1), IsExclusive = true, IsActive = true } };
            yield return new object[] { new EF.Event { Name = "EVENT0002", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(2), IsExclusive = true, IsActive = true } };
            yield return new object[] { new EF.Event { Name = "EVENT0003", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(3), IsExclusive = true, IsActive = true } };
        }
        #endregion

        #region GroupTests
        public static IEnumerable<object[]> FindGroup_HasResultParams()
        {
            yield return new object[] { new EF.Group { Name = "CELL", GroupCategory = new EF.GroupCategory { CommunityId = 1 } } };
            yield return new object[] { new EF.Group { Name = "NETWORK", GroupCategory = new EF.GroupCategory { CommunityId = 1 } } };
        }

        public static IEnumerable<object[]> FindMembers_HasResultParams()
        {
            yield return new object[] { new EF.Member { Name = "JHE" }, 30 };
            yield return new object[] { new EF.Member { Name = "JOEL" }, 40 };
        }
        #endregion

        #region GroupMemberTests
        public static IEnumerable<object[]> Checklist_IsMemberParams()
        {
            yield return new object[] { new EF.Member { Name = "JHE", CommunityId = 1 }, 30, };
            yield return new object[] { new EF.Member { Name = "JOEL", CommunityId = 1 }, 40 };
        }

        public static IEnumerable<object[]> Checklist_IsNotMemberParams()
        {
            yield return new object[] { new EF.Member { Name = "JONATHAN", CommunityId = 1 }, 30 };
            yield return new object[] { new EF.Member { Name = "LEBRON", CommunityId = 1 }, 40 };
        }
        #endregion

        #region GroupActivityReportTests
        public static IEnumerable<object[]> MonitoringReport_HasResultParams()
        {
            yield return new object[] { new BLL.GroupReport {
                DateFrom = new DateTime(2018, 1, 1),
                DateTo = new DateTime(2018, 12, 31),
                GroupId = 30,
                ReportId = 1,
                CommunityId = 1
            } };
        }
        #endregion
    }
}
