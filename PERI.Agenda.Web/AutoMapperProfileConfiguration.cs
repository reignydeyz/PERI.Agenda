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
            CreateMap<Models.SignUp, EF.Member>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Models.Event, EF.Event>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Models.EventCategory, EF.EventCategory>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Models.Location, EF.Location>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Models.Attendance, EF.Attendance>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Models.Group, EF.Group>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Models.ReportTemplate, EF.Report>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Models.ActivityReport, BLL.GroupReport>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
