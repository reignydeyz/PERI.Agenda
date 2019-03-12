using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IEvent : ISampleData<EF.Event>
    {
        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
        IQueryable<EF.Event> Calendar(int memberId, int communityId);
    }
}
