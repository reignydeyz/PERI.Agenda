using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Recurrence
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Pattern { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
