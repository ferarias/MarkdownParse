using System.Collections.Generic;

namespace MarkdownParse
{
    public class FormattedChunk
    {
        public string Text { get; set; }
        public IEnumerable<FormattingStyle> FormattingStyles {get; set;}
    }
}