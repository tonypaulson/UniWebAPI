using UniWeb.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UniWeb.API.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly SMTPSettings _settings;
        private readonly ILogger<EmailHelper> _logger;
        public EmailHelper(SMTPSettings settings, ILogger<EmailHelper> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public bool SendEmail(string subject, string body, string fromAddress, List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string name = "", List<string> filePaths = null)
        {
            try
            {
                var credentials = new NetworkCredential(_settings.UserName, _settings.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(_settings.FromMail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                toAddresses.ForEach(e =>
                {
                    mail.To.Add(e);
                });

                ccAddresses.ForEach(e =>
                {
                    mail.CC.Add(e);
                });

                bccAddresses.ForEach(e =>
                {
                    mail.Bcc.Add(e);
                });

                var smtpClient = new SmtpClient()
                {
                    Port = _settings.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _settings.Server,
                    EnableSsl = _settings.EnableSSL,
                    Credentials = credentials
                };
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public bool SendEmail(string subject, string body, string fromAddress, string toAddresses)
        {
            try
            {
                var credentials = new NetworkCredential(_settings.UserName, _settings.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(_settings.FromMail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mail.To.Add(toAddresses);

                var smtpClient = new SmtpClient()
                {
                    Port = _settings.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _settings.Server,
                    EnableSsl = _settings.EnableSSL,
                    Credentials = credentials
                };
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
