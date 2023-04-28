using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using UniWeb.API.DataContext;
using UniWeb.API.Entities;
using UniWeb.API.Enums;
using UniWeb.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UniWeb.API.Services
{
    public class EmailHostedService : IHostedService
    {
        private IServiceProvider _services;
        private SMTPSettings _smtpSettings;
        private ILogger<EmailHostedService> _logger;
        private readonly CancellationTokenSource _loopCancellationToken = new CancellationTokenSource();
        private Task _mailLoop = null;

        public EmailHostedService(IServiceProvider services, 
        SMTPSettings smtpSettings,
        ILogger<EmailHostedService> logger)
        {
            _services = services;
            _smtpSettings = smtpSettings;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var loopToken = _loopCancellationToken.Token;
            _mailLoop = Task.Run(() =>
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<EFDataContext>();
                    var credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
                    var smtpClient = new SmtpClient()
                    {
                        Port = _smtpSettings.Port,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = _smtpSettings.Server,
                        EnableSsl = _smtpSettings.EnableSSL,
                        Credentials = credentials
                    };

                    while (!loopToken.IsCancellationRequested)
                    {
                        MailRecord pendingMail = null;
                        try
                        {
                            pendingMail = context.MailRecords
                            .OrderBy(x => x.UpdatedAt)
                            .Where(x => x.Status == MailStatus.Failed || x.Status == MailStatus.Ready)
                            .FirstOrDefault();

                            if (null == pendingMail)
                            {
                                continue;
                            }

                            var currentTime = DateTime.Now;
                            var span = currentTime - pendingMail.CreatedAt;
                            if (span.TotalHours > 24)
                            {
                                pendingMail.Status = MailStatus.Purged;
                                pendingMail.UpdatedAt = DateTime.Now;
                                context.MailRecords.Update(pendingMail);
                                context.SaveChanges();
                             //   _logger.LogDebug($"Mail {pendingMail.Id} is purged.");
                            }
                            else
                            {
                                var message = new MailMessage();
                                message.From = new MailAddress(_smtpSettings.FromMail);
                                message.Subject = pendingMail.Subject;
                                message.Body = pendingMail.Body;
                                message.IsBodyHtml = true;
                                var recipients = pendingMail.To.Split(",", StringSplitOptions.RemoveEmptyEntries);
                                foreach(var recipient in recipients)
                                {
                                    message.To.Add(new MailAddress(recipient));
                                }
                                smtpClient.Send(message);
                                pendingMail.Status = MailStatus.Sent;
                                pendingMail.UpdatedAt = DateTime.Now;
                                context.MailRecords.Update(pendingMail);
                                context.SaveChanges();
                              //   _logger.LogDebug($"Mail {pendingMail.Id} is sent.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error while sending email.");
                            if (null == pendingMail)
                            {
                                continue;
                            }

                            pendingMail.Status = MailStatus.Failed;
                            pendingMail.UpdatedAt = DateTime.Now;
                            pendingMail.Error = ex.Message;
                            if (null != ex.InnerException)
                            {
                                pendingMail.Error += ". " + ex.InnerException.Message;
                            }
                            context.MailRecords.Update(pendingMail);
                            context.SaveChanges();
                        }
                        finally
                        {
                            var delay = Task.Delay(75000);
                            Task.WaitAny(delay);
                        }
                    }
                }
            });
            if (_mailLoop.IsCompleted)
            {
                return _mailLoop;
            }
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (null == _mailLoop)
            {
                return;
            }
            _loopCancellationToken.Cancel();
            await Task.WhenAny(_mailLoop);
        }
    }
}