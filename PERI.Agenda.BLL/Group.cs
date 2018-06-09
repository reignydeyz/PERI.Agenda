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

        public Task<int> Add(EF.Group args)
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

        public Task Delete(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Group args)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EF.Group> Find(EF.Group args)
        {
            return unitOfWork.GroupRepository.Entities
                .Include(x => x.GroupMember).ThenInclude(x => x.Member)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.Group> Get(EF.Group args)
        {
            return await unitOfWork.GroupRepository.Entities
                .Include(x => x.GroupCategory)
                .Where(x => x.Id == args.Id).FirstOrDefaultAsync();
        }
    }
}
