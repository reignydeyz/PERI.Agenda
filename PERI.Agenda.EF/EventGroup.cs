using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.EF
{
    public class EventGroup
    {
        public int EventId { get; set; }
        public int GroupId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Group Group { get; set; }
    }
}
