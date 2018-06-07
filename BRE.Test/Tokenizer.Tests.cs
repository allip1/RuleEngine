using System;
using Xunit;
using BRE;

namespace BRE.Test
{
    public class TokenizationTests
    {
        [Fact]
        public void Test_Correct_TokenContent()
        {

			string input = "everything is over 1.11 + 1 && 1 < 1";

			var tokens = TextTokenizer.TextTokenize(input);

			Assert.Equal("everything", new String(tokens[0].RawData.ToArray()));

			Assert.Equal("is", new String(tokens[1].RawData.ToArray()));

			Assert.Equal("over", new String(tokens[2].RawData.ToArray()));
            
			Assert.Equal("1.11", new String(tokens[3].RawData.ToArray()));
                     
			Assert.Equal("+", new String(tokens[4].RawData.ToArray()));

			Assert.Equal("1", new String(tokens[5].RawData.ToArray()));

			Assert.Equal("&&", new String(tokens[6].RawData.ToArray()));
           
            Assert.Equal("1", new String(tokens[7].RawData.ToArray()));

            Assert.Equal("<", new String(tokens[8].RawData.ToArray()));

            Assert.Equal("1", new String(tokens[9].RawData.ToArray()));
        }

		[Fact]
        public void Test_Correct_TokenType()
        {

			string input = "everything (over 1.11 + 1)&& ;<";

			var tokens = TextTokenizer.TextTokenize(input);

			Assert.Equal(TextTokenType.Word, tokens[0].Type);
           
            Assert.Equal(TextTokenType.ParentheseStart, tokens[1].Type);

            Assert.Equal(TextTokenType.Word, tokens[2].Type);
            
            Assert.Equal(TextTokenType.Number, tokens[3].Type);

            Assert.Equal(TextTokenType.Operator, tokens[4].Type);

            Assert.Equal(TextTokenType.Number, tokens[5].Type);

            Assert.Equal(TextTokenType.ParentheseStop, tokens[6].Type);

			Assert.Equal(TextTokenType.Operator, tokens[7].Type);
   
            Assert.Equal(TextTokenType.EndOfSentence, tokens[8].Type);

            Assert.Equal(TextTokenType.Operator, tokens[9].Type);
        }
    }
}
