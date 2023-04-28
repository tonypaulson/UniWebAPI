using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Exceptions
{
    public class UniWebBusinessException : Exception
    {
        public UniWebBusinessException(string message) : base(message)
        {

        }
    }
}
