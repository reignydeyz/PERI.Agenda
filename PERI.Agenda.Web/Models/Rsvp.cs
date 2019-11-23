using System.ComponentModel.DataAnnotations;

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
