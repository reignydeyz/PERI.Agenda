using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    interface ICommunity
    {
        Task<IEnumerable<EF.Community>> Get();
    }
}
