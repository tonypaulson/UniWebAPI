using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Helpers
{
    public interface IEmailHelper
    {
        bool SendEmail(string subject, string body, string fromAddress, List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string name = "", List<string> filePaths = null);
        bool SendEmail(string subject, string body, string fromAddress, string toAddresses);
    }
}
