using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class LookUp
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int? Weight { get; set; }
    }
}
