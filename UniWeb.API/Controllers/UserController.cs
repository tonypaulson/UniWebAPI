using System;
using System.Net;
using System.Threading.Tasks;
using UniWeb.API.DTO;
using UniWeb.API.Exceptions;
using UniWeb.API.Helpers;
using UniWeb.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace UniWeb.API.Controllers
{
    [Route("uniweb/api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserService _service = null;
        private IConfiguration _configuration;
        ILogger<UsersController> _logger = null;
        private readonly ISharedResource _sharedResource;

        public UsersController(UserService service,
        IConfiguration configuration,
        ILogger<UsersController> logger,
        ISharedResource sharedResource)
        {
            _service = service;
            _configuration = configuration;
            _logger = logger;
            _sharedResource = sharedResource;
        }

        /// <summary>
        /// Authenticate admin user login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authuser")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }

                switch (request.GrantType)
                {
                    case "password":
                        var response = await _service.GenerateNewToken(request);
                        if (response == null)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDto<string>().HandleException("Incorrect username/password"));
                        }

                        return StatusCode((int)HttpStatusCode.OK, response);

                    case "refresh_token":
                        var refreshTokenResponse = await _service.RefreshToken(request);
                        return StatusCode((int)HttpStatusCode.OK, refreshTokenResponse);

                    default:
                        return StatusCode((int)HttpStatusCode.Unauthorized);
                }
            }
            catch (UniWebBusinessException ex)
            {
                return this.GetMessageResponseForUnauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing request for Auth token generation.");
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserRegisterDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.FirstName))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(dto.LastName))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(dto.Email))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(dto.Password))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(dto.MobileNumber))
                {
                    return StatusCode(400);
                }

                var user = _service.CreateUser(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace.ToString());
                _logger.LogError(ex, "Error while creating user.");
                return StatusCode(500);
            }
        }

        [HttpPost("resetpassword")]
        public IActionResult Post([FromBody] PasswordResetDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Password))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(dto.ConfirmPassword))
                {
                    return StatusCode(400);
                }

                if (dto.ConfirmPassword != dto.Password)
                {
                    return StatusCode(400);
                }

                var user = _service.UpdateUserPassword(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Password.");
                return StatusCode(500);
            }
        }

        //[HttpGet]
        //public IActionResult Get(int userId)
        //{
        //    try
        //    {
        //        var user = _service.GetUserById(userId);
        //        ResponseDto<UserDto> response = new ResponseDto<UserDto>
        //        {
        //            Status = Enums.ResponseStatus.Success,
        //            Data = user
        //        };

        //        return Ok(response);
        //    }
        //    catch (UniWebBusinessException ex)
        //    {
        //        _logger.LogError(ex, $"Error while trying to getting user by user id - {userId}");
        //        return this.GetMessageResponse(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error while getting user by user id.");
        //        return StatusCode(500);
        //    }
        //}

        [HttpPost("forget")]
        public IActionResult Forget([FromBody] ForgetPasswordInputDto dto)
        {
            try
            {
                var password = _service.StartForgetPassword(dto.Email, dto.HostName);
                return Ok(password);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while trying to activate forgot password session.", ex);
                return this.GetErrorResponse(ex.Message);
            }
        }

        [HttpPost("forget/reset")]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                _service.ResetPassword(dto);
                ResponseDto<bool> response = new ResponseDto<bool>
                {
                    Data = true,
                    Status = Enums.ResponseStatus.Success                   
                };

                return Ok(response);
            }
            catch (UniWebBusinessException ex)
            {
                _logger.LogError(ex, $"Error while trying to reset password");
                return this.GetMessageResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while resetting password.", ex);
                return this.GetErrorResponse(ex.Message);
            }
        }


        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var results = _service.GetAllUsers();
                ResponseDto<List<UserRegisterDto>> response = new ResponseDto<List<UserRegisterDto>>
                {
                    Status = Enums.ResponseStatus.Success,
                    Data = results,
                };

                return StatusCode((int)HttpStatusCode.OK,response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while getting all users");
                return StatusCode(500);
            }
        }

    }
}