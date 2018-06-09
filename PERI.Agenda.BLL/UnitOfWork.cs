using Microsoft.EntityFrameworkCore;
using PERI.Agenda.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class UnitOfWork
    {
        private readonly EF.AARSContext _dbContext;
        #region Repositories
        public IRepository<EF.Attendance> AttendanceRepository => new GenericRepository<EF.Attendance>(_dbContext);
        public IRepository<EF.Community> CommunityRepository => new GenericRepository<EF.Community>(_dbContext);
        public IRepository<EF.EndUser> EndUserRepository => new GenericRepository<EF.EndUser>(_dbContext);
        public IRepository<EF.Event> EventRepository => new GenericRepository<EF.Event>(_dbContext);
        public IRepository<EF.EventCategory> EventCategoryRepository => new GenericRepository<EF.EventCategory>(_dbContext);
        public IRepository<EF.Group> GroupRepository => new GenericRepository<EF.Group>(_dbContext);
        public IRepository<EF.GroupCategory> GroupCategoryRepository => new GenericRepository<EF.GroupCategory>(_dbContext);
        public IRepository<EF.GroupMember> GroupMemberRepository => new GenericRepository<EF.GroupMember>(_dbContext);
        public IRepository<EF.Location> LocationRepository => new GenericRepository<EF.Location>(_dbContext);
        public IRepository<EF.LookUp> LookUpRepository => new GenericRepository<EF.LookUp>(_dbContext);
        public IRepository<EF.Member> MemberRepository => new GenericRepository<EF.Member>(_dbContext);
        public IRepository<EF.Registrant> RegistrantRepository => new GenericRepository<EF.Registrant>(_dbContext);
        public IRepository<EF.Role> RoleRepository => new GenericRepository<EF.Role>(_dbContext);
        #endregion
        public UnitOfWork(EF.AARSContext dbContext)
        {
            _dbContext = dbContext;
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
