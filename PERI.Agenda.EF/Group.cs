using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Group
    {
        public Group()
        {
            GroupMember = new HashSet<GroupMember>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? GroupCategoryId { get; set; }
        public int? GroupLeader { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public GroupCategory GroupCategory { get; set; }
        public ICollection<GroupMember> GroupMember { get; set; }
    }
}
