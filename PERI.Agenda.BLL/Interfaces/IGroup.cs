using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IGroup : ISampleData<EF.Group>
    {
        IQueryable<EF.Attendance> Activities(int id);

        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
    }
}
