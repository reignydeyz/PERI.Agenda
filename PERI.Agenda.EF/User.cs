using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? CommunityId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
