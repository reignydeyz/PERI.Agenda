using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Location : ILocation
    {
        EF.AARSContext context;

        public Location(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Location args)
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

        public Task Delete(EF.Location args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Location args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Location>> Find(EF.Location args)
        {
            return await context.Location
                .Where(x => x.Name.Contains(args.Name ?? ""))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public Task<EF.Location> Get(EF.Location args)
        {
            throw new NotImplementedException();
        }
    }
}
