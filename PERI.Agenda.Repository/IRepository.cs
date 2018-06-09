using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Add(T entity);
        Task AddAsync(T entity);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
