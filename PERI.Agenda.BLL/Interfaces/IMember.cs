using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IMember : ISampleData<EF.Member>
    {
        Task<EF.Member> GetById(int id);
        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
        Task<int?> GetIdByName(string name, int communityId);
        IQueryable<EF.Attendance> Activities(int id);
        Task<int> Leading(int id);
        Task<int> Following(int id);
        Task<int> Invites(int id);
        Task<EF.Member> GetByEmail(string email);
    }
}
