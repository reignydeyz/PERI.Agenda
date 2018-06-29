using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Rsvp
    {
        private readonly UnitOfWork unitOfWork;

        public Rsvp(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task Add(EF.Rsvp args)
        {
            await unitOfWork.RsvpRepository.AddAsync(args);
            unitOfWork.Commit();
        }

        public async Task Delete(EF.Rsvp args)
        {
            unitOfWork.RsvpRepository.Remove(args);
            await unitOfWork.CommitAsync();
        }
    }
}
