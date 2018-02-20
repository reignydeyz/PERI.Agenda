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
        EF.aarsdbContext context;

        public Member(aarsdbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Member args)
        {
            args.IsActive = true;
            var id = await context.Member.AddAsync(args);
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
            context.Member.RemoveRange(context.Member.Where(x => ids.Contains(x.Id)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Member args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Member args)
        {
            var user = await context.Member.FirstAsync(x => x.Id == args.Id);

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
            return await context.Member.Where(x => x.Name.Contains(args.Name ?? "")).OrderBy(x => x.Name).Take(1000).ToListAsync();
        }

        public async Task<EF.Member> Get(EF.Member args)
        {
            return await context.Member.FirstOrDefaultAsync(x => x.Id == args.Id);
        }
    }
}
