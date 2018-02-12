using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class EventSection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventCategoryId { get; set; }
        public string Description { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }

        public EventCategory EventCategory { get; set; }
    }
}
