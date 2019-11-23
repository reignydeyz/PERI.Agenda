using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class ChangePassword
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ReEnterNewPassword { get; set; }
    }
}
