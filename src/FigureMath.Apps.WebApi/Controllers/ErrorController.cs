using EnsureThat;
using FigureMath.Common.AspNet;
using FigureMath.Common.AspNet.ExceptionHandling;
using FigureMath.Common.AspNet.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FigureMath.Apps.WebApi
{
    /// <summary>
    /// Handles errors that occur during application running.
    /// </summary>
    [ApiController]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : CommonControllerBase
    {
        private readonly IProblemInfoFactory _problemInfoFactory;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ErrorController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="problemInfoFactory">An instance of <see cref="IProblemInfoFactory"/>.</param>
        /// <param name="env">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <param name="logger">An instance of <see cref="ILogger{ErrorController}"/>.</param>
        public ErrorController(IProblemInfoFactory problemInfoFactory, IHostEnvironment env, ILogger<ErrorController> logger)
        {
            _problemInfoFactory = EnsureArg.IsNotNull(problemInfoFactory, nameof(problemInfoFactory));
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

            bool showExceptionInfo = false;

            var problemContext = new ProblemContext
            {
                RequestMethod = Request.Method,
                Error = errorContext.Error
            };

            IProblemInfo problemInfo = _problemInfoFactory.Create(problemContext);

            if (problemInfo.StatusCode >= StatusCodes.Status500InternalServerError)
            {
                showExceptionInfo =  _env.IsDevelopment() || _env.IsDockerDesktop();
                
                _logger.LogError(errorContext.Error, $"Exception on {Request.Method} {errorContext.Path}");
            }

            return showExceptionInfo
                ? Problem(
                    type: problemInfo.ProblemType,
                    statusCode: problemInfo.StatusCode,
                    title: errorContext.Error.Message,
                    detail: errorContext.Error.StackTrace)
                : Problem(problemInfo);
        }
    }
}