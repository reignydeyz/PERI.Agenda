﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class SignUp
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [MaxLength(1)]
        public string MiddleInitial { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        public string LastName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [BLL.NoWhiteSpace]
        [MaxLength(50)]
        public string NickName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [BLL.NoWhiteSpace]
        public string Address { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [BLL.NoWhiteSpace]
        [MinLength(5)]
        [MaxLength(13)]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(10)]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [BLL.NoWhiteSpace]
        public string Remarks { get; set; }

        [Range(1,2)]
        public int? CivilStatus { get; set; }

        [Range(3,4)]
        public int? Gender { get; set; }
        public int? InvitedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }

        [Required]
        public int? CommunityId { get; set; }
    }
}
