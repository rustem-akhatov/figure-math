using System;
using System.Threading.Tasks;
using EnsureThat;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FigureMath.Common.AspNet.Logging
{
    /// <summary>
    /// A middleware for logging unhandled exceptions.
    /// </summary>
    public class UnhandledExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnhandledExceptionLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.</param>
        /// <param name="logger">Logger for <see cref="UnhandledExceptionLoggingMiddleware"/>.</param>
        public UnhandledExceptionLoggingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionLoggingMiddleware> logger)
        {
            _next = EnsureArg.IsNotNull(next, nameof(next));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        [UsedImplicitly]
        public async Task Invoke(HttpContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred while executing the request.");
                throw;
            }
        }
    }
}