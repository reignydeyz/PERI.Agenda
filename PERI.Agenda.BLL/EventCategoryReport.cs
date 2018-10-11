using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class EventCategoryReport
    {
        private readonly IUnitOfWork unitOfWork;

        public EventCategoryReport(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task AddRange(IEnumerable<EF.EventCategoryReport> args)
        {
            await unitOfWork.EventCategoryReportRepository.AddRangeAsync(args);
            await unitOfWork.CommitAsync();
        }

        public async Task Update(IEnumerable<EF.EventCategoryReport> args)
        {
            var res = await unitOfWork.EventCategoryReportRepository.Entities.Where(x => x.ReportId == args.First().ReportId).ToListAsync();

            unitOfWork.EventCategoryReportRepository.RemoveRange(res);

            await AddRange(args);
        }

        public async Task Delete(EF.EventCategoryReport args)
        {
            var a = await unitOfWork.EventCategoryReportRepository.Entities.FirstAsync(x => x.ReportId == args.ReportId && x.EventCategoryId == args.EventCategoryId);
            unitOfWork.EventCategoryReportRepository.Remove(a);
            unitOfWork.Commit();
        }
    }
}
