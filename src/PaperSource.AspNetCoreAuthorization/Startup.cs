using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperSource.AspNetCoreAuthorization.Services.Permissions;
using PaperSource.AspNetCoreAuthorization.Services.Policies;
using PaperSource.AspNetCoreAuthorization.Services.Resources;

namespace PaperSource.AspNetCoreAuthorization
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("age-adult-policy", x => { x.AddRequirements(new MinAgeRequirement(18)); });
                options.AddPolicy("age-elder-policy", x => { x.AddRequirements(new MinAgeRequirement(42)); });

                options.AddPolicy("resource-allow-policy", x => { x.AddRequirements(new ResourceBasedRequirement()); });
            });

            services.AddSingleton<IAuthorizationHandler, MinAgeHandler>();
            services.AddSingleton<IAuthorizationHandler, ResourceHandlerV1>();
            services.AddSingleton<IAuthorizationHandler, ResourceHandlerV1>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "MyCookieMiddlewareInstance",
                CookieName = "MyCookieMiddlewareInstance",
                LoginPath = new PathString("/Home/Login/"),
                AccessDeniedPath = new PathString("/Home/Error/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
