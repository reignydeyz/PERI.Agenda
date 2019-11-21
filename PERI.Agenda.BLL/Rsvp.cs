using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Rsvp : IRsvp
    {
        private readonly IUnitOfWork unitOfWork;

        public Rsvp(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<int> Add(EF.Rsvp args)
        {
            args.DateCreated = DateTime.Now;
            args.DateModified = args.DateCreated;
            await unitOfWork.RsvpRepository.AddAsync(args);
            unitOfWork.Commit();

            return args.EventId;
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
                && x.Member.Attendance.Count(y => y.EventId == eventId) == 0)
                .OrderBy(x => x.Member.Name);
        }

        public Task Activate(int[] ids)
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

        public Task Edit(EF.Rsvp args)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EF.Rsvp> Find(EF.Rsvp args)
        {
            throw new NotImplementedException();
        }
    }
}
