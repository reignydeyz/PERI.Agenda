using System;
using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class ActivityReport
    {
        [Required]
        public int ReportId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}
