using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class GroupMember
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? MemberId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }

        public Group Group { get; set; }
        public Member Member { get; set; }
    }
}
