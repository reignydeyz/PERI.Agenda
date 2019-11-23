using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IEndUser : ISampleData<EF.EndUser>
    {
        Task<EF.EndUser> GetById(int id);
        Task<EF.EndUser> GetByEmail(string email);
        Task UpdateRole(EF.EndUser endUser);
    }
}
