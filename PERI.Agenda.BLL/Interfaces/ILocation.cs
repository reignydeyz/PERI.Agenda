using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface ILocation : ISampleData<EF.Location>
    {
        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
    }
}
