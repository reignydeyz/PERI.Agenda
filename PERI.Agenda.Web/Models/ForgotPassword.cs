using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        [MinLength(10)]
        public string Email { get; set; }
    }
}
