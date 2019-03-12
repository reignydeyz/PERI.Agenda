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

        private readonly IUnitOfWork unitOfWork;

        public EventCategory(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.EventCategory args)
        {
            await unitOfWork.EventCategoryRepository.AddAsync(args);
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
            unitOfWork.EventCategoryRepository.RemoveRange(unitOfWork.EventCategoryRepository.Entities.Where(x => ids.Contains(x.Id)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.EventCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.EventCategory args)
        {
            var ec = await unitOfWork.EventCategoryRepository.Entities.FirstAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);

            ec.Name = args.Name;
            ec.Description = args.Description;
            ec.DateTimeModified = DateTime.Now;
            ec.ModifiedBy = args.ModifiedBy;

            unitOfWork.Commit();
        }

        public IQueryable<EF.EventCategory> Find(EF.EventCategory args)
        {
            return unitOfWork.EventCategoryRepository.Entities
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.CommunityId == args.CommunityId)
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.EventCategory> Get(EF.EventCategory args)
        {
            return await unitOfWork.EventCategoryRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.CommunityId == args.CommunityId);
        }

        public async Task<IEnumerable<EF.EventCategory>> GetByIds(int[] ids)
        {
            return await unitOfWork.EventCategoryRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await unitOfWork.EventCategoryRepository.Entities.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
