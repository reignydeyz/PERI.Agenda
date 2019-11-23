using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class GroupMember
    {
        public int Id { get; set; }
                
        public int? MemberId { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
