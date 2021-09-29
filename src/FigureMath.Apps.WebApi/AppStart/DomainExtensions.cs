using System;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Apps.WebApi.AppStart
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to configure Domain services.
    /// </summary>
    public static class DomainExtensions
    {
        /// <summary>
        /// Adds domain services to <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            // ReSharper disable once RedundantNameQualifier
            Type domainAssemblyType = typeof(Domain.AssemblyInfo);
            
            return services
                .Scan(scan => scan
                    .FromAssembliesOf(domainAssemblyType)
                    .AddClasses(classes => classes.AssignableTo<IFigureDescriptor>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime())
                .AddSingleton<IFigureDescriptorProvider, FigureDescriptorProvider>()
                .AddSingleton<IFigureInfoTypeProvider, FigureInfoTypeProvider>()
                .AddSingleton<IFigureInfoFactory, FigureInfoFactory>()
                .AddMediatR(domainAssemblyType);
        }
    }
}