using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Attendance : IAttendance
    {
        EF.AARSContext context;

        public Attendance(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Attendance args)
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

        public Task Delete(EF.Attendance args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Attendance args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Attendance>> Find(EF.Attendance args)
        {
            return await context.Attendance
                .Include(x => x.Member)
                .Where(x => x.EventId == args.EventId
            && x.EventSectionId == (args.EventSectionId ?? x.EventSectionId))
            .ToListAsync();
        }

        public Task<EF.Attendance> Get(EF.Attendance args)
        {
            throw new NotImplementedException();
        }
    }
}
