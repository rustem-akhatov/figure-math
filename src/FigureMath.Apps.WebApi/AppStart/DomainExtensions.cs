using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
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
        public static void ConfigureDomain(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            services.Scan(scan => scan
                .FromAssemblyOf<IFigureDescriptor>()
                .AddClasses(classes => classes.AssignableTo<IFigureDescriptor>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            
            services.AddSingleton<IFigureDescriptorProvider, FigureDescriptorProvider>();

            services.AddSingleton<IFigureInfoTypeProvider, FigureInfoTypeProvider>();
            services.AddSingleton<IFigureInfoFactory, FigureInfoFactory>();
            
            services.AddMediatR(
                typeof(Domain.Properties.AssemblyInfo)
            );
        }
    }
}