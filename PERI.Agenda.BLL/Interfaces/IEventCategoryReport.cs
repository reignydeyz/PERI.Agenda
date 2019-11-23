using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IEventCategoryReport
    {
        Task AddRange(IEnumerable<EF.EventCategoryReport> args);

        Task Update(IEnumerable<EF.EventCategoryReport> args);

        Task Delete(EF.EventCategoryReport args);
    }
}
