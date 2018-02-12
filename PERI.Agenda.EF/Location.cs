using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Location
    {
        public Location()
        {
            Event = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? CommunityId { get; set; }

        public ICollection<Event> Event { get; set; }
    }
}
