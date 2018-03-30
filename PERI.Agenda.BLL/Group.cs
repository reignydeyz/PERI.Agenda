using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Agenda.BLL
{
    public class Group : IGroup
    {
        AARSContext context;

        public Group(EF.AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Group>> Find(EF.Group args)
        {
            return await context.Group
                .Include(x => x.GroupMember).ThenInclude(x => x.Member)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<EF.Group> Get(EF.Group args)
        {
            return await context.Group
                .Include(x => x.GroupCategory)
                .Where(x => x.Id == args.Id).FirstOrDefaultAsync();
        }
    }
}
