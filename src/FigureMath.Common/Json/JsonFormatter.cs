using System;
using System.Linq;
using FigureMath.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FigureMath.Common.Json
{
    // TODO: Move to System.Text.Json as soon as https://github.com/dotnet/runtime/issues/45188 will be done.
    /// <summary>
    /// Helper methods to work with JSON.
    /// </summary>
    public static class JsonFormatter
    {
        /// <summary>
        /// Truncates large data inside properties.
        /// </summary>
        /// <param name="text">JSON as a string.</param>
        /// <param name="maxLengthPerPropertyToKeep">Maximum length to keep of the value of the property.</param>
        /// <returns>Truncated JSON as a string.</returns>
        public static string TruncateLongStrings(string text, int maxLengthPerPropertyToKeep)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLengthPerPropertyToKeep)
                return text;
            
            JToken json;

            try
            {
                json = JToken.Parse(text);
            }
            catch (JsonReaderException ex)
            {
                return ex.Message + Environment.NewLine + text.Truncate(maxLengthPerPropertyToKeep);
            }

            TruncateLongStrings(json, maxLengthPerPropertyToKeep);

            return json.ToString(Formatting.Indented);
        }

        private static void TruncateLongStrings(JToken node, int maxLengthPerPropertyToKeep)
        {
            if (node.Type == JTokenType.Object || node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children().ToList())
                {
                    TruncateLongStrings(child, maxLengthPerPropertyToKeep);
                }
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (node.Type)
            {
                case JTokenType.Property:
                {
                    var property = (JProperty)node;

                    // ReSharper disable once TailRecursiveCall
                    TruncateLongStrings(property.Value, maxLengthPerPropertyToKeep);

                    break;
                }
                case JTokenType.String:
                {
                    var strValue = (string)node;

                    node.Replace(strValue.Truncate(maxLengthPerPropertyToKeep));

                    break;
                }
            }
        }
    }
}