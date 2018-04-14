using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Member
    {
        public Member()
        {
            Attendance = new HashSet<Attendance>();
            GroupMember = new HashSet<GroupMember>();
            Registrant = new HashSet<Registrant>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Remarks { get; set; }
        public int? CivilStatus { get; set; }
        public int? Gender { get; set; }
        public int? InvitedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? CommunityId { get; set; }

        public ICollection<Attendance> Attendance { get; set; }
        public ICollection<GroupMember> GroupMember { get; set; }
        public ICollection<Registrant> Registrant { get; set; }
    }
}
