using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string recipientEmail, string subject, string htmlMessage)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpClient))
            {
                client.Port = _emailSettings.Port;
                client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                client.EnableSsl = _emailSettings.EnableSsl;

                var message = new MailMessage(_emailSettings.Email, recipientEmail, subject, htmlMessage)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }
    }
}
