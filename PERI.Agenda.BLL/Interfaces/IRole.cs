using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IRole
    {
        Task<IEnumerable<EF.Role>> GetAll();
    }
}
