using System;
using System.Collections.Generic;

namespace FigureMath.Common.Net.Mime
{
    /// <summary>
    /// Specifies the media type information for a body of HTTP-request.
    /// </summary>
    public static class MediaTypeNames
    {
        /// <summary>
        /// Specifies the kind of application data.
        /// </summary>
        public static class Application
        {
            /// <summary>
            /// Specifies that the <see cref="MediaTypeNames.Application"/> data is in JSON format.
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            /// Specifies that the <see cref="MediaTypeNames.Application"/> data is in Problem JSON format.
            /// </summary>
            public const string JsonProblem = "application/problem+json";

            /// <summary>
            /// Specifies that the <see cref="MediaTypeNames.Application"/> data is in Plain text format.
            /// </summary>
            public const string TextPlain = "text/plain";

            /// <summary>
            /// All JSON formats.
            /// </summary>
            public static readonly HashSet<string> JsonLike = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Json,
                JsonProblem
            };
            
            /// <summary>
            /// All text formats including JSON, XML, etc.
            /// </summary>
            public static readonly HashSet<string> TextLike = new HashSet<string>(JsonLike, StringComparer.OrdinalIgnoreCase)
            {
                TextPlain
            };
        }
    }
}