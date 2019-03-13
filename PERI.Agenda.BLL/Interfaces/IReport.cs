using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IReport : ISampleData<EF.Report>
    {
        Task<EF.Report> GetById(int id);

        Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user);
    }
}
