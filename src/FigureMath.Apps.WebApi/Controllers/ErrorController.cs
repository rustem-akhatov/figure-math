using System;
using EnsureThat;
using FigureMath.Apps.Hosting;
using FigureMath.Common.Data.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FigureMath.Apps.WebApi.Controllers
{
    /// <summary>
    /// Handles errors that occur during application running.
    /// </summary>
    [ApiController]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ErrorController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="env">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <param name="logger">An instance of <see cref="ILogger{ErrorController}"/>.</param>
        public ErrorController(IHostEnvironment env, ILogger<ErrorController> logger)
        {
            _env = EnsureArg.IsNotNull(env, nameof(env));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }
        
        /// <summary>
        /// Handles all errors.
        /// </summary>
        [Route("unknown")]
        public IActionResult Unknown()
        {
            var errorContext = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (errorContext?.Error == null)
                return NotFound();
            
            bool showExceptionInfo = _env.IsDevelopment() || _env.IsDockerDesktop();

            (int statusCode, string message) = GetMessageAndHttpStatusCode(errorContext.Error);
            
            if (statusCode >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(errorContext.Error, $"Exception on {Request.Method} {errorContext.Path}");    
            }

            return showExceptionInfo 
                ? Problem(statusCode: statusCode, title: errorContext.Error.Message, detail: errorContext.Error.StackTrace)
                : Problem(statusCode: statusCode, title: message);
        }

        private static (int, string) GetMessageAndHttpStatusCode(Exception exception)
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (exception)
            {
                case EntityNotFoundException entityNotFoundException:
                    return (StatusCodes.Status404NotFound, entityNotFoundException.Message);
                default:
                    return (StatusCodes.Status500InternalServerError, null);
            }
        }
    }
}