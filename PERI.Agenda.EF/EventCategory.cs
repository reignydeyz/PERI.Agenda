using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class EventCategory
    {
        public EventCategory()
        {
            Event = new HashSet<Event>();
            EventSection = new HashSet<EventSection>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllowMemberToLogOnMultipleEvents { get; set; }
        public bool AllowSectionOverlapping { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? CommunityId { get; set; }

        public ICollection<Event> Event { get; set; }
        public ICollection<EventSection> EventSection { get; set; }
    }
}
