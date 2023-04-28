using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Carewell.API.DTO;
using System.Linq;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using UniWeb.API.Entities;

namespace UniWeb.API.Services
{
    public class ConfigurationService
    {
        private IConfiguration _configuration = null!;
        private IWebHostEnvironment _env = null!;
        private readonly ILogger<ConfigurationService> _logger;
        public ConfigurationService(IConfiguration configuration,
        IWebHostEnvironment env, ILogger<ConfigurationService> logger)
        {
            _configuration = configuration;
            _env = env;
            _logger = logger;
        }

        public string GetSignInUrl()
        {
            var signInUrl = _configuration.GetValue<string>("CWL_SIGNIN_URL", "https://my.dev.imaginepractice.com/admin/login");
            return signInUrl;
        }

        public string GetConnectionString()
        {
            return "server =localhost;database=UniWeb.API;user = root;password =Root;";

            var dbhost = _configuration.GetValue<string>("CWL_DB_HOST", "localhost");
            var dbname = _configuration.GetValue<string>("CWL_DB_NAME", "");
            var dbuser = _configuration.GetValue<string>("CWL_DB_USER", null!);
            var dbpassword = _configuration.GetValue<string>("CWL_DB_PASSWORD", null!);
            var dbport = _configuration.GetValue<int>("CWL_DB_PORT", 0);

            if (string.IsNullOrEmpty(dbuser))
            {
                return GetDefaultConnectionString();
            }

            if (string.IsNullOrEmpty(dbpassword))
            {
                return GetDefaultConnectionString();
            }

            return $"server={dbhost};database={dbname};user={dbuser};password={dbpassword}";
        }

        public string GetDefaultConnectionString()
        {
            if (_env.IsDevelopment())
            {
               // _logger.LogInformation("ENVIRONMENT VARIABLES: " + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariables()));
               // _logger.LogInformation("CONNECTION STRING:"+ JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariable("connectionstring")));
                return _configuration.GetConnectionString("Defaultconnection");
                //return Environment.GetEnvironmentVariable("connectionstring");
            }
            else if (_env.IsStaging())
            {
               // _logger.LogInformation("ENVIRONMENT VARIABLES: " + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariables()));
               // _logger.LogInformation("CONNECTION STRING:" + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariable("connectionstring")));
                //return _configuration.GetConnectionString("Staging");
                return Environment.GetEnvironmentVariable("connectionstring");
            }
            else
            {
              //  _logger.LogInformation("ENVIRONMENT VARIABLES: " + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariables()));
              //  _logger.LogInformation("CONNECTION STRING:" + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariable("connectionstring")));
              //  return _configuration.GetConnectionString("Prodcution");
               return Environment.GetEnvironmentVariable("connectionstring");
            }
        }

        public CloudinaryModal GetCloudinaryConfig()
        {
            if (_env.IsDevelopment())
            {
                var cloudinaryConfig = _configuration.GetSection("cloudinary")
                  .GetChildren()
                  .ToList()
                  .Select(x => new CloudinaryModal
                  {
                      Cloud = x.GetValue<string>("Local:Cloud"),
                      API_key = x.GetValue<string>("Local:API_key"),
                      API_Secret = x.GetValue<string>("Local:API_Secret")
                  });

                return cloudinaryConfig.ToList()[0];
            }
            else if (_env.IsStaging())
            {
                var cloudinaryConfig = _configuration.GetSection("cloudinary")
                 .GetChildren()
                 .ToList()
                 .Select(x => new CloudinaryModal
                 {
                     Cloud = x.GetValue<string>("Staging:Cloud"),
                     API_key = x.GetValue<string>("Staging:API_key"),
                     API_Secret = x.GetValue<string>("Staging:API_Secret")
                 });

                return cloudinaryConfig.ToList()[0];
            }
            else
            {
                var cloudinaryConfig = _configuration.GetSection("cloudinary")
               .GetChildren()
               .ToList()
               .Select(x => new CloudinaryModal
               {
                   Cloud = x.GetValue<string>("Production:Cloud"),
                   API_key = x.GetValue<string>("Production:API_key"),
                   API_Secret = x.GetValue<string>("Production:API_Secret")
               });

                return cloudinaryConfig.ToList()[0];
            }
        }
    }
}