using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace PERI.Agenda.Core
{
    public class Emailer
    {
        public string SendGridApiKey { get; set; }

        public async Task<Response> SendEmail(string to, string subject, string message)
        {
            var client = new SendGridClient(SendGridApiKey);
            var from = new EmailAddress("no-reply@agenda.com", "Agenda Admin");
            var to1 = new EmailAddress(to, "Member");
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to1, subject, string.Empty, htmlContent);
            var res = await client.SendEmailAsync(msg);
            return res;
        }
    }
}
