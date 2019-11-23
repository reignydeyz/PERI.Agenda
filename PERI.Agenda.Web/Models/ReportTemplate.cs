using System.ComponentModel.DataAnnotations;

namespace PERI.Agenda.Web.Models
{
    public class ReportTemplate
    {
        public int ReportId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Name { get; set; }

        public int? CommunityId { get; set; }
    }
}
