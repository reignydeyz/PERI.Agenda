using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IGroupCategory : ISampleData<EF.GroupCategory>
    {
        Task<bool> AreSelectedIdsOk(int[] ids, EF.EndUser user);

        Task<List<EF.Member>> Members(int id);
    }
}
