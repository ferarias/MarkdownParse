using MarkdownParse;
using System.Linq;
using Xunit;

namespace ParserUnitTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("Bring out the **charge** of the Love Brigade", 3)]
        [InlineData("**Bring out the _charge of the** Love Brigade_", 3)]
        [InlineData("Bring out the **charge of the Love Brigade**", 2)]
        [InlineData("**Bring out the charge of the** Love Brigade", 2)]
        [InlineData("**_Rituel DC_**. Réviser dernière classe. Les élèves écrivent le _Rituel DC_ dans le Portfolio. ", 4)]
        public void Test3(string text, int chunkCount)
        {
            // Arrange
            var formatter = new Formatter();

            // Act
            var chunks = formatter.ParseFormattedText(text);

            // Assert
            Assert.Equal(chunkCount, chunks.Count());
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var formatter = new Formatter();

            // Act
            var chunks = formatter.ParseFormattedText("**Bring out the _charge of the** Love Brigade_").ToArray();

            // Assert
            Assert.Contains<FormattingStyle>(FormattingStyle.Bold, chunks[0].FormattingStyles);
            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Italic, chunks[0].FormattingStyles);
            Assert.Contains<FormattingStyle>(FormattingStyle.Bold, chunks[1].FormattingStyles);
            Assert.Contains<FormattingStyle>(FormattingStyle.Italic, chunks[1].FormattingStyles);
            Assert.Contains<FormattingStyle>(FormattingStyle.Italic, chunks[2].FormattingStyles);
            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Bold, chunks[2].FormattingStyles);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            var formatter = new Formatter();

            // Act
            var chunks = formatter.ParseFormattedText("**_Rituel DC_**. Réviser dernière classe. Les élèves écrivent le _Rituel DC_ dans le Portfolio. ").ToArray();

            // Assert
            Assert.Contains<FormattingStyle>(FormattingStyle.Bold, chunks[0].FormattingStyles);
            Assert.Contains<FormattingStyle>(FormattingStyle.Italic, chunks[0].FormattingStyles);

            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Bold, chunks[1].FormattingStyles);
            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Italic, chunks[1].FormattingStyles);

            Assert.Contains<FormattingStyle>(FormattingStyle.Italic, chunks[2].FormattingStyles);
            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Bold, chunks[2].FormattingStyles);

            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Italic, chunks[3].FormattingStyles);
            Assert.DoesNotContain<FormattingStyle>(FormattingStyle.Bold, chunks[3].FormattingStyles);
        }
    }
}
