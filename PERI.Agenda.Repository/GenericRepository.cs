using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly EF.AARSContext _dbContext;
        private DbSet<T> _dbSet => _dbContext.Set<T>();
        public IQueryable<T> Entities => _dbSet;
        public GenericRepository(EF.AARSContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
    }
}
