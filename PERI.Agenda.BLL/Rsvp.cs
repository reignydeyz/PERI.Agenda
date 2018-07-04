using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            args.DateCreated = DateTime.Now;
            args.DateModified = args.DateCreated;
            await unitOfWork.RsvpRepository.AddAsync(args);
            unitOfWork.Commit();
        }

        public async Task Delete(EF.Rsvp args)
        {
            unitOfWork.RsvpRepository.Remove(args);
            await unitOfWork.CommitAsync();
        }

        public async Task Update(EF.Rsvp args)
        {
            var r = unitOfWork.RsvpRepository.Entities.First(x => x.EventId == args.EventId && x.MemberId == args.MemberId);
            r.IsGoing = args.IsGoing;
            r.DateModified = DateTime.Now;
            await unitOfWork.CommitAsync();
        }

        public async Task<EF.Rsvp> Get(EF.Rsvp args)
        {
            return await unitOfWork.RsvpRepository.Entities.FirstOrDefaultAsync(x => x.EventId == args.EventId && x.MemberId == args.MemberId);
        }
    }
}
