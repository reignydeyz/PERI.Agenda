using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Community
    {
        public Community()
        {
            Report = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public decimal? Utc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public int? MaxEntities { get; set; }
        public int? MaxUsers { get; set; }
        public DateTime? DateExpiration { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Report> Report { get; set; }
    }
}
