using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Test
{
    public class TestDataGenerator
    {
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
    }
}
