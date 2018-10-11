using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Registrant
    {
        private readonly IUnitOfWork unitOfWork;

        public Registrant(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task Add(IEnumerable<EF.Registrant> args)
        {
            await unitOfWork.RegistrantRepository.AddRangeAsync(args);
            await unitOfWork.CommitAsync();
        }
    }
}
