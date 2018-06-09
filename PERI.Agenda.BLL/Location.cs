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
        private readonly UnitOfWork unitOfWork;

        public Location(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
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

        public IQueryable<EF.Location> Find(EF.Location args)
        {
            return unitOfWork.LocationRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.Attendance)
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public Task<EF.Location> Get(EF.Location args)
        {
            throw new NotImplementedException();
        }
    }
}
