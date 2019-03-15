using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Test
{
    public class TestRepo
    {
        public static List<EF.Member> Members => new List<EF.Member>
            {
                new EF.Member { Id = 1, Name = "ALVIN", Email = "abc@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 1 } },
                new EF.Member { Id = 2, Name = "CHUA", Email = "bcd@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 2 } },
                new EF.Member { Id = 3, Name = "JUAN", Email = "cde@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 3 } },
                new EF.Member { Id = 4, Name = "JOHN", Email = "def@y.com", CommunityId = 1 , EndUser =  new EF.EndUser { RoleId = 1, MemberId = 4 }}
            };

        public static List<EF.Role> Roles => new List<EF.Role>
            {
                new EF.Role { RoleId = 1, Name = "Admin" },
                new EF.Role { RoleId = 2, Name = "User" },
            };

        public static List<EF.EndUser> EndUsers = new List<EF.EndUser>
            {
                new EF.EndUser { RoleId = 1, MemberId = 1 },
                new EF.EndUser { RoleId = 1, MemberId = 2 },
                new EF.EndUser { RoleId = 1, MemberId = 3 },
                new EF.EndUser { RoleId = 1, MemberId = 4 },
            };
    }
}
