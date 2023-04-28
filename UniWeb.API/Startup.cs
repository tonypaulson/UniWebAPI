using System;
using UniWeb.API.DataContext;
using UniWeb.API.Middleware;
using UniWeb.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Globalization;
using UniWeb.API.Services;
using Carewell.API.Filters;
using NuGet.Protocol.Core.Types;

namespace Carewell.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(GlobalExceptionFilter));
            });
            services.AddDbContext<EFDataContext>((_services, options) =>
            {
                var configuration = (ConfigurationService)_services.GetService(typeof(ConfigurationService))!;
                var logger = (ILogger<Startup>)_services.GetService(typeof(ILogger<Startup>))!;
                //var connectionString = Environment.GetEnvironmentVariable("connectionstring");
                var connectionString = configuration.GetConnectionString();


                if (string.IsNullOrEmpty(connectionString))
                {
                   // logger.LogWarning("Connection string is not resolved.");
                    return;
                }

                //logger.LogInformation($"Connection string: {connectionString}");

                options.UseMySql(connectionString, new MySqlServerVersion(new Version()),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly("UniWeb.API");
                        mySqlOptions.EnableStringComparisonTranslations();
                    });


            });

            services.AddControllers();
           
            services.AddLocalization((config) => { config.ResourcesPath = "Resources"; });
            services.AddServices();
            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "V-Care�", Version = "v1" });
            });

            var dbConfig = Configuration.GetSection(nameof(SMTPSettings)).Get<SMTPSettings>();
            services.AddSingleton<SMTPSettings>(dbConfig);
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddSingleton<AppSettings>(appSettings);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> loggerFactory)
        {
            if (env.IsDevelopment())
            {
               // loggerFactory.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Startup>>();
                try
                {
                    var context = services.GetRequiredService<EFDataContext>();
                    if ((null == context) || (null == context.Database))
                    {
                       // logger.LogWarning("DBContext is not resolved.");
                    }
                    else
                    {
                        //logger.LogInformation("Applying database migration.");
                        context.Database.Migrate();
                       // logger.LogInformation("Database migrations has been applied.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while trying to apply database migrations.");
                }
            }
            app.UseTokenDecryptor();
            app.UseCors("AllowAllOrigins");

            var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("es"), new CultureInfo("fr"), };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.OAuthAppName("Lambda Api");
                c.OAuthScopeSeparator("");
                c.OAuthUsePkce();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNet_AWS");
            });

            // app.UseHttpsRedirection(); Not needed for hosting on apache2

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
            });
        }
    }
}
