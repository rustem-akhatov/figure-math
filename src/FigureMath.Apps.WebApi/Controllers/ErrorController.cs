using System;
using System.Net;
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

            (HttpStatusCode statusCode, string message) = GetMessageAndHttpStatusCode(errorContext.Error);
            
            if ((int)statusCode >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(errorContext.Error, $"Exception on {Request.Method} {errorContext.Path}");    
            }

            return showExceptionInfo 
                ? Problem(statusCode: (int)statusCode, title: errorContext.Error.Message, detail: errorContext.Error.StackTrace)
                : Problem(statusCode: (int)statusCode, title: message);
        }

        private static (HttpStatusCode, string) GetMessageAndHttpStatusCode(Exception exception)
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (exception)
            {
                case EntityNotFoundException entityNotFoundException:
                    return (HttpStatusCode.NotFound, entityNotFoundException.Message);
                default:
                    return (HttpStatusCode.InternalServerError, null);
            }
        }
    }
}