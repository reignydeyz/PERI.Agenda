using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web
{
    public interface ITypedHubClient
    {
        Task AttendanceBroadcast(Models.Attendance args);
    }
}
