using EnsureThat;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FigureMath.Common.AspNet.ExceptionHandling
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Decorates default <see cref="ProblemDetailsFactory"/> using <see cref="ActivityIdProblemDetailsFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection ReplaceDefaultProblemsDetailsFactory(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            return services.Decorate<ProblemDetailsFactory, ActivityIdProblemDetailsFactory>();
        }
    }
}