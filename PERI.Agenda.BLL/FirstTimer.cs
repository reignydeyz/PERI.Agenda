using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class FirstTimer
    {
        private readonly IUnitOfWork _unitOfWork;

        public FirstTimer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Validates if member is a first-timer
        /// </summary>
        /// <param name="args">Attendance</param>
        /// <returns></returns>
        public async Task ValidateThenAdd(EF.FirstTimer args)
        {
            var memberId = _unitOfWork.AttendanceRepository.Entities.First(x => x.Id == args.AttendanceId).MemberId;

            var countOfEventsAttented = _unitOfWork.EventRepository.Entities.Where(x => x.Attendance.Any(y => y.MemberId == memberId)).Count();

            if (countOfEventsAttented == 1)
            {
                await _unitOfWork.FirstTimerRepository.AddAsync(args);
                _unitOfWork.Commit();
            }
        }
    }
}
