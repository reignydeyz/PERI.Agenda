using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IEventCategory : ISampleData<EF.EventCategory>
    {
        Task<IEnumerable<EF.EventCategory>> GetByIds(int[] ids);

        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
    }
}
