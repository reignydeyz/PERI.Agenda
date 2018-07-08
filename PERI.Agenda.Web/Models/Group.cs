using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int GroupCategoryId { get; set; }
        public string Name { get; set; }
        public string Leader { get; set; }
    }
}
