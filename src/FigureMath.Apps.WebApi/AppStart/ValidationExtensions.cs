using System.Text.Json;
using EnsureThat;
using FigureMath.Apps.WebApi.Properties;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Apps.WebApi.AppStart
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to configure models validation.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Adds models validation services to <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public static void ConfigureValidation(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            var defaultPropertyNameResolver = ValidatorOptions.Global.PropertyNameResolver;

            ValidatorOptions.Global.PropertyNameResolver =
                (type, info, expression) => JsonNamingPolicy.CamelCase.ConvertName(defaultPropertyNameResolver(type, info, expression));
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(AssemblyInfo))
                .AddClasses(classes => classes.AssignableTo<IValidator>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}