using UniWeb.API.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Helpers
{
    public static class ResponseExtentions
    {
        public static ResponseDto<string> HandleException(this ResponseDto<string> responseDto, string errorMessage)
        {
            return new ResponseDto<string>() { Status = 0, Message = errorMessage };
        }
    }
}
