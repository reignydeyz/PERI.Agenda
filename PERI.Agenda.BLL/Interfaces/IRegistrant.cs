using System.Collections.Generic;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IRegistrant
    {
        Task Add(IEnumerable<EF.Registrant> args);
    }
}
