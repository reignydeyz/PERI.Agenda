using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Role : IRole
    {
        private readonly IUnitOfWork _unitOfWork;

        public Role(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EF.Role>> GetAll()
        {
            return await _unitOfWork.RoleRepository.Entities.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
