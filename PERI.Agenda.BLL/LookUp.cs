using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.BLL
{
    public class LookUp : ILookUp
    {
        EF.aarsdbContext context;

        public LookUp(aarsdbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task<IEnumerable<EF.LookUp>> GetByGroup(string group)
        {
            return await context.LookUp.Where(x => x.Group == group).OrderBy(x => x.Weight).ToListAsync();
        }
    }
}
