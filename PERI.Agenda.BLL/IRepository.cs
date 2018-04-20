using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IRepository<T>
    {
        Task Activate(int[] ids);

        Task<int> Add(T args);

        Task Deactivate(int[] ids);

        Task Delete(int id);

        Task Delete(int[] ids);

        Task Delete(T args);

        Task Edit(T args);

        IQueryable<T> Find(T args);

        Task<T> Get(T args);
    }
}
