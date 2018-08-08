using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Agenda.BLL
{
    public class Group
    {

        private readonly UnitOfWork unitOfWork;

        public Group(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Group args)
        {
            await unitOfWork.GroupRepository.AddAsync(args);
            unitOfWork.Commit();
            return args.Id;
        }

        public async Task Deactivate(int[] ids)
        {
            unitOfWork.GroupRepository.RemoveRange(unitOfWork.GroupRepository.Entities.Where(x => ids.Contains(x.Id)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.GroupRepository.RemoveRange(unitOfWork.GroupRepository.Entities.Where(x => ids.Contains(x.Id)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Group args)
        {
            var g = await unitOfWork.GroupRepository.Entities.FirstAsync(x => x.Id == args.Id);

            g.Name = args.Name;
            g.Description = args.Description;
            g.GroupCategoryId = args.GroupCategoryId;
            g.GroupLeader = args.GroupLeader;

            unitOfWork.Commit();
        }

        public IQueryable<EF.Group> Find(EF.Group args)
        {
            return unitOfWork.GroupRepository.Entities
                .Include(x => x.GroupCategory)
                .Include(x => x.GroupMember).ThenInclude(x => x.Member)
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.GroupLeader == (args.GroupLeader == null || args.GroupLeader == 0 ? x.GroupLeader : args.GroupLeader)
                && x.GroupCategoryId == (args.GroupCategoryId == null || args.GroupCategoryId == 0 ? x.GroupCategoryId : args.GroupCategoryId)
                && x.GroupCategory.CommunityId == args.GroupCategory.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.Group> Get(EF.Group args)
        {
            return await unitOfWork.GroupRepository.Entities
                .Include(x => x.GroupMember)
                .Include(x => x.GroupCategory)
                .Where(x => x.Id == args.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the list of attendance from events attended by any member of the group
        /// </summary>
        /// <param name="id">Group Id</param>
        /// <returns>List of attendance</returns>
        public IQueryable<EF.Attendance> Activities(int id)
        {
            return unitOfWork.AttendanceRepository.Entities
                .Where(x => x.Member.GroupMember.Any(y => y.GroupId == id));
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await unitOfWork.GroupRepository.Entities.Where(x => ids.Contains(x.Id) && x.GroupCategory.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
