using System;
using EnsureThat;

namespace FigureMath.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="input">String to truncate.</param>
        /// <param name="length">Maximum string length to keep.</param>
        /// <param name="truncatedReplacement">Truncated characters will be replaced by this value.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string input, int length, string truncatedReplacement = "...")
        {
            EnsureArg.IsGte(length, 0, nameof(length));

            if (string.IsNullOrEmpty(input))
                return input;

            int maxLength = Math.Min(input.Length, length);
            
            string replacement = input.Length > length ? truncatedReplacement : string.Empty;

            return input.Substring(0, maxLength) + replacement;
        }
    }
}