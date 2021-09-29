using EnsureThat;
using FigureMath.Apps.WebApi.AppStart;
using FigureMath.Common.AspNet.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Apps.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _appConfig;

        public Startup(IConfiguration appConfig)
        {
            _appConfig = EnsureArg.IsNotNull(appConfig, nameof(appConfig));
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            
            services
                .AddDataServices(_appConfig)
                .AddDomainServices()
                .AddValidatorServices()
                .AddWebApiServices()
                .AddSwaggerServices();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            EnsureArg.IsNotNull(app, nameof(app));

            app
                .UseUnhandledExceptionLogging()
                .UseActivityId()
                .UseRequestResponseLogging()
                .UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandlingPath = "/error/unknown",
                    AllowStatusCode404Response = true
                })
                .UseRouting()
                .UseSwaggerApp()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}