﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public interface ICommunity
    {
        Task<IEnumerable<EF.Community>> Get();
    }
}
