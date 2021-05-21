using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MarkdownParse
{
    /// <summary>
    /// Parse Markdown into chunks of text with associated format
    /// </summary>
    /// <seealso cref="https://stackoverflow.com/a/18051204"/>
    public class Formatter
    {
        /// <summary>
        /// This will store which string translates into which format
        /// </summary>
        private static readonly Dictionary<string, FormattingStyle> FormattingCharacters = new Dictionary<string, FormattingStyle>
        {
            { "**", FormattingStyle.Bold },
            { "_", FormattingStyle.Italic },
            { "``", FormattingStyle.Code}
        };

        public IEnumerable<FormattedChunk> ParseFormattedText(string text)
        {
            var chunks = new List<FormattedChunk>();
            var currentActiveFormats = new Dictionary<FormattingStyle, bool>();
            foreach (FormattingStyle format in Enum.GetValues(typeof(FormattingStyle)))
            {
                currentActiveFormats.Add(format, false);
            }

            var currentWordBuilder = new StringBuilder();
            for (int currentIndex = 0; currentIndex < text.Length; currentIndex++)
            {
                while (currentIndex < text.Length && IsFormatSymbol(text, currentIndex, out FormattingStyle currentFormatSymbol, out int shift))
                {
                    var currentWord = currentWordBuilder.ToString();
                    if (!string.IsNullOrEmpty(currentWord))
                    {
                        chunks.Add(new FormattedChunk
                        {
                            Text = currentWord,
                            FormattingStyles = (from f in currentActiveFormats
                                                where f.Value.Equals(true)
                                                select f.Key).ToList()
                        });
                        currentWordBuilder = new StringBuilder();
                    }
                    currentActiveFormats[currentFormatSymbol] = !currentActiveFormats[currentFormatSymbol];
                    currentIndex += shift;
                }

                if (currentIndex < text.Length)
                    currentWordBuilder.Append(text[currentIndex]);

            }

            var remainingWord = currentWordBuilder.ToString();
            if (!string.IsNullOrEmpty(remainingWord))
            {
                chunks.Add(new FormattedChunk
                {
                    Text = remainingWord,
                    FormattingStyles = (from f in currentActiveFormats
                                        where f.Value.Equals(true)
                                        select f.Key).ToList()
                });
            }

            return chunks;
        }

        // Checks if the current position is the begining of a format symbol
        // if true - p_currentFormatSymbol will be the discovered format delimiter
        // and p_shift will denote its length
        private static bool IsFormatSymbol(string text, int currentIndex, out FormattingStyle currentFormatStyle, out int symbolLength)
        {
            const int limit = 2;
            var length = currentIndex + 2 > text.Length ? 1 : limit;
            var substring = text.Substring(currentIndex, length);
            foreach (var formatString in FormattingCharacters.Keys)
            {
                if (substring.StartsWith(formatString))
                {
                    symbolLength = formatString.Length;
                    currentFormatStyle = FormattingCharacters[formatString];
                    return true;
                }
            }

            symbolLength = -1;
            currentFormatStyle = FormattingStyle.Undefined;
            return false;
        }
    }
}