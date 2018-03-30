﻿using System;
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

        public async Task<IEnumerable<EF.Attendance>> Registrants(int eventId)
        {
            var ev = await context.Event.FirstAsync(x => x.Id == eventId);

            var members = await context.Member.Where(x => x.IsActive).ToListAsync();
            var registrants = await context.Registrant.Where(x => x.EventId == eventId).ToListAsync();
            var attendance = await context.Attendance.Where(x => x.EventId == eventId).ToListAsync();

            if (ev.IsExclusive == false)
            {
                var res = from m in members
                          join a in attendance on m.Id equals a.MemberId into leftr
                          from lr in leftr.DefaultIfEmpty()
                          select new EF.Attendance
                          {
                              Member = m,
                              MemberId = m.Id,
                              DateTimeLogged = (lr == null ? null : lr.DateTimeLogged)
                          };

                return res.OrderBy(x => x.Member.Name);
            }
            else
            {
                var res = from m in members
                          join r in registrants on m.Id equals r.MemberId
                          join a in attendance on r.MemberId equals a.MemberId into leftr
                          from lr in leftr.DefaultIfEmpty()
                          select new EF.Attendance
                          {
                              Member = m,
                              MemberId = m.Id,
                              DateTimeLogged = (lr == null ? null : lr.DateTimeLogged)
                          };

                return res.OrderBy(x => x.Member.Name);
            }
        }
    }
}
