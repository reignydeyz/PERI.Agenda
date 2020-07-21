using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IEvent : ISampleData<EF.Event>
    {
        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
        IQueryable<EF.Event> Calendar(int memberId, int communityId);
        Task<int> Add(EF.Event e, int groupId);
    }
}
