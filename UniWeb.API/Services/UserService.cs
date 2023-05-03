using UniWeb.API.DataContext;
using UniWeb.API.DTO;
using UniWeb.API.DTO.AccountDto;
using UniWeb.API.Entities;
using UniWeb.API.Exceptions;
using UniWeb.API.Helpers;
using UniWeb.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniWeb.API.DataServices;

namespace UniWeb.API.Services
{
    public class UserService
    {
        EFDataContext _context = null!;
        private readonly AppSettings _appSettings;
        private readonly IUserDataService _userDataService;
        private readonly PasswordHaser _hasher;
        private readonly ISharedResource _sharedResource;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailHelper;
        private readonly MailService _mailService;
        private readonly ILogger<UserService> _logger;
        

        // public object RoleIdKeyValuePair { get; private set; }

        public UserService(EFDataContext context,
           
            AppSettings appsettings,
            IUserDataService userDataService,
            PasswordHaser hasher,
            ISharedResource sharedResource,
            IConfiguration configuration,
            IEmailHelper emailHelper,
            MailService mailService,
            ILogger<UserService> logger
            )
        {
            
            _context = context;
            _appSettings = appsettings;
            _userDataService = userDataService;
            _hasher = hasher;
            _sharedResource = sharedResource;
            _configuration = configuration;
            _emailHelper = emailHelper;
            _mailService = mailService;
            _logger = logger;
          
        }

        public bool Exists(string email)
        {
            return _context
                .Users
                .Any(x => x.Email == email);
        }

        public UserRegisterDto CreateUser(UserRegisterDto dto)
        {
            if (this.Exists(dto.Email))
            {
                throw new InvalidOperationException("User already exists.");
            }
            var entry = _context.Users.Add(new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                Gender = dto.Gender,
                Password = _hasher.HashPassword(dto.Password),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });

            _context.SaveChanges();
            return UserRegisterDto.FromUser(entry.Entity);
        }

        public async Task<TokenResponseDto> GenerateNewToken(TokenRequestDto tokenRequestDto)
        {
            try
            {
                var admin = UserLogin(tokenRequestDto.UserName, tokenRequestDto.Password);
                //var tenant = _context.Tenants.SingleOrDefault(x => x.Id == user.TenantId);
                //if (null == tenant)
                //{
                //    throw new UniWebBusinessException("Invalid user. Tenant is incorrect.");
                //}

                if (admin != null)
                {
                    var newRefreshToken = CreateRefreshToken(admin.Id);
                    await RemoveAllOldAndAddNewToken(admin.Id, newRefreshToken);
                    var encodedToken = CreateAccessToken(admin,newRefreshToken);
                    return encodedToken;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TokenResponseDto> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            try
            {
                var _user = ValidateTokenAndUser(tokenRequestDto.RefreshToken.ToString());
                var rtNew = CreateRefreshToken(_user.Id);
                await RemoveAllOldAndAddNewToken(_user.Id,rtNew);
                var encodedToken = CreateAccessToken(_user, rtNew);
                return encodedToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User UserLogin(string email, string password)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(s => s.Email == email && s.Password == _hasher.HashPassword(password));

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

        //public UserDto GetUserById(int userId)
        //{
        //    try
        //    {
        //        var user = _context.Users.FirstOrDefault(s => s.Id == userId);
        //        if (user == null)
        //        {
        //            throw new UniWebBusinessException("Invalid userid");
        //        }

        //        return UserDto.FromUser(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool UpdateUserPassword(PasswordResetDto dto)
        {
            try
            {
                var user = _context.Users.Find(dto.UserId);
                if (user == null)
                {
                    throw new UniWebBusinessException("Invalid user");
                }

                user.Password = _hasher.HashPassword(dto.Password);
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TemporaryPasswordDto StartForgetPassword(string email, string host)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            if (null == user)
            {
                throw new UniWebBusinessException("No such user exists.");
            }
            var temporaryPassword = _context.TemporaryPasswords
            .SingleOrDefault(x => x.UserId == user.Id);
            if (null != temporaryPassword)
            {
                _context.TemporaryPasswords.Remove(temporaryPassword);
                _context.SaveChanges();
            }

            temporaryPassword = new TemporaryPassword()
            {
                User = user,
                Token = (Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "")),
                Password = GenerateTemporaryPassword(),
                CreatedAt = DateTime.Now
            };

            var entry = _context.TemporaryPasswords.Add(temporaryPassword);
            _context.SaveChanges();

            var entity = entry.Entity;
            var result = new TemporaryPasswordDto()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Email = email,
                Token = entity.Token
            };
            var linkUrl = _configuration.GetValue<string>("ForgetPasswordUrl");
            if (string.IsNullOrEmpty(linkUrl))
            {
                throw new UniWebBusinessException("Forget URL link is not configured.");
            }

            linkUrl = linkUrl.ToLower();
            linkUrl = linkUrl.Replace("{hostname}", host);
            linkUrl = linkUrl.Replace("{token}", result.Token);

            // var mainBody = _sharedResource.ForgetPasswordEmail
            // .Replace("{{ UserName }}", user.FirstName)
            // .Replace("{{ Password }}", entity.Password)
            // .Replace("{{ ResetUrl }}", linkUrl);
            // _emailHelper.SendEmail("Password Reset", mainBody, "", email);
            _mailService.CreateMail("Reset Password", _sharedResource.ForgetPasswordEmail, 
                new Dictionary<string, string>() 
                {
                    { "userName", user.FirstName },
                    { "password", entity.Password },
                    { "ResetUrl", linkUrl }
                }, email);
            return result;
        }

        public void ResetPassword(ResetPasswordDto dto)
        {
            var temporaryPassword = _context.TemporaryPasswords
            .SingleOrDefault(x => x.Token == dto.Token);
            if (null == temporaryPassword)
            {
                throw new UniWebBusinessException("Invalid token for resetting password.");
            }
            var timeDifference = DateTime.Now - temporaryPassword.CreatedAt;
            if (24 < timeDifference.TotalHours)
            {
                throw new UniWebBusinessException("Forget password and rest token expired.");
            }
            if ((temporaryPassword.Password != dto.TemporaryPassword))
            {
                throw new UniWebBusinessException("Password mismatch. Temporary password verification failed.");
            }

            var user = _context.Users
            .SingleOrDefault(x => x.Id == temporaryPassword.UserId);
            if (null == user)
            {
                throw new UniWebBusinessException("Invalid user.");
            }
            user.Password = _hasher.HashPassword(dto.NewPassword);
            user.UpdatedDate = DateTime.Now;
            _context.Users.Update(user);
            _context.TemporaryPasswords.Remove(temporaryPassword);
            _context.SaveChanges();
        }

        //public UserRoleDto AddRole(UserRoleDto userRoleDto)
        //{
        //    var userrole = _userRoleDataService.AddRole(userRoleDto);
        //    return userrole;
        //}


        public List<UserRegisterDto> GetAllUsers()
        {
            try
            {
                var users = _userDataService.GetUsers();
                return users;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #region Private methods

        private string GenerateTemporaryPassword()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var password = "";
            for (int index = 0; index < 8; index++)
            {
                int ordinal = random.Next(65, 90);
                var character = Convert.ToChar(ordinal);
                password += character;
            }
            return password;
        }

        private User ValidateTokenAndUser(string refreshToken)
        {
            try
            {
                var rt = _context.UserToken.FirstOrDefault(t => t.Value == refreshToken);
                if (rt == null)
                {
                    // refresh token not found or invalid (or invalid client id)
                    throw new UnauthorizedAccessException();
                }

                if (rt.ExpiryTime < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException();
                }

                var _user = _context.Users.FirstOrDefault(x => x.Id == rt.UserId);
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

        private UserToken CreateRefreshToken(int userId)
        {
            double tokenExpiryTime = Convert.ToDouble(_appSettings.RefreshToken_ExpireTime);
            return new UserToken()
            {
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
            };
        }

        private async Task RemoveAllOldAndAddNewToken(int adminId,UserToken refreshToken)
        {
            try
            {
                var oldTokens = _context.UserToken.Where(x => x.UserId == adminId);
                if (null != oldTokens)
                {
                    foreach (var oldToken in oldTokens)
                    {
                        _context.UserToken.Remove(oldToken);
                    }

                    _context.UserToken.Add(refreshToken);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private TokenResponseDto CreateAccessToken(User user, UserToken newRefreshToken)
        {
            // authentication successful so generate jwt token
            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
           
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("LoggedOn", DateTime.Now.ToString())
                               
                }),

                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };



          
            var newtoken = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(newtoken);
           

            return new TokenResponseDto
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                Refresh_Token = newRefreshToken.Value,
                Username = user.Email,
                userId = user.Id
            };
        }
      

    }

    #endregion

}
 
