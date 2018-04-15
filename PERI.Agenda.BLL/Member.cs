using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;
using System.Linq;

namespace PERI.Agenda.BLL
{
    public class Member : IMember
    {
        EF.AARSContext context;

        public Member(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            foreach (var id in ids)
            {
                (await context.Member.FirstAsync(x => x.Id == id)).IsActive = true;
            }
            context.SaveChanges();
        }

        public async Task<int> Add(EF.Member args)
        {
            args.IsActive = true;
            var id = await context.Member.AddAsync(args);
            context.SaveChanges();
            return args.Id;
        }

        public async Task Deactivate(int[] ids)
        {
            foreach (var id in ids)
            {
                (await context.Member.FirstAsync(x => x.Id == id)).IsActive = false;
            }
            context.SaveChanges();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Member.RemoveRange(context.Member.Where(x => ids.Contains(x.Id)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Member args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Member args)
        {
            var user = await context.Member.FirstAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);

            user.Name = args.Name;
            user.NickName = args.NickName;
            user.BirthDate = args.BirthDate;
            user.Gender = args.Gender;
            user.Email = args.Email;
            user.Address = args.Address;
            user.Mobile = args.Mobile;
            user.IsActive = args.IsActive;

            context.SaveChanges();
        }

        public async Task<IEnumerable<EF.Member>> Find(EF.Member args)
        {
            return await context.Member.Where(x => x.Name.Contains(args.Name ?? "")
            && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<EF.Member> Get(EF.Member args)
        {
            return await context.Member.FirstOrDefaultAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await context.Member.Where(x => ids.Contains(x.Id) && x.CommunityId == user.CommunityId).CountAsync() == ids.Count();
        }
    }
}
