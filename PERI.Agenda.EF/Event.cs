using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Event
    {
        public Event()
        {
            Attendance = new HashSet<Attendance>();
            EventGroup = new HashSet<EventGroup>();
            Registrant = new HashSet<Registrant>();
            Rsvp = new HashSet<Rsvp>();
        }

        public int Id { get; set; }
        public int? LocationId { get; set; }
        public string Name { get; set; }
        public int EventCategoryId { get; set; }
        public string Description { get; set; }
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public int? RecurrenceId { get; set; }
        public bool? AllowRegistration { get; set; }
        public bool? IsExclusive { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }

        public EventCategory EventCategory { get; set; }
        public Location Location { get; set; }
        public ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<EventGroup> EventGroup { get; set; }
        public ICollection<Registrant> Registrant { get; set; }
        public ICollection<Rsvp> Rsvp { get; set; }
    }
}
