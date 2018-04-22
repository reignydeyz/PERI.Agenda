using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IAttendance : IRepository<EF.Attendance>
    {
        /// <summary>
        /// Gets the checklist for Attendance
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>List of Registrants</returns>
        Task<IQueryable<EF.Attendance>> Registrants(int eventId);
    }
}
