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
        EF.AARSContext context;

        public EventCategory(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.EventCategory args)
        {
            await context.EventCategory.AddAsync(args);
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
            context.EventCategory.RemoveRange(context.EventCategory.Where(x => ids.Contains(x.Id)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.EventCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.EventCategory args)
        {
            var ec = await context.EventCategory.FirstAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);

            ec.Name = args.Name;
            ec.Description = args.Description;
            ec.DateTimeModified = DateTime.Now;
            ec.ModifiedBy = ec.ModifiedBy;

            context.SaveChanges();
        }

        public IQueryable<EF.EventCategory> Find(EF.EventCategory args)
        {
            return context.EventCategory
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.CommunityId == args.CommunityId)
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.EventCategory> Get(EF.EventCategory args)
        {
            return await context.EventCategory
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.CommunityId == args.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await context.EventCategory.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
