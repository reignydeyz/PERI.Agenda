using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Report : IReport
    {
        private readonly IUnitOfWork unitOfWork;

        public Report(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Report args)
        {
            args.DateModified = args.DateCreated;
            args.ModifiedBy = args.CreatedBy;

            await unitOfWork.ReportRepository.AddAsync(args);
            unitOfWork.Commit();
            return args.ReportId;
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.ReportRepository.RemoveRange(unitOfWork.ReportRepository.Entities.Where(x => ids.Contains(x.ReportId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Report args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Report args)
        {
            var ec = await unitOfWork.ReportRepository.Entities.FirstAsync(x => x.ReportId == args.ReportId
            && x.CommunityId == args.CommunityId);

            ec.Name = args.Name;
            ec.DateModified = DateTime.Now;
            ec.ModifiedBy = args.ModifiedBy;

            unitOfWork.Commit();
        }

        public IQueryable<EF.Report> Find(EF.Report args)
        {
            return unitOfWork.ReportRepository.Entities
                .Where(x => x.Name.Contains(args.Name ?? "")
                && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public Task<EF.Report> Get(EF.Report args)
        {
            throw new NotImplementedException();
        }

        public async Task<EF.Report> GetById(int id)
        {
            return await unitOfWork.ReportRepository.Entities
                .Include(x => x.EventCategoryReport)
                .FirstOrDefaultAsync(x => x.ReportId == id);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await unitOfWork.ReportRepository.Entities.Where(x => ids.Contains(x.ReportId) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
