using System.IO;
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
        /// Executes <paramref name="next"/> and returns HTTP-response body as a string.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/>.</param>
        /// <param name="context">The <see cref="HttpContext"/> to pass to <paramref name="next"/>.</param>
        /// <returns>HTTP-response body as a string.</returns>
        public static async Task<string> RunAndReadResponseBodyAsync(this RequestDelegate next, HttpContext context)
        {
            EnsureArg.IsNotNull(next, nameof(next));
            EnsureArg.IsNotNull(context, nameof(context));

            Stream originalStream = context.Response.Body;
            
            await using var stream = new MemoryStream();

            context.Response.Body = stream;

            try
            {
                await next(context);
             
                // TODO: It's necessary to consider ContentType and maximum length.
                // For logging is only needed MediaTypeNames.Application.TextLike responses and 2048 bytes in maximum length.
                // See method RequestResponseLoggingMiddleware.GetCompleteLogMessage to solve this problem.
                
                string result = await stream.ReadAllTextAsync(Encoding.UTF8);

                return result;
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
                
                await stream.CopyToAsync(originalStream);
            }
        }

        private static async Task<string> ReadAllTextAsync(this Stream stream, Encoding encoding)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream, encoding, false, 1024, true);
            
            return await reader.ReadToEndAsync();
        }
    }
}