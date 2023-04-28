using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniWeb.API.DataContext;
using UniWeb.API.Entities;

namespace UniWeb.API.Services
{
    public class MailService
    {
        private EFDataContext _context;

        public MailService(EFDataContext context)
        {
            _context = context;
        }

        public void CreateMail(string subject, string template, 
        Dictionary<string, string> bindings, 
        params string[] recipients)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new InvalidOperationException("Subject is required");
            }

            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Template is required to construct the body.");
            }

            if ((null == recipients) || (0 == recipients.Length))
            {
                throw new InvalidOperationException("At least one recipient is required.");
            }

            foreach(var input in bindings)
            {
                var regex = new Regex($"\\{{{{\\s*{input.Key}\\s*\\}}}}", RegexOptions.IgnoreCase);
                template = regex.Replace(template, input.Value);
            }

            var mailRecord = new MailRecord()
            {
                Subject = subject,
                To = String.Join(',', recipients, 0, recipients.Length),
                Body = template,
                Status = Enums.MailStatus.Ready,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.MailRecords.Add(mailRecord);
            _context.SaveChanges();
        }
    }
}