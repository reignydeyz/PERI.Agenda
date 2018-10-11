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
        private readonly IUnitOfWork unitOfWork;

        public Rsvp(IUnitOfWork _unitOfWork)
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

        public IQueryable<EF.Rsvp> Find(string name, int eventId, bool? isGoing)
        {
            return unitOfWork.RsvpRepository.Entities
                .Include(x => x.Member)
                .Where(x => x.EventId == eventId
                && x.IsGoing == (isGoing ?? x.IsGoing)
                && x.Member.Name.Contains(String.IsNullOrEmpty(name) ? "" : name)
                && x.Member.Attendance.Where(y => y.EventId == eventId).Count() == 0)
                .OrderBy(x => x.Member.Name);
        }
    }
}
