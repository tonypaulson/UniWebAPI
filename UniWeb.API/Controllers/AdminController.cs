using System;
using System.Collections.Generic;
using System.Net;
using UniWeb.API.DTO;
using UniWeb.API.Exceptions;
using UniWeb.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Amazon.Runtime.Internal;
using UniWeb.API.Helpers;
using UniWeb.API.Entities;

namespace UniWeb.API.Controllers
{
    [Route("uniweb/api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private ILogger<AdminController> _logger;
       
        private readonly IAdminService _adminService;
        private readonly ISharedResource _sharedResource;

        public AdminController(IAdminService adminService,
            ISharedResource sharedResource,
      
        ILogger<AdminController> logger)
        {
            _logger = logger;
            _adminService = adminService;
            _sharedResource = sharedResource;
        }

        [HttpPost]
        public IActionResult Post(Admin admin)
        {
            try
            {
                if (string.IsNullOrEmpty(admin.FirstName))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(admin.LastName))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(admin.Email))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(admin.Password))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(admin.MobileNumber))
                {
                    return StatusCode(400);
                }

                var Admin = _adminService.Create(admin);
                //ResponseDto<Admin> response = new ResponseDto<Admin>
                //{
                //    Status = Enums.ResponseStatus.Success,
                //    Message = "Your account has been created succesfully",
                //    Data = Admin
                //};

                //return Ok(response);
                return this.GetOKResponse<Admin>(null, _sharedResource.SaveSuccess);
            }
            catch (UniWebBusinessException ex)
            {
                _logger.LogError(ex, $"Error while trying to create a tenant");
                return this.GetMessageResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user.");
                return StatusCode(500);
            }
        }

        [HttpPost("authadmin")]
        public async Task<IActionResult> Authadmin([FromBody] TokenRequestDto tokenRequestDto)
        {
            try
            {
                if(tokenRequestDto == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }

                switch (tokenRequestDto.GrantType)
                {
                    case "password":
                        var response = await _adminService.GenerateNewTokenadmin(tokenRequestDto);
                        if (response == null)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDto<string>().HandleException("Incorrect username/password"));
                        }

                        return StatusCode((int)HttpStatusCode.OK, response);

                    case "refresh_token":
                        var refreshTokenResponse = await _adminService.RefreshToken(tokenRequestDto);
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

        [HttpPost("ResetpasswordAdmin")]
        public IActionResult post([FromBody] PasswordResetDto passwordResetDto)
        {
            try
            {
                if (string.IsNullOrEmpty(passwordResetDto.Password))
                {
                    return StatusCode(400);
                }

                if (string.IsNullOrEmpty(passwordResetDto.ConfirmPassword))
                {
                    return StatusCode(400);
                }

                if (passwordResetDto.ConfirmPassword != passwordResetDto.Password)
                {
                    return StatusCode(400);
                }

                var user = _adminService.ResetPassword(passwordResetDto);
                return Ok(user);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating password");
                return StatusCode(500);
            }
        }

        [HttpPost("checkpassword/{password}")]
        public IActionResult CheckPassword(string password)
        {
            try
            {
                var userId = HttpContext.Request.GetUserId();
                ResponseDto<bool> response = new ResponseDto<bool>
                {

                    Status = Enums.ResponseStatus.Success,
                    Data = _adminService.IsPasswordEqual(password, userId)

                };
                return StatusCode((int)HttpStatusCode.OK,response);  
            }
            catch(UniWebBusinessException ex)
            {
                _logger.LogError(ex,"$ Error while checking password info.");
                return this.GetMessageResponse(ex.Message);

            }
            catch (Exception ex)
            {
                return this.GetErrorResponse(_sharedResource.UnknownError);
            }
        }

        
    }
}