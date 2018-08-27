using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Attendance
    {
        private readonly UnitOfWork unitOfWork;

        public Attendance(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<int> Add(EF.Attendance args)
        {
            await unitOfWork.AttendanceRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

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

        public async Task Delete(EF.Attendance args)
        {
            var a = await unitOfWork.AttendanceRepository.Entities.FirstAsync(x => x.MemberId == args.MemberId && x.EventId == args.EventId);
            unitOfWork.AttendanceRepository.Remove(a);
            unitOfWork.Commit();
        }

        public Task Edit(EF.Attendance args)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EF.Attendance> Find(EF.Attendance args)
        {
            return unitOfWork.AttendanceRepository.Entities
                .Include(x => x.Member)
                .Include(x => x.FirstTimer)
                .Where(x => x.EventId == args.EventId
            && x.EventSectionId == (args.EventSectionId ?? x.EventSectionId)).AsQueryable();
        }

        public IQueryable<EF.Attendance> FindByEventCategoryIds(int[] ids)
        {
            return unitOfWork.AttendanceRepository.Entities
                .Include(x => x.Member)
                .Where(x => ids.Contains(x.Event.EventCategoryId)).AsQueryable();
        }

        public Task<EF.Attendance> Get(EF.Attendance args)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<EF.Attendance>> Registrants(int eventId)
        {
            var ev = await unitOfWork.EventRepository.Entities
                .Include(x => x.EventCategory)
                .FirstAsync(x => x.Id == eventId);

            var members = unitOfWork.MemberRepository.Entities.Where(x => x.IsActive && x.CommunityId == ev.EventCategory.CommunityId);
            var registrants = unitOfWork.RegistrantRepository.Entities.Where(x => x.EventId == eventId);
            var attendance = unitOfWork.AttendanceRepository.Entities.Where(x => x.EventId == eventId);

            if (ev.IsExclusive == false || ev.IsExclusive == null)
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

        public async Task<IQueryable<EF.Attendance>> Registrants(int eventId, string member)
        {
            member = member == null ? string.Empty : member.ToLower();

            var ev = await unitOfWork.EventRepository.Entities
                .Include(x => x.EventCategory)
                .FirstAsync(x => x.Id == eventId);

            var members = unitOfWork.MemberRepository.Entities.Where(x => x.IsActive && x.CommunityId  == ev.EventCategory.CommunityId);
            var registrants = unitOfWork.RegistrantRepository.Entities.Where(x => x.EventId == eventId);
            var attendance = unitOfWork.AttendanceRepository.Entities.Where(x => x.EventId == eventId);

            if (ev.IsExclusive == false || ev.IsExclusive == null)
            {
                var res = from m in members
                          join a in attendance on m.Id equals a.MemberId into leftr
                          from lr in leftr.DefaultIfEmpty()
                          where m.Name.ToLower().Contains(member)
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
                          where m.Name.ToLower().Contains(member)
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
