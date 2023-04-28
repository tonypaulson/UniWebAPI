
using UniWeb.API.Helpers;
using UniWeb.API.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ISharedResource, SharedResource>();
            services.AddTransient<IEmailHelper, EmailHelper>();
           
          
            services.AddTransient<IFileUploader, FileUploader>();
            services.AddTransient<UserService>();
            services.AddSingleton<ConfigurationService>();
        
            services.AddTransient<TimeService>();
            services.AddTransient<AdminService>();
            services.AddTransient<PasswordHaser>();
            services.AddTransient<PlaceService>();
            services.AddTransient<MailService>();
          
            services.AddSingleton<IAppConfiguration, AwsConfiguration>();
            

        }
    }
}
