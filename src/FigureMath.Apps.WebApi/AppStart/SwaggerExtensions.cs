using EnsureThat;
using FigureMath.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FigureMath.Apps.WebApi.AppStart
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> and <see cref="IApplicationBuilder"/> to configure Swagger.
    /// </summary>
    public static class SwaggerExtensions
    {
        private const string _apiName = "FigureMath API";

        /// <summary>
        /// Adds Swagger services to <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = _apiName,
                    Version = AssemblyHelper.GetVersion()
                });
            });
        }

        /// <summary>
        /// Adds Swagger and SwaggerUI middlewares.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        public static IApplicationBuilder UseSwaggerApp(this IApplicationBuilder app)
        {
            EnsureArg.IsNotNull(app, nameof(app));

            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", _apiName);
                    options.RoutePrefix = string.Empty;
                });
        }
    }
}