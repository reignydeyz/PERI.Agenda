using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class Report
    {
        public Report()
        {
            EventCategoryReport = new HashSet<EventCategoryReport>();
        }

        public int ReportId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int CommunityId { get; set; }

        public Community Community { get; set; }
        public ICollection<EventCategoryReport> EventCategoryReport { get; set; }
    }
}
