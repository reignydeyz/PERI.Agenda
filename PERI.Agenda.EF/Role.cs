using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Role
    {
        public Role()
        {
            EndUser = new HashSet<EndUser>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<EndUser> EndUser { get; set; }
    }
}
