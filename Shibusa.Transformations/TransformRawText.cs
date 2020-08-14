using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Utility class for common text manipulations.
    /// </summary>
    public static class TransformRawText
    {
        /// <summary>
        /// The regular expression for the data tag match.
        /// </summary>
        public const string DATA_TAG_EXPRESSION = @"<data name=""([^\""]+)""[\s+]?\/>";

        /// <summary>
        /// The regular expression for the curly brace match.
        /// </summary>
        public const string CURLY_BRACE_EXPRESSION = @"\{[\s+]?([^\}]+)\}";

        /// <summary>
        /// The regular expression for the bracket match.
        /// </summary>
        public const string BRACKET_EXPRESSION = @"\[[\s+]?([^\}]+)\]";

        /// <summary>
        /// The regular expression for the pound match.
        /// </summary>
        public const string POUND_EXPRESSION = @"#[\s+]?([^#}]+)#";

        /// <summary>
        /// Convert all newline and tab characters to spaces.
        /// </summary>
        /// <param name="text">Text to manipulate.</param>
        /// <returns>Returns the supplied string with all newline and tab
        /// characters replaced with spaces. If provided null, null is returned.</returns>
        public static string ConvertNewLinesAndTabsToSpaces(string text)
        {
            return string.IsNullOrWhiteSpace(text)
                ? text
                : text.Replace("\n", " ").Replace("\r", "").Replace("\t", " ");
        }

        /// <summary>
        /// Replace all instances of two or more spaces (including tabs) with a single space.
        /// </summary>
        /// <param name="text">Text to manipulate.</param>
        /// <returns>Returns the supplied string with only single spaces between other characters.
        /// Tabs are converted to spaces. If provided null, null is returned.</returns>
        public static string CondenseSpacingAndTrim(string text)
        {
            if (text == null) { return null; }
            text = text.Replace("\t", " ");
            while (text.Contains("  ")) { text = text.Replace("  ", " ").Trim(); }
            return text;
        }

        /// <summary>
        /// Transform a template into a string using a regular expression.
        /// </summary>
        /// <param name="template">The template text.</param>
        /// <param name="keyValuePairs">A dictionary of key/value pairs to inform the transformation.</param>
        /// <param name="regularExpression">The regular expression to use for matching text.</param>
        /// <param name="indexOfGroupWithKey">The index of the group within the regular expression that
        /// corresponds to the key in the dictionary.</param>
        /// <param name="throwOnMissingKeys">An indicator of whether missing keys in the dictionary should
        /// throw an exception.</param>
        /// <param name="replaceMissingKeysWithEmptySpace">An indicator of whether keys missing from the dictionary
        /// should be replaced with empty space or left as-is.</param>
        /// <param name="recursive">An indicator of whether the algorithm should function recursively until no matches are found.</param>
        /// <param name="maxRecursiveIterations">The maximum number of recursive iterations; present to prevent infinite loops.</param>
        /// <param name="currentIteration">The current iteration as related to <paramref name="maxRecursiveIterations"/>.</param>
        /// <returns>The transformed text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the template or regularExpression argument is missing.</exception>
        /// <exception cref="ArgumentException">Thrown if the group index integer argument is less than zero (0).</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the group index integer argument is greater than the maximum index of
        /// matched groups.</exception>
        public static string Transform(string template,
            Dictionary<string, string> keyValuePairs,
            string regularExpression = DATA_TAG_EXPRESSION,
            int indexOfGroupWithKey = 0,
            bool throwOnMissingKeys = true,
            bool replaceMissingKeysWithEmptySpace = true,
            bool recursive = false,
            int maxRecursiveIterations = 100,
            int currentIteration = 0)
        {
            if (string.IsNullOrWhiteSpace(template)) { throw new ArgumentNullException(nameof(template)); }
            if (string.IsNullOrWhiteSpace(regularExpression)) { throw new ArgumentException(nameof(regularExpression)); }
            if (indexOfGroupWithKey < 0) { throw new ArgumentException($"{nameof(indexOfGroupWithKey)} cannot be negative."); }

            string templateCopy = string.Copy(template);
            Dictionary<string, string> dictionary;

            try
            {
                dictionary = new Dictionary<string, string>(keyValuePairs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                throw new ArgumentException("The keys provided in the dictionary are not unique. The key matching is case insensitive.");
            }

            Regex regex = new Regex(regularExpression, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            MatchCollection matches = regex.Matches(template);

            foreach (Match match in matches)
            {
                string fullMatch = match.Groups[0].Value;

                if (indexOfGroupWithKey >= match.Groups.Count)
                {
                    throw new IndexOutOfRangeException($"{nameof(indexOfGroupWithKey)} is out of range of available groups.");
                }
                string key = match.Groups[indexOfGroupWithKey].Value.Trim();
                string value = default;

                if (dictionary.ContainsKey(key))
                {
                    value = dictionary[key];
                }
                else
                {
                    if (throwOnMissingKeys) { throw new Exception($"Key '{key}' was not found in the provided dictionary."); }
                    else
                    {
                        value = replaceMissingKeysWithEmptySpace ? string.Empty : value;
                    }
                }
                templateCopy = templateCopy.Replace(fullMatch, value ?? (replaceMissingKeysWithEmptySpace ? string.Empty : fullMatch));
            }

            if (recursive && !templateCopy.Equals(template) && currentIteration < maxRecursiveIterations)
            {
                return Transform(template: templateCopy,
                    keyValuePairs: keyValuePairs,
                    regularExpression: regularExpression,
                    indexOfGroupWithKey: indexOfGroupWithKey,
                    throwOnMissingKeys: throwOnMissingKeys,
                    replaceMissingKeysWithEmptySpace: replaceMissingKeysWithEmptySpace,
                    recursive: recursive,
                    currentIteration: ++currentIteration,
                    maxRecursiveIterations: maxRecursiveIterations);
            }

            return templateCopy;
        }

        /// <summary>
        /// Transform a template using the <see cref="Constants.DATA_TAG_EXPRESSION"/> expression.
        /// </summary>
        /// <param name="template">The template text.</param>
        /// <param name="keyValuePairs">A dictionary of key/value pairs to inform the transformation.</param>
        /// <param name="recursive">An indicator of whether the algorithm should function recursively until no matches are found.</param>
        /// <returns>The transformed text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the template or regularExpression argument is missing.</exception>
        /// <exception cref="ArgumentException">Thrown if the group index integer argument is less than zero (0).</exception>
        public static string TransformDataTags(string template,
            Dictionary<string, string> keyValuePairs,
            bool recursive = false) => Transform(template: template,
                keyValuePairs: keyValuePairs,
                regularExpression: DATA_TAG_EXPRESSION,
                indexOfGroupWithKey: 1,
                recursive: recursive);

        /// <summary>
        /// Transform a template using the <see cref="Constants.CURLY_BRACE_EXPRESSION"/> expression.
        /// </summary>
        /// <param name="template">The template text.</param>
        /// <param name="keyValuePairs">A dictionary of key/value pairs to inform the transformation.</param>
        /// <param name="recursive">An indicator of whether the algorithm should function recursively until no matches are found.</param>
        /// <returns>The transformed text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the template or regularExpression argument is missing.</exception>
        /// <exception cref="ArgumentException">Thrown if the group index integer argument is less than zero (0).</exception>
        public static string TransformBraces(string template,
            Dictionary<string, string> keyValuePairs, bool recursive = false) => Transform(template: template,
                keyValuePairs: keyValuePairs,
                regularExpression: CURLY_BRACE_EXPRESSION,
                indexOfGroupWithKey: 1,
                recursive: recursive);

        /// <summary>
        /// Transform a template using the <see cref="Constants.POUND_EXPRESSION"/> expression.
        /// </summary>
        /// <param name="template">The template text.</param>
        /// <param name="keyValuePairs">A dictionary of key/value pairs to inform the transformation.</param>
        /// <param name="recursive">An indicator of whether the algorithm should function recursively until no matches are found.</param>
        /// <returns>The transformed text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the template or regularExpression argument is missing.</exception>
        /// <exception cref="ArgumentException">Thrown if the group index integer argument is less than zero (0).</exception>
        public static string TransformPounds(string template,
            Dictionary<string, string> keyValuePairs,
            bool recursive = false) => Transform(template: template,
                keyValuePairs: keyValuePairs,
                regularExpression: POUND_EXPRESSION,
                indexOfGroupWithKey: 1,
                recursive: recursive);

        /// <summary>
        /// Transform a template using the <see cref="Constants.BRACKET_EXPRESSION"/> expression.
        /// </summary>
        /// <param name="template">The template text.</param>
        /// <param name="keyValuePairs">A dictionary of key/value pairs to inform the transformation.</param>
        /// <param name="recursive">An indicator of whether the algorithm should function recursively until no matches are found.</param>
        /// <returns>The transformed text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the template or regularExpression argument is missing.</exception>
        /// <exception cref="ArgumentException">Thrown if the group index integer argument is less than zero (0).</exception>
        public static string TransformBrackets(string template,
            Dictionary<string, string> keyValuePairs,
            bool recursive = false) => Transform(template: template,
                keyValuePairs: keyValuePairs,
                regularExpression: BRACKET_EXPRESSION,
                indexOfGroupWithKey: 1,
                recursive: recursive);
    }
}
