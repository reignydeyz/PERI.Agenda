using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class EventCategoryReport
    {
        private readonly UnitOfWork unitOfWork;

        public EventCategoryReport(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<int> Add(EF.EventCategoryReport args)
        {
            await unitOfWork.EventCategoryReportRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

            return args.EventCategoryId;
        }

        public async Task Delete(EF.EventCategoryReport args)
        {
            var a = await unitOfWork.EventCategoryReportRepository.Entities.FirstAsync(x => x.ReportId == args.ReportId && x.EventCategoryId == args.EventCategoryId);
            unitOfWork.EventCategoryReportRepository.Remove(a);
            unitOfWork.Commit();
        }
    }
}
