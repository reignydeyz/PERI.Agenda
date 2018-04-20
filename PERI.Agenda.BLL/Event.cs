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
        EF.AARSContext context;

        public Event(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Event args)
        {
            args.IsActive = true;
            var id = await context.Event.AddAsync(args);
            context.SaveChanges();
            return args.Id;
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Event.RemoveRange(context.Event.Where(x => ids.Contains(x.Id)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Event args)
        {
            var e = await context.Event.FirstAsync(x => x.Id == args.Id
            && x.EventCategory.CommunityId == args.EventCategory.CommunityId);

            e.Name = args.Name;
            e.LocationId = args.LocationId;
            e.EventCategoryId = args.EventCategoryId;
            e.DateTimeStart = args.DateTimeStart;

            context.SaveChanges();
        }

        public IQueryable<EF.Event> Find(EF.Event args)
        {
            return context.Event
            .Include(x => x.EventCategory)
            .Include(x => x.Attendance)
            .Include(x => x.Location)
            .Where(x => (x.DateTimeStart >= (args.DateTimeStart ?? x.DateTimeStart) && x.DateTimeStart <= (args.DateTimeEnd ?? DateTime.MaxValue))
            && x.Name.Contains(args.Name ?? "")
            && x.EventCategoryId == (args.EventCategoryId == 0 ? x.EventCategoryId : args.EventCategoryId)
            && x.LocationId == ((args.LocationId ?? 0) == 0 ? x.LocationId : args.LocationId)
            && x.EventCategory.CommunityId == args.EventCategory.CommunityId)
            .OrderByDescending(x => x.DateTimeStart).AsQueryable();
        }

        public async Task<EF.Event> Get(EF.Event args)
        {
            return await context.Event
                .Include(x => x.EventCategory)
                .Include(x => x.Attendance)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.EventCategory.CommunityId == args.EventCategory.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await context.Event.Where(x => ids.Contains(x.Id) && x.EventCategory.CommunityId == user.CommunityId).CountAsync() == ids.Count();
        }
    }
}
