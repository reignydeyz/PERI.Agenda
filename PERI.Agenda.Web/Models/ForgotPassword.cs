using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
