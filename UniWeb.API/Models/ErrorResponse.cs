using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Models
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, Dictionary<string, string> validationErrors = null)
        {
            Message = message;
            ValidationErrors = validationErrors;
        }

        public string Message { get; }

        public Dictionary<string, string> ValidationErrors { get; }
    }
}
