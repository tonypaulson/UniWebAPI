using System;

namespace UniWeb.API.Exceptions
{
    public class ResourceNotFoundException : ApplicationException
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
