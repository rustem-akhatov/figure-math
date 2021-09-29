using System;
using System.Diagnostics;
using EnsureThat;
using Microsoft.AspNetCore.Builder;

namespace FigureMath.Common.AspNet.Logging
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Fills <see cref="CorrelationManager.ActivityId"/> for current <see cref="Trace.CorrelationManager"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseActivityId(this IApplicationBuilder app)
        {
            EnsureArg.IsNotNull(app, nameof(app));

            return app.Use((_, next) =>
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
                
                return next();
            });
        }

        /// <summary>
        /// Adds a <see cref="RequestResponseLoggingMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            EnsureArg.IsNotNull(app, nameof(app));

            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        /// <summary>
        /// Adds a <see cref="UnhandledExceptionLoggingMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseUnhandledExceptionLogging(this IApplicationBuilder app)
        {
            EnsureArg.IsNotNull(app, nameof(app));

            return app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();
        }
    }
}