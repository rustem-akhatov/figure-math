using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Http;

namespace FigureMath.Common.AspNet.Http
{
    /// <summary>
    /// Extension methods for <see cref="RequestDelegate"/>.
    /// </summary>
    public static class RequestDelegateExtensions
    {
        /// <summary>
        /// Executes <paramref name="next"/>, captures exception during execution and body of the result.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/>.</param>
        /// <param name="context">The <see cref="HttpContext"/> to pass to <paramref name="next"/>.</param>
        /// <returns>Tuple of HTTP-response body as a string and captured exception.</returns>
        public static async Task<(string, ExceptionDispatchInfo)> TryRunAsync(this RequestDelegate next, HttpContext context)
        {
            EnsureArg.IsNotNull(next, nameof(next));
            EnsureArg.IsNotNull(context, nameof(context));
            
            Stream originalStream = context.Response.Body;
            
            await using var memoryStream = new MemoryStream();

            context.Response.Body = memoryStream;

            ExceptionDispatchInfo exceptionInfo = null;
            
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                exceptionInfo = ExceptionDispatchInfo.Capture(ex);
            }

            // TODO: It's necessary to consider ContentType and maximum length.
            // For logging is only needed MediaTypeNames.Application.TextLike responses and 2048 maximum length.
            // See method RequestResponseLoggingMiddleware.GetCompleteLogMessage to solve this problem.
                
            string body = await ReadAllTextAsync(memoryStream, Encoding.UTF8);
                
            await memoryStream.CopyToAsync(originalStream);

            return (body, exceptionInfo);
        }
        
        private static async Task<string> ReadAllTextAsync(Stream stream, Encoding encoding)
        {
            string result;

            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream, encoding, false, 1024, true))
            {
                result = await reader.ReadToEndAsync();
            }

            stream.Seek(0, SeekOrigin.Begin);

            return result;
        }
    }
}