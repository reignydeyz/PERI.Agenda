using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Test
{
    public class TestRepo
    {
        public List<EF.Member> Members => new List<EF.Member>
        {
            new EF.Member { Id = 1, Name = "ALVIN", Email = "abc@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 1 } },
            new EF.Member { Id = 2, Name = "CHUA", Email = "bcd@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 2 } },
            new EF.Member { Id = 3, Name = "JUAN", Email = "cde@y.com", CommunityId = 1, EndUser = new EF.EndUser { RoleId = 1, MemberId = 3 } },
            new EF.Member { Id = 4, Name = "JOHN", Email = "def@y.com", CommunityId = 1 , EndUser =  new EF.EndUser { RoleId = 1, MemberId = 4 }}
        };

        public List<EF.Role> Roles => new List<EF.Role>
        {
            new EF.Role { RoleId = 1, Name = "Admin" },
            new EF.Role { RoleId = 2, Name = "User" },
        };

        public List<EF.EndUser> EndUsers = new List<EF.EndUser>
        {
            new EF.EndUser { RoleId = 1, MemberId = 1 },
            new EF.EndUser { RoleId = 1, MemberId = 2 },
            new EF.EndUser { RoleId = 1, MemberId = 3 },
            new EF.EndUser { RoleId = 1, MemberId = 4 },
        };

        public List<EF.Event> Events = new List<EF.Event>
        {
            new EF.Event { Id=1, Name = "EVENT0001", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(1), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=2,Name = "EVENT0002", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(2), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=3,Name = "EVENT0003", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(3), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=4,Name = "EVENT0004", EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=5,Name = "EVENT0005", EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=6,Name = "EVENT0006", EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=7,Name = "EVENT0007", EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=8,Name = "EVENT0008", EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=9,Name = "EVENT0009", EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=10,Name = "EVENT0010", EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
            new EF.Event { Id=11,Name = "EVENT0011", EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
            new EF.Event { Id=12,Name = "EVENT0012", EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
        };
    }
}
