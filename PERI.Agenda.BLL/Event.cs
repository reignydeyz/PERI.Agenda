using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Event : IEvent
    {

        private readonly IUnitOfWork unitOfWork;

        public Event(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Event args)
        {
            args.IsActive = true;
            await unitOfWork.EventRepository.AddAsync(args);
            unitOfWork.Commit();
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
            unitOfWork.EventRepository.RemoveRange(unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.Id)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Event args)
        {
            var e = await unitOfWork.EventRepository.Entities.FirstAsync(x => x.Id == args.Id
            && x.EventCategory.CommunityId == args.EventCategory.CommunityId);

            e.Name = args.Name;
            e.LocationId = args.LocationId;
            e.EventCategoryId = args.EventCategoryId;
            e.DateTimeStart = args.DateTimeStart;

            unitOfWork.Commit();
        }

        public IQueryable<EF.Event> Find(EF.Event args)
        {
            return unitOfWork.EventRepository.Entities
            .Include(x => x.EventCategory)
            .Include(x => x.Attendance)
            .Include(x => x.Location)
            .Include(x => x.EventGroup)
            .Where(x => (x.DateTimeStart >= (args.DateTimeStart ?? x.DateTimeStart) && x.DateTimeStart <= (args.DateTimeEnd ?? DateTime.Now.AddYears(100)))
            && x.Name.Contains(args.Name ?? "")
            && x.EventCategoryId == (args.EventCategoryId == 0 ? x.EventCategoryId : args.EventCategoryId)
            && x.LocationId == ((args.LocationId ?? 0) == 0 ? x.LocationId : args.LocationId)
            && x.EventCategory.CommunityId == args.EventCategory.CommunityId)
            .OrderByDescending(x => x.DateTimeStart).AsQueryable();
        }

        public async Task<EF.Event> Get(EF.Event args)
        {
            return await unitOfWork.EventRepository.Entities
                .Include(x => x.EventCategory)
                .Include(x => x.Attendance).ThenInclude(x => x.FirstTimer)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.EventCategory.CommunityId == args.EventCategory.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            switch (user.RoleId)
            {
                case 1:
                case 3:
                    return await unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.Id) && x.EventCategory.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
                default:
                    return await unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.Id) && x.EventCategory.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count()
                        && await unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.Id) && x.CreatedBy == user.Member.Name).CountAsync() == ids.Count();
            }
        }

        /// <summary>
        /// Gets upcoming events
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="communityId"></param>
        /// <returns>List of events</returns>
        public IQueryable<EF.Event> Calendar(int memberId, int communityId)
        {
            return unitOfWork.EventRepository.Entities
            .Include(x => x.Registrant)
            .Include(x => x.EventCategory)
            .Include(x => x.Attendance)
            .Include(x => x.Location)
            .Include(x => x.Rsvp)
            .Where(x => (x.DateTimeStart >= DateTime.Today && x.DateTimeStart <= DateTime.Now.AddMonths(3))
            && (x.IsExclusive == false 
                || x.IsExclusive == null
                || x.IsExclusive == true && x.Registrant.Select(y => y.MemberId).Contains(memberId))
            && x.EventCategory.CommunityId == communityId
            && x.Attendance.Count(y => y.MemberId == memberId) <= 0);
        }

        public async Task<int> Add(EF.Event e, int groupId)
        {
            var id = await this.Add(e);

            await unitOfWork.EventGroupRepository.AddAsync(new EF.EventGroup { EventId = id, GroupId = groupId });

            return id;
        }
    }
}
