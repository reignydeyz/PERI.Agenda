﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Models.Member, EF.Member>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
