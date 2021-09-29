using System.Text.Json;
using System.Text.Json.Serialization;
using EnsureThat;
using FigureMath.Common.AspNet.ExceptionHandling;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Apps.WebApi.AppStart
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to configure WebApi.
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        /// Adds and configures WebApi services (controllers, JSON, etc.) to <paramref name="services"/>. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<CheckModelStateFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation();

            return services
                .ReplaceDefaultProblemsDetailsFactory()
                .AddSingleton<IProblemInfoFactory, DefaultProblemInfoFactory>();
        }
    }
}