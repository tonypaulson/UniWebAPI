using System;
using UniWeb.API.DataContext;
using UniWeb.API.DTO;
using UniWeb.API.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniWeb.API.Exceptions;
using System.Collections.Generic;
using UniWeb.API.Helpers;
using UniWeb.API.DTO.AccountDto;
using UniWeb.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Amazon.S3.Model;
using UniWeb.API.DataServices;
using System.Security.Cryptography;

namespace UniWeb.API.Services
{
    public class AdminService:IAdminService
    {
        public static readonly string CreateError = "Error while trying to creating Admin.";
        private EFDataContext _context;
        private PasswordHaser _hasher;
        private ILogger<AdminService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IAdminDataServices _adminDataServices;
        private readonly PlaceService _placeService;
        private readonly MailService _mailService;
        private readonly ISharedResource _sharedResource;
        private readonly ConfigurationService _configurationService;

        public AdminService(EFDataContext context,
            PlaceService placeService,
            MailService mailService,
            ISharedResource sharedResource,
            ConfigurationService configurationService,
            PasswordHaser hasher,
            ILogger<AdminService> logger,
            AppSettings appSettings,
            IAdminDataServices adminDataServices)
        {
            _context = context;
            _hasher = hasher;
            _logger = logger;
            _appSettings = appSettings;
            _adminDataServices = adminDataServices;
            _placeService = placeService;
            _mailService = mailService;
            _sharedResource = sharedResource;
            _configurationService = configurationService;
        }

        public Admin Create(Admin admin)
        {
            try
            {
                if (admin != null)
                {
                    if (admin.Id == 0)
                    {
                        if (_context.Admin.Any(x => x.Email == admin.Email))
                        {
                            throw new UniWebBusinessException("User already exists.");
                        }

                        var newadmin = new Admin()
                        {
                            FirstName = admin.FirstName,
                            LastName = admin.LastName,
                            Email = admin.Email,
                            MobileNumber = admin.MobileNumber,
                            Password = _hasher.HashPassword(admin.Password),
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow

                        };
                        _context.Admin.Add(newadmin);
                        _context.SaveChanges();
                        return AdminDto.FromAdmin(newadmin);
                    }
                    else
                    {
                        admin.Password = _hasher.HashPassword(admin.Password);
                        admin.CreatedDate = DateTime.UtcNow;
                        admin.UpdatedDate = DateTime.UtcNow;
                        _context.Admin.Update(admin);
                        _context.SaveChanges();
                    }
                }
                return null;
               
            }
            catch (UniWebBusinessException ex)
            {
                _logger.LogError(ex, $"Error while trying to create a Admin");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CreateError);
                throw new InvalidOperationException(CreateError);
            }
        }

        public bool ResetPassword(PasswordResetDto dto)
        {
            try
            {

                var admin = _context.Admin.Find(dto.UserId);
                if(admin == null)
                {
                    throw new UniWebBusinessException("Invalid user");
                }
                admin.Password = _hasher.HashPassword(dto.Password);
                _context.Admin.Update(admin);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<TokenResponseDto> GenerateNewTokenadmin(TokenRequestDto tokenRequestDto)
        {
            try
            {
                var admin = Userlogin(tokenRequestDto.UserName, tokenRequestDto.Password);
                //var tenant = _context.Tenants.SingleOrDefault(x => x.Id == user.TenantId);
                //if (null == tenant)
                //{
                //    throw new UniWebBusinessException("Invalid user. Tenant is incorrect.");
                //}

                if (admin != null)
                {
                    var newRefreshToken = CreateRefreshToken(admin.Id);
                    await RemoveAllOldAndAddNewToken(admin.Id, newRefreshToken);
                    var encodedToken = CreateAccessTokenAdmin(admin, admin.Id, newRefreshToken);
                    return encodedToken;
                }

                return null;



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task <TokenResponseDto> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            try
            {

                var _user = ValidateTokenAndUser(tokenRequestDto.RefreshToken.ToString());
                var rtNew = CreateRefreshToken(_user.Id);
                await RemoveAllOldAndAddNewToken(_user.Id, rtNew);
                var encodedToken = CreateAccessTokenAdmin(_user, _user.Id, rtNew);
                return encodedToken;

            }
            catch(Exception  ex)
            {
                throw ex;
            }
        }


        private Admin ValidateTokenAndUser(string refreshToken)
        {
            try
            {
                var rt = _context.AdminToken.FirstOrDefault(t => t.Value == refreshToken);
                if (rt == null)
                {
                    // refresh token not found or invalid (or invalid client id)
                    throw new UnauthorizedAccessException();
                }

                if (rt.ExpiryTime < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException();
                }

                var _user = _context.Admin.FirstOrDefault(x => x.Id == rt.AdminId);
                if (_user == null)
                {
                    // user id not found or invalid
                    throw new UnauthorizedAccessException();
                }

                return _user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task RemoveAllOldAndAddNewToken(int adminId, AdminToken refreshToken)
        {
            try
            {
                var oldTokens = _context.AdminToken.Where(x => x.AdminId == adminId);
                if (null != oldTokens)
                {
                    foreach (var oldToken in oldTokens)
                    {
                        _context.AdminToken.Remove(oldToken);
                    }

                    _context.AdminToken.Add(refreshToken);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdminToken CreateRefreshToken(int adminId)
        {
            double tokenExpiryTime = Convert.ToDouble(_appSettings.RefreshToken_ExpireTime);
            return new AdminToken()
            {
                AdminId = adminId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
            };
        }

        public Admin Userlogin(string email,string password)
        {
            try
            {
                var user = _context.Admin.SingleOrDefault(s => s.Email == email && s.Password == _hasher.HashPassword(password));

                if (user == null)
                {
                    throw new UniWebBusinessException("Invalid UserName or password");
                }

                return user;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TokenResponseDto CreateAccessTokenAdmin(Admin admin,int adminId, AdminToken newRefreshtoken)
        {
            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim("UserId", admin.Id.ToString()),
                    new Claim("LoggedOn", DateTime.Now.ToString())

                }),

                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var newtoken = tokenhandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenhandler.WriteToken(newtoken);


            return new TokenResponseDto
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                Refresh_Token = newRefreshtoken.Value,
                Username = admin.Email,
                userId = admin.Id
            };


        }

        //public bool IsExist(int tenantId)
        //{
        //    if (!_context.Tenants.Any(x => x.Id == tenantId))
        //    {
        //        return false;
        //    }
        //    return true;
        //}


        public string HashPassword(string password)
        {
            var hasher = SHA256.Create();
            var hash = hasher.ComputeHash(Encoding.Default.GetBytes(password));
            return Convert.ToBase64String(hash);    
        }
        
        bool IAdminService.IsPasswordEqual(string password,int userId)
        {
            try
            {
                var response = _adminDataServices.GetAdminDetails(userId);
                if (response.Password.Equals(HashPassword(password)))
                {
                    return true;
                }
                else
                {
                    return false;   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
    }
}