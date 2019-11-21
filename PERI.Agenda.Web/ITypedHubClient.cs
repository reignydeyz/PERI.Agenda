using System.Threading.Tasks;

namespace PERI.Agenda.Web
{
    public interface ITypedHubClient
    {
        Task AttendanceBroadcast(Models.Attendance args);
    }
}
