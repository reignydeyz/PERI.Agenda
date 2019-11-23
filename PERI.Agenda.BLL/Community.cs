using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Community : ICommunity
    {
        private readonly IUnitOfWork unitOfWork;

        public Community(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IEnumerable<EF.Community>> Get()
        {
            return await unitOfWork.CommunityRepository.Entities.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
