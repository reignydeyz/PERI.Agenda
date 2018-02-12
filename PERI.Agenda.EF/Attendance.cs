using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? EventSectionId { get; set; }
        public int MemberId { get; set; }
        public DateTime? DateTimeLogged { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public Event Event { get; set; }
        public Member Member { get; set; }
    }
}
