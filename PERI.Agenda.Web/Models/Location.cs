using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Address { get; set; }
        public int? CommunityId { get; set; }
    }
}
