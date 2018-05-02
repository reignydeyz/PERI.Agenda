using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Core
{
    public class SmtpClient
    {
        public string DisplayName { get; set; }
        public string DisplayEmail { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public bool UseSsl { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
