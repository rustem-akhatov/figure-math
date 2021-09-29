using EnsureThat;
using FigureMath.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Apps.WebApi.AppStart
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to configure Data services.
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Adds data services to <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="appConfig"></param>
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration appConfig)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNull(appConfig, nameof(appConfig));

            string figureMathDbConnectionString = appConfig.GetConnectionString("FigureMathDb");

            return services
                .AddDbContext<IFigureMathDbContext, FigureMathDbContext>(options =>
                {
                    options.UseNpgsql(figureMathDbConnectionString);
                });
        }
    }
}