
using UniWeb.API.Exceptions;
using UniWeb.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Carewell.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            ErrorResponse res = null;
            var exceptionType = context.Exception.GetType();
            var exception = context.Exception;
            if (exceptionType == typeof(InputValidationFailureException))
            {
                var inputValidationFailureException = (exception as InputValidationFailureException);
                status = HttpStatusCode.BadRequest;
                res = new ErrorResponse(inputValidationFailureException.Message, inputValidationFailureException.Errors);
                logger.LogError(exception, exception.Message);
            }
            else if (exceptionType == typeof(ResourceNotFoundException))
            {
                status = HttpStatusCode.NotFound;
                res = new ErrorResponse(exception.Message);
                logger.LogError(exception, exception.Message);
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Forbidden;
                res = new ErrorResponse(exception.Message);
                logger.LogError(exception, exception.Message);
            }
            else if (exceptionType == typeof(UniWebBusinessException))
            {
                status = HttpStatusCode.NoContent;
                res = new ErrorResponse(exception.Message);
                logger.LogError(exception, exception.Message);
            }
            else
            {
                logger.LogCritical(exception, exception.Message);
                status = HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var errorResponse = (res == null) ? string.Empty : JsonConvert.SerializeObject(res);
            response.WriteAsync(errorResponse);
        }
    }
}
