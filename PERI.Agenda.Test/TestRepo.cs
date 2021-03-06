﻿using System;
using System.Collections.Generic;

namespace PERI.Agenda.Test
{
    public class TestRepo
    {
        public List<EF.Member> Members => new List<EF.Member>
        {
            new EF.Member { Id = 1, Name = "ALVIN", Email = "abc@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 1 } },
            new EF.Member { Id = 2, Name = "CHUA", Email = "bcd@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 2 } },
            new EF.Member { Id = 3, Name = "JUAN", Email = "cde@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 3 } },
            new EF.Member { Id = 4, Name = "JOHN", Email = "def@y.com", CommunityId = 1, IsActive = true, EndUser =  new EF.EndUser { RoleId = 1, MemberId = 4 }},
            new EF.Member { Id = 5, Name = "ALVIN1", Email = "abc1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 5 } },
            new EF.Member { Id = 6, Name = "CHUA1", Email = "bcd1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 6 } },
            new EF.Member { Id = 7, Name = "JUAN1", Email = "cde1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 7 } },
            new EF.Member { Id = 8, Name = "JOHN1", Email = "def1@y.com", CommunityId = 1 , IsActive = true, EndUser =  new EF.EndUser { RoleId = 1, MemberId = 8 }},
            new EF.Member { Id = 9, Name = "ALVIN2", Email = "abc2@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 9 } },
            new EF.Member { Id = 10, Name = "CHUA2", Email = "bcd2@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 10 } },
            new EF.Member { Id = 11, Name = "JUAN2", Email = "cde2@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 11 } },
            new EF.Member { Id = 12, Name = "JOHN2", Email = "def2@y.com", CommunityId = 1, IsActive = true, EndUser =  new EF.EndUser { RoleId = 1, MemberId = 12 }},
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
            new EF.EndUser { RoleId = 1, MemberId = 5 },
            new EF.EndUser { RoleId = 1, MemberId = 6 },
            new EF.EndUser { RoleId = 1, MemberId = 7 },
            new EF.EndUser { RoleId = 1, MemberId = 8 },
            new EF.EndUser { RoleId = 1, MemberId = 9 },
            new EF.EndUser { RoleId = 1, MemberId = 10 },
            new EF.EndUser { RoleId = 1, MemberId = 11 },
            new EF.EndUser { RoleId = 1, MemberId = 12 },
        };

        public List<EF.Event> Events = new List<EF.Event>
        {
            new EF.Event { Id=1, Name = "EVENT0001",LocationId = 1,  EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(1), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=2,Name = "EVENT0002", LocationId = 2,  EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(2), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=3,Name = "EVENT0003", LocationId = 3,  EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(3), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } },
            new EF.Event { Id=4,Name = "EVENT0004", LocationId = 4,  EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=5,Name = "EVENT0005", LocationId = 5,  EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=6,Name = "EVENT0006", LocationId = 6,  EventCategoryId = 2, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 } },
            new EF.Event { Id=7,Name = "EVENT0007", LocationId = 7,  EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=8,Name = "EVENT0008", LocationId = 8,  EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=9,Name = "EVENT0009", LocationId = 9,  EventCategoryId = 3, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 } },
            new EF.Event { Id=10,Name = "EVENT0010",LocationId = 10, EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(4), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
            new EF.Event { Id=11,Name = "EVENT0011",LocationId = 11, EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(5), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
            new EF.Event { Id=12,Name = "EVENT0012",LocationId = 12, EventCategoryId = 4, DateTimeStart = DateTime.Now.AddDays(6), IsExclusive = true, IsActive = true, EventCategory = new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 } },
        };

        public List<EF.EventCategory> EventCategories = new List<EF.EventCategory>
        {
            new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 },
            new EF.EventCategory { Id = 2, Name = "Category2", CommunityId = 1 },
            new EF.EventCategory { Id = 3, Name = "Category3", CommunityId = 1 },
            new EF.EventCategory { Id = 4, Name = "Category4", CommunityId = 1 },
            new EF.EventCategory { Id = 5, Name = "Category5", CommunityId = 1 },
            new EF.EventCategory { Id = 6, Name = "Category6", CommunityId = 1 },
            new EF.EventCategory { Id = 7, Name = "Category7", CommunityId = 1 },
            new EF.EventCategory { Id = 8, Name = "Category8", CommunityId = 1 },
            new EF.EventCategory { Id = 9, Name = "Category9", CommunityId = 1 },
            new EF.EventCategory { Id = 10, Name = "Category10", CommunityId = 1 },
        };

        public List<EF.Registrant> Registrants = new List<EF.Registrant>
        {
            new EF.Registrant { Id = 1, EventId = 1, MemberId = 1 },
            new EF.Registrant { Id = 2, EventId = 1, MemberId = 2 },
            new EF.Registrant { Id = 3, EventId = 1, MemberId = 3 },
            new EF.Registrant { Id = 4, EventId = 2, MemberId = 1 },
            new EF.Registrant { Id = 5, EventId = 2, MemberId = 2 },
            new EF.Registrant { Id = 6, EventId = 2, MemberId = 3 },
            new EF.Registrant { Id = 7, EventId = 3, MemberId = 1 },
            new EF.Registrant { Id = 8, EventId = 3, MemberId = 2 },
            new EF.Registrant { Id = 9, EventId = 3, MemberId = 3 },
            new EF.Registrant { Id = 10, EventId = 4, MemberId = 1 },
            new EF.Registrant { Id = 11, EventId = 4, MemberId = 2 },
            new EF.Registrant { Id = 12, EventId = 4, MemberId = 3 },
            new EF.Registrant { Id = 13, EventId = 7, MemberId = 1 },
            new EF.Registrant { Id = 14, EventId = 7, MemberId = 2 },
            new EF.Registrant { Id = 15, EventId = 7, MemberId = 3 },
            new EF.Registrant { Id = 16, EventId = 8, MemberId = 1 },
            new EF.Registrant { Id = 17, EventId = 8, MemberId = 2 },
            new EF.Registrant { Id = 18, EventId = 8, MemberId = 3 },
            new EF.Registrant { Id = 19, EventId = 9, MemberId = 1 },
            new EF.Registrant { Id = 20, EventId = 9, MemberId = 2 },
            new EF.Registrant { Id = 21, EventId = 9, MemberId = 3 },
        };

        public List<EF.Attendance> Attendances = new List<EF.Attendance>
        {
            new EF.Attendance { Id = 1, EventId = 1, MemberId = 1, Event = new EF.Event { Id=1, Name = "EVENT0001", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(1), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
            new EF.Attendance { Id = 2, EventId = 1, MemberId = 2, Event = new EF.Event { Id=1, Name = "EVENT0001", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(1), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
            new EF.Attendance { Id = 3, EventId = 2, MemberId = 1, Event = new EF.Event { Id=2,Name = "EVENT0002", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(2), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
            new EF.Attendance { Id = 4, EventId = 2, MemberId = 2, Event = new EF.Event { Id=2,Name = "EVENT0002", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(2), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
            new EF.Attendance { Id = 5, EventId = 3, MemberId = 1, Event = new EF.Event { Id=3,Name = "EVENT0003", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(3), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
            new EF.Attendance { Id = 6, EventId = 3, MemberId = 2, Event = new EF.Event { Id=3,Name = "EVENT0003", EventCategoryId = 1, DateTimeStart = DateTime.Now.AddDays(3), IsExclusive = false, IsActive = true, EventCategory = new EF.EventCategory { Id = 1, Name = "Category1", CommunityId = 1 } } },
        };

        public List<EF.Group> Groups = new List<EF.Group>
        {
            new EF.Group { Id = 1, Name = "Group1", GroupLeader = 10, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } },
            new EF.Group { Id = 2, Name = "Group2", GroupLeader = 11, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } },
            new EF.Group { Id = 3, Name = "Group3", GroupLeader = 12, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } },
        };

        public List<EF.GroupCategory> GroupCategories = new List<EF.GroupCategory>
        {
            new EF.GroupCategory { Id = 1, Name = "Category1", CommunityId = 1 },
            new EF.GroupCategory { Id = 2, Name = "Category2", CommunityId = 1 },
            new EF.GroupCategory { Id = 3, Name = "Category3", CommunityId = 1 },
            new EF.GroupCategory { Id = 4, Name = "Category4", CommunityId = 1 },
            new EF.GroupCategory { Id = 5, Name = "Category5", CommunityId = 1 },
            new EF.GroupCategory { Id = 6, Name = "Category6", CommunityId = 1 },
        };

        public List<EF.GroupMember> GroupMembers = new List<EF.GroupMember>
        {
            new EF.GroupMember { Member = new EF.Member { Id = 1, Name = "ALVIN" }, MemberId = 1, GroupId = 1, Group = new EF.Group { Id = 1, Name = "Group1", GroupLeader = 10, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } } },
            new EF.GroupMember { Member = new EF.Member { Id = 2, Name = "CHUA" }, MemberId = 2, GroupId = 1, Group = new EF.Group { Id = 1, Name = "Group1", GroupLeader = 10, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } } },
            new EF.GroupMember { Member = new EF.Member { Id = 3, Name = "JUAN" }, MemberId = 3, GroupId = 1, Group = new EF.Group { Id = 1, Name = "Group1", GroupLeader = 10, GroupCategoryId = 1, GroupCategory = new EF.GroupCategory { CommunityId = 1 } } },
        };

        public List<EF.Rsvp> Rsvps = new List<EF.Rsvp>
        {
            new EF.Rsvp { EventId = 1, MemberId = 1, IsGoing = true, Member = new EF.Member { Id = 1, Name = "ALVIN", Email = "abc@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 1 } }, },
            new EF.Rsvp { EventId = 1, MemberId = 2, IsGoing = true, Member = new EF.Member { Id = 2, Name = "CHUA", Email = "bcd@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 2 } },},
            new EF.Rsvp { EventId = 1, MemberId = 3, IsGoing = true, Member = new EF.Member { Id = 3, Name = "JUAN", Email = "cde@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 3 } }, },
            new EF.Rsvp { EventId = 1, MemberId = 4, IsGoing = true, Member = new EF.Member { Id = 4, Name = "JOHN", Email = "def@y.com", CommunityId = 1, IsActive = true, EndUser =  new EF.EndUser { RoleId = 1, MemberId = 4 }}, },
            new EF.Rsvp { EventId = 1, MemberId = 5, IsGoing = true, Member = new EF.Member { Id = 5, Name = "ALVIN1", Email = "abc1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 5 } }, },
            new EF.Rsvp { EventId = 1, MemberId = 6, IsGoing = true, Member = new EF.Member { Id = 6, Name = "CHUA1", Email = "bcd1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 6 } }, },
            new EF.Rsvp { EventId = 1, MemberId = 7, IsGoing = true, Member = new EF.Member { Id = 7, Name = "JUAN1", Email = "cde1@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 7 } }, },
            new EF.Rsvp { EventId = 1, MemberId = 8, IsGoing = true, Member = new EF.Member { Id = 8, Name = "JOHN1", Email = "def1@y.com", CommunityId = 1 , IsActive = true, EndUser =  new EF.EndUser { RoleId = 1, MemberId = 8 }}, },
            new EF.Rsvp { EventId = 1, MemberId = 9, IsGoing = true, Member = new EF.Member { Id = 9, Name = "ALVIN2", Email = "abc2@y.com", CommunityId = 1, IsActive = true, EndUser = new EF.EndUser { RoleId = 1, MemberId = 9 } }, },
        };

        public List<EF.Report> Reports = new List<EF.Report>
        {
            new EF.Report { CommunityId = 1, ReportId = 1, Name = "Report1" },
            new EF.Report { CommunityId = 1, ReportId = 2, Name = "Report2" },
            new EF.Report { CommunityId = 1, ReportId = 3, Name = "Report3" },
            new EF.Report { CommunityId = 1, ReportId = 4, Name = "Report4" },
            new EF.Report { CommunityId = 1, ReportId = 5, Name = "Report5" },
            new EF.Report { CommunityId = 2, ReportId = 6, Name = "Report19" },
            new EF.Report { CommunityId = 2, ReportId = 7, Name = "Report29" },
            new EF.Report { CommunityId = 2, ReportId = 8, Name = "Report39" },
            new EF.Report { CommunityId = 2, ReportId = 9, Name = "Report49" },
            new EF.Report { CommunityId = 2, ReportId = 10, Name = "Report59" },
        };

        public List<EF.Location> Locations = new List<EF.Location>
        {
            new EF.Location {Id = 1, CommunityId = 1, Name = "Location1" },
            new EF.Location {Id = 2, CommunityId = 1, Name = "Location2" },
            new EF.Location {Id = 3, CommunityId = 1, Name = "Location3" },
            new EF.Location {Id = 4, CommunityId = 1, Name = "Location4" },
            new EF.Location {Id = 5, CommunityId = 1, Name = "Location5" },
            new EF.Location {Id = 6, CommunityId = 1, Name = "Location19" },
            new EF.Location {Id = 7, CommunityId = 1, Name = "Location29" },
            new EF.Location {Id = 8, CommunityId = 1, Name = "Location39" },
            new EF.Location {Id = 9, CommunityId = 1, Name = "Location49" },
            new EF.Location {Id = 10, CommunityId = 1, Name = "Location59" },
            new EF.Location {Id = 11, CommunityId = 1, Name = "Location69" },
            new EF.Location {Id = 12, CommunityId = 1, Name = "Location79" },
        };
    }
}
