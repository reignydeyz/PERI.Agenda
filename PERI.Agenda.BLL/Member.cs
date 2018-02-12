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

        public Task Delete(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Member args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Member args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Member>> Find(EF.Member args)
        {
            return await context.Member.Where(x => x.Name.Contains(args.Name ?? "")).ToListAsync();
        }

        public Task<EF.Member> Get(EF.Member args)
        {
            throw new NotImplementedException();
        }
    }
}
