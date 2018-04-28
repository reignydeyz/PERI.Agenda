﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? EventSectionId { get; set; }

        [Required]
        public int? MemberId { get; set; }
        public DateTime? DateTimeLogged { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
