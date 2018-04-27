using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;
using System.ComponentModel.DataAnnotations;
using PERI.Agenda.EF;

namespace PERI.Agenda.Web.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(7)]
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Remarks { get; set; }
        public int? CivilStatus { get; set; }
        public int? Gender { get; set; }
        public int? InvitedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }        
        public int? CommunityId { get; set; }
    }
}
