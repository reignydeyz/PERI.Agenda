using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Location
    {
        private readonly IUnitOfWork unitOfWork;

        public Location(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Location args)
        {
            await unitOfWork.LocationRepository.AddAsync(args);
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
            unitOfWork.LocationRepository.RemoveRange(unitOfWork.LocationRepository.Entities.Where(x => ids.Contains(x.Id)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Location args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Location args)
        {
            var l = await unitOfWork.LocationRepository.Entities.FirstAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);

            l.Name = args.Name;
            l.Address = args.Address;
            l.DateTimeModified = DateTime.Now;
            l.ModifiedBy = args.ModifiedBy;

            unitOfWork.Commit();
        }

        public IQueryable<EF.Location> Find(EF.Location args)
        {
            return unitOfWork.LocationRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.Location> Get(EF.Location args)
        {
            return await unitOfWork.LocationRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.CommunityId == args.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await unitOfWork.LocationRepository.Entities.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
