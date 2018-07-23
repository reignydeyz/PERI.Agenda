using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Role
    {
        private UnitOfWork _unitOfWork;

        public Role(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EF.Role>> GetAll()
        {
            return await _unitOfWork.RoleRepository.Entities.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
