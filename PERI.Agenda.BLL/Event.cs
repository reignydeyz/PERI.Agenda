using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Event : IEvent
    {
        EF.aarsdbContext context;

        public Event(aarsdbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Event args)
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

        public Task Delete(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Event>> Find(EF.Event args)
        {
            return await context.Event
            .Include(x => x.EventCategory)
            .Include(x => x.Attendance)
            .Where(x => x.DateTimeStart == (args.DateTimeStart ?? x.DateTimeStart)
            && x.Name.Contains(args.Name ?? "")
            && x.EventCategoryId == (args.EventCategoryId == 0 ? x.EventCategoryId : args.EventCategoryId))
            .OrderByDescending(x => x.DateTimeStart)
            .ToListAsync();
        }

        public Task<EF.Event> Get(EF.Event args)
        {
            throw new NotImplementedException();
        }
    }
}
