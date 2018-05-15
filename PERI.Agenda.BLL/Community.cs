﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class Community : ICommunity
    {
        EF.AARSContext context;

        public Community(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task<IEnumerable<EF.Community>> Get()
        {
            return await context.Community.OrderBy(x => x.Name).ToListAsync();
        }
    }
}