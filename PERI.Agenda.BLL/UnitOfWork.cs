using Microsoft.EntityFrameworkCore;
using PERI.Agenda.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EF.AARSContext _dbContext;
        #region Repositories
        public IRepository<EF.Attendance> AttendanceRepository => new GenericRepository<EF.Attendance>(_dbContext);
        public IRepository<EF.Community> CommunityRepository => new GenericRepository<EF.Community>(_dbContext);
        public IRepository<EF.EndUser> EndUserRepository { get; set;}
        public IRepository<EF.Event> EventRepository { get; set; }
        public IRepository<EF.EventCategory> EventCategoryRepository => new GenericRepository<EF.EventCategory>(_dbContext);
        public IRepository<EF.Group> GroupRepository => new GenericRepository<EF.Group>(_dbContext);
        public IRepository<EF.GroupCategory> GroupCategoryRepository => new GenericRepository<EF.GroupCategory>(_dbContext);
        public IRepository<EF.GroupMember> GroupMemberRepository => new GenericRepository<EF.GroupMember>(_dbContext);
        public IRepository<EF.Location> LocationRepository => new GenericRepository<EF.Location>(_dbContext);
        public IRepository<EF.LookUp> LookUpRepository => new GenericRepository<EF.LookUp>(_dbContext);
        public IRepository<EF.Member> MemberRepository { get; set; }
        public IRepository<EF.Registrant> RegistrantRepository => new GenericRepository<EF.Registrant>(_dbContext);
        public IRepository<EF.Role> RoleRepository { get; set; }
        public IRepository<EF.Rsvp> RsvpRepository => new GenericRepository<EF.Rsvp>(_dbContext);
        public IRepository<EF.Report> ReportRepository => new GenericRepository<EF.Report>(_dbContext);
        public IRepository<EF.EventCategoryReport> EventCategoryReportRepository => new GenericRepository<EF.EventCategoryReport>(_dbContext);
        public IRepository<EF.FirstTimer> FirstTimerRepository => new GenericRepository<EF.FirstTimer>(_dbContext);
        #endregion
        public UnitOfWork(EF.AARSContext dbContext)
        {
            _dbContext = dbContext;

            // I have to make a setter for mocking
            this.MemberRepository = this.MemberRepository ?? new GenericRepository<EF.Member>(_dbContext);
            this.EndUserRepository = this.EndUserRepository ?? new GenericRepository<EF.EndUser>(_dbContext);
            this.RoleRepository = this.RoleRepository ?? new GenericRepository<EF.Role>(_dbContext);
            this.EventRepository = this.EventRepository ?? new GenericRepository<EF.Event>(_dbContext);
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
