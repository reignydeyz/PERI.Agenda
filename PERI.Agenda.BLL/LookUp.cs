﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PERI.Agenda.BLL
{
    public class LookUp : ILookUp
    {
        private readonly IUnitOfWork unitOfWork;
        public LookUp(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IEnumerable<EF.LookUp>> GetByGroup(string group)
        {
            return await unitOfWork.LookUpRepository.Entities.Where(x => x.Group == group).OrderBy(x => x.Weight).ToListAsync();
        }
    }
}
