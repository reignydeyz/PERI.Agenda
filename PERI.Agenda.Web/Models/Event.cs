using System;
using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public int? LocationId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public int? EventCategoryId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [BLL.NoWhiteSpace]
        public string Description { get; set; }

        [Required]
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public int? RecurrenceId { get; set; }
        public bool? AllowRegistration { get; set; }
        public bool? IsExclusive { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
