using PERI.Agenda.Repository;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IUnitOfWork
    {
        IRepository<EF.Attendance> AttendanceRepository { get; }
        IRepository<EF.Community> CommunityRepository { get; }
        IRepository<EF.EndUser> EndUserRepository { get; }
        IRepository<EF.Event> EventRepository { get; }
        IRepository<EF.EventCategory> EventCategoryRepository { get; }
        IRepository<EF.Group> GroupRepository { get; }
        IRepository<EF.GroupCategory> GroupCategoryRepository { get; }
        IRepository<EF.GroupMember> GroupMemberRepository { get; }
        IRepository<EF.Location> LocationRepository { get; }
        IRepository<EF.LookUp> LookUpRepository { get; }
        IRepository<EF.Member> MemberRepository { get; }
        IRepository<EF.Registrant> RegistrantRepository { get; }
        IRepository<EF.Role> RoleRepository { get; }
        IRepository<EF.Rsvp> RsvpRepository { get; }
        IRepository<EF.EventCategoryReport> EventCategoryReportRepository { get; }
        IRepository<EF.Report> ReportRepository { get; }
        IRepository<EF.FirstTimer> FirstTimerRepository { get; }
        IRepository<EF.EventGroup> EventGroupRepository { get; }
        /// <summary>
        /// Commits all changes
        /// </summary>
        void Commit();

        Task CommitAsync();
        
        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}
