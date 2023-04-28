using System;
using System.Collections.Generic;

namespace UniWeb.API.Exceptions
{
    public class InputValidationFailureException : ApplicationException
    {       
        public Dictionary<string, string> Errors { get; }

        public InputValidationFailureException(Dictionary<string, string> errors) : base()
        {
            Errors = errors;
        }

        public InputValidationFailureException(string message) : base(message)
        {
        }
    }
}
