using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class ReportTemplate
    {
        public int ReportId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Name { get; set; }

        public int? CommunityId { get; set; }
    }
}
