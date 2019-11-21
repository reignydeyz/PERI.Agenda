using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class LookUp : ILookUp
    {
        private readonly IUnitOfWork unitOfWork;
        public LookUp(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IEnumerable<EF.LookUp>> GetByGroup(string group)
        {
            return await unitOfWork.LookUpRepository.Entities.Where(x => x.Group == group).OrderBy(x => x.Weight).ToListAsync();
        }
    }
}
