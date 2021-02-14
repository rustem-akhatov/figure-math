using FigureMath.Common.Extensions;
using FigureMath.Common.Json;
using FigureMath.Common.Net.Mime;

namespace FigureMath.Common.Logging
{
    /// <summary>
    /// Helper methods for logging.
    /// </summary>
    public static class LoggingHelper
    {
        /// <summary>
        /// Truncate specified <paramref name="input"/> using <paramref name="mimeType"/>.
        /// </summary>
        /// <param name="input">Text to be truncated.</param>
        /// <param name="mimeType">Media type of the specified <paramref name="input"/>.</param>
        /// <param name="maxLengthPerJsonProperty">Maximum length to keep of the value of the property in case of <paramref name="input"/> is JSON-string.</param>
        /// <param name="maxLogMessageLength">Maximum length of the log message.</param>
        /// <returns>Truncated string.</returns>
        public static string TruncateLongStrings(string input, string mimeType, int maxLengthPerJsonProperty = 256, int maxLogMessageLength = 2048)
        {
            string result = MediaTypeNames.Application.JsonLike.Contains(mimeType)
                ? JsonFormatter.TruncateLongStrings(input, maxLengthPerJsonProperty)
                : input;

            return result.Truncate(maxLogMessageLength);
        }
    }
}