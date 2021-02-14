using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using FigureMath.Common.AspNet.Http;
using FigureMath.Common.Logging;
using FigureMath.Common.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FigureMath.Common.AspNet.Logging
{
    /// <summary>
    /// A middleware for logging HTTP-requests and HTTP-responses. Can affect performance.
    /// </summary>
    [UsedImplicitly]
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.</param>
        /// <param name="logger">Logger for <see cref="RequestResponseLoggingMiddleware"/>.</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
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

            HttpRequest request = context.Request;

            string startLogMessage = await GetStartLogMessageAsync(request);
            
            _logger.LogInformation(startLogMessage);

            try
            {
                string responseBody = await _next.RunAndReadResponseBodyAsync(context);
                
                string completeLogMessage = GetCompleteLogMessage(request, context.Response, responseBody);
                
                _logger.LogInformation(completeLogMessage);
            }
            catch (Exception ex)
            {
                _logger.Log(GetLogLevel(context.Response.StatusCode), ex, GetBeggingOfCompleteLogMessage(context));
            }
        }

        private static async Task<string> GetStartLogMessageAsync(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append($"Start {request.Method} {request.Path}{request.QueryString} ({request.ContentType})");

            // ReSharper disable once ConstantConditionalAccessQualifier
            string mimeType = request.ContentType?.Split(";")[0];

            if (MediaTypeNames.Application.TextLike.Contains(mimeType))
            {
                string requestBody = await request.ReadRequestBodyAsync();

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    stringBuilder.AppendLine();

                    stringBuilder.Append(LoggingHelper.TruncateLongStrings(requestBody, mimeType));
                }
            }

            return stringBuilder.ToString();
        }
        
        private static string GetCompleteLogMessage(HttpRequest request, HttpResponse response, string responseBody)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(GetBeggingOfCompleteLogMessage(request, response));

            if (!string.IsNullOrEmpty(response.ContentType))
            {
                string mimeType = response.ContentType.Split(";")[0];

                stringBuilder.AppendLine();
                stringBuilder.Append($"Content-Type: {mimeType}");

                if (!string.IsNullOrWhiteSpace(responseBody) && MediaTypeNames.Application.TextLike.Contains(mimeType))
                {
                    stringBuilder.AppendLine();

                    stringBuilder.Append(LoggingHelper.TruncateLongStrings(responseBody, mimeType));
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetBeggingOfCompleteLogMessage(HttpContext context)
        {
            return GetBeggingOfCompleteLogMessage(context.Request, context.Response);
        }

        private static string GetBeggingOfCompleteLogMessage(HttpRequest request, HttpResponse response)
        {
            var statusCodeText = Enum.GetName(typeof(HttpStatusCode), response.StatusCode) ?? "Unknown";

            return $"Complete {request.Method} {request.Path} {response.StatusCode} ({statusCodeText})";
        }
        
        private static LogLevel GetLogLevel(int statusCode)
        {
            return statusCode >= 500 ? LogLevel.Error : LogLevel.Information;
        }
    }
}