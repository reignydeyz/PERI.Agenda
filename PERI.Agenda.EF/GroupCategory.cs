using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class GroupCategory
    {
        public GroupCategory()
        {
            Group = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllowMemberToJoinMultipleGroups { get; set; }
        public bool AllowLeaderToHandleMultipleGroups { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? CommunityId { get; set; }

        public ICollection<Group> Group { get; set; }
    }
}
