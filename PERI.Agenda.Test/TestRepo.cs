using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Test
{
    public class TestRepo
    {
        public static IEnumerable<EF.Member> Members => new List<EF.Member>
            {
                new EF.Member { Id = 1, Name = "ALVIN", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 1 } },
                new EF.Member { Id = 2, Name = "CHUA", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 2 } },
                new EF.Member { Id = 3, Name = "JUAN", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 3 } },
                new EF.Member { Id = 4, Name = "JOHN", CommunityId = 1 , EndUser =  new EF.EndUser { RoleId = 1, MemberId = 4 }}
            };

        public static IEnumerable<EF.Role> Roles => new List<EF.Role>
            {
                new EF.Role { RoleId = 1, Name = "Admin" },
                new EF.Role { RoleId = 2, Name = "User" },
            };

        public static IEnumerable<EF.EndUser> EndUsers = new List<EF.EndUser>
            {
                new EF.EndUser { RoleId = 1, MemberId = 1 },
                new EF.EndUser { RoleId = 1, MemberId = 2 },
                new EF.EndUser { RoleId = 1, MemberId = 3 },
                new EF.EndUser { RoleId = 1, MemberId = 4 },
            };
    }
}
