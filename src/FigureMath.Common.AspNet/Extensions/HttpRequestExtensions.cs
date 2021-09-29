using System.IO;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Http;

namespace FigureMath.Common.AspNet
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Reads and returns HTTP-request body.
        /// Also resets the request body stream position so the next middleware can read it.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/>.</param>
        /// <returns>HTTP-request body as a string.</returns>
        public static async Task<string> ReadRequestBodyAsync(this HttpRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));
            
            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, Encoding.UTF8, false, 1024, true);
            string body = await reader.ReadToEndAsync();
            
            request.Body.Position = 0;

            return body;
        }
    }
}