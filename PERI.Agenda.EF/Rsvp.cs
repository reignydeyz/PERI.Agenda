using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Rsvp
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public bool IsGoing { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Event Event { get; set; }
        public Member Member { get; set; }
    }
}
