using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class Rsvp
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int MemberId { get; set; }
        public string Member { get; set; }
        public bool IsGoing { get; set; }
    }
}
