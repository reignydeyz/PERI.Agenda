using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface ILookUp
    {
        Task<IEnumerable<EF.LookUp>> GetByGroup(string group);
    }
}
