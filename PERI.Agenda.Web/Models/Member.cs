using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class Member
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
    }
}
