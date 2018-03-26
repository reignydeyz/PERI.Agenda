using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.BLL
{
    public class GroupCategory : IRepository<EF.GroupCategory>
    {
        AARSContext context;

        public GroupCategory(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.GroupCategory args)
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

        public Task Delete(EF.GroupCategory args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.GroupCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.GroupCategory>> Find(EF.GroupCategory args)
        {
            return await context.GroupCategory
                .Include(x => x.Group).ThenInclude(x => x.GroupMember).ToListAsync();
        }

        public Task<EF.GroupCategory> Get(EF.GroupCategory args)
        {
            throw new NotImplementedException();
        }
    }
}
