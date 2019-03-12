using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface IFirstTimer
    {
        Task ValidateThenAdd(EF.FirstTimer args);
    }
}
