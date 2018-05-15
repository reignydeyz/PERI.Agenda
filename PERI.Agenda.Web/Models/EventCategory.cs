using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class EventCategory
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public bool AllowMemberToLogOnMultipleEvents { get; set; }
        public bool AllowSectionOverlapping { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? CommunityId { get; set; }
    }
}
