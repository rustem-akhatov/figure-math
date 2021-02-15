using System;
using System.Net;
using EnsureThat;
using FigureMath.Apps.Hosting;
using FigureMath.Common.Data.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="env">An instance of <see cref="IWebHostEnvironment"/>.</param>
        public ErrorController(IHostEnvironment env)
        {
            _env = EnsureArg.IsNotNull(env, nameof(env));
        }
        
        /// <summary>
        /// Handles all errors.
        /// </summary>
        [Route("unknown")]
        public IActionResult Unknown()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (context?.Error == null)
                return NotFound();
            
            bool showExceptionInfo = _env.IsDevelopment() || _env.IsDockerDesktop();

            (HttpStatusCode statusCode, string message) = GetMessageAndHttpStatusCode(context.Error);

            return showExceptionInfo 
                ? Problem(statusCode: (int)statusCode, title: context.Error.Message, detail: context.Error.StackTrace)
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