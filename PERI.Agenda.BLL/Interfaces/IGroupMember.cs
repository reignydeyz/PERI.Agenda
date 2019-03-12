using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IGroupMember
    {
        Task<int> Add(EF.GroupMember args);
        Task Delete(EF.GroupMember args);
        IQueryable<EF.Member> Members(EF.Member args, int id);
        IQueryable<EF.GroupMember> Checklist(EF.Member args, int id);
    }
}
