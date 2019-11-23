using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IRsvp : ISampleData<EF.Rsvp>
    {
        IQueryable<EF.Rsvp> Find(string name, int eventId, bool? isGoing);

        Task Update(EF.Rsvp args);
    }
}
