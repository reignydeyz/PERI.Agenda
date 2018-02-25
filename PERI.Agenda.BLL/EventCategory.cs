using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class EventCategory : IEventCategory
    {
        EF.aarsdbContext context;

        public EventCategory(aarsdbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.EventCategory args)
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

        public Task Delete(EF.EventCategory args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.EventCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.EventCategory>> Find(EF.EventCategory args)
        {
            return await context.EventCategory.Where(x => x.Name.Contains(args.Name ?? ""))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public Task<EF.EventCategory> Get(EF.EventCategory args)
        {
            throw new NotImplementedException();
        }
    }
}
