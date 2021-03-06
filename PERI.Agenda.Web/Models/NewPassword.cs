﻿using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class NewPassword
    {
        public EF.EndUser EndUser { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "Passwords don't match.")]
        public string ReenterPassword { get; set; }
    }
}
