using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class FirstTimer
    {
        public int AttendanceId { get; set; }

        public Attendance Attendance { get; set; }
    }
}
