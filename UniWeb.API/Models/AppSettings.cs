using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Models
{
    public class AppSettings
    {
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
        public string RefreshToken_ExpireTime { get; set; }

        // send grind
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
    }
}
