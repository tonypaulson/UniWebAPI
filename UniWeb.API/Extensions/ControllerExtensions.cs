using UniWeb.API.Enums;
using UniWeb.API.DTO;
using UniWeb.API.Helpers;

namespace Microsoft.AspNetCore.Mvc {
    public static class ControllerExtensions 
    {
        public static IActionResult GetOKResponse<T>(this ControllerBase controller, 
        T data, string message = "") where T: class
        {
            var response = new ResponseDto<T>
            {
                Data = data,
                Message = message,
                Status = ResponseStatus.Success
            };
            return controller.StatusCode(200, response);
        }

        public static IActionResult GetErrorResponse(this ControllerBase controller, 
        string message, int statusCode = 500)
        {
            var response = new ResponseDto<object>
            {
                Data = null,
                Message = message,
                Status = ResponseStatus.Error
            };
            return controller.StatusCode(statusCode, response);
        }

        public static IActionResult GetMessageResponse(this ControllerBase controller, 
        string message)
        {
            var response = new ResponseDto<string>
            {
                Data = null,
                Message = message,
                Status = ResponseStatus.Error
            };
            return controller.StatusCode(200, response);
        }

        public static IActionResult GetDeleteSuccess(this ControllerBase controller, 
        ISharedResource sharedResource)
        {
            var response = new ResponseDto<object>
            {
                Data = null,
                Message = sharedResource.ServiceDeleteSuccess,
                Status = ResponseStatus.Success
            };
            return controller.StatusCode(200, response);
        }

        public static IActionResult GetMessageResponseForUnauthorized(this ControllerBase controller,
      string message)
        {
            var response = new ResponseDto<string>
            {
                Data = null,
                Message = message,
                Status = ResponseStatus.Error
            };
            return controller.StatusCode(500, response);
        }
    }
}