using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Registrant
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? GroupId { get; set; }
        public int MemberId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }

        public Event Event { get; set; }
        public Member Member { get; set; }
    }
}
