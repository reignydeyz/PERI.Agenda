using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class EventCategoryReport
    {
        public int EventCategoryId { get; set; }
        public int ReportId { get; set; }

        public EventCategory EventCategory { get; set; }
        public Report Report { get; set; }
    }
}
