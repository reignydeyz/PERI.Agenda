using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace PERI.Agenda.Core
{
    public class Emailer
    {
        public string DisplayName { get; set; }
        public string DisplayEmail { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public bool UseSsl { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }

        public async Task SendEmail(string to, string subject, string message)
        {
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress(DisplayName, DisplayEmail));
            msg.To.Add(new MailboxAddress("", to));
            msg.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            msg.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, Convert.ToInt32(SmtpPort), Convert.ToBoolean(UseSsl));

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(SmtpUser, SmtpPassword);

                await client.SendAsync(msg);
                await client.DisconnectAsync(true);
            }
        }
    }
}
