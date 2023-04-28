using System;
using UniWeb.API.Enums;

namespace UniWeb.API.Entities
{
    public class MailRecord
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public MailStatus Status { get; set; }
        public string Error { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}