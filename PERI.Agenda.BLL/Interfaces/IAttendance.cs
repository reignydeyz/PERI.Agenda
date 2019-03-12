using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IAttendance : ISampleData<EF.Attendance>
    {
        Task<IQueryable<EF.Attendance>> Registrants(int eventId);
        Task<IQueryable<EF.Attendance>> Registrants(int eventId, string member);
    }
}
