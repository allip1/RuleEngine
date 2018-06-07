using System;
using System.Collections.Generic;
using Xunit;

namespace BRE.Test
{
    public class BRETests
    {
		public BRETests()
        {
        }

		[Fact]
		public void Real_Word_Example()
        {

			string input = "NumberOfCards > 30 && Three >= (1+1)";

			Dictionary<string, double> preDefinedVariables = new Dictionary<string, double>
			{
				{ "NumberOfCards", 30.1 },
				{ "Three", 3 }
			};

			BRE bRE = new BRE();
			object result = bRE.EvaluateSentence(input, preDefinedVariables);

			Assert.Equal(true, result);
        }


		[Fact]
        public void Real_Word_Example2()
        {

			string input = "NumberOfCards == 30 || 1+2 < 3+3 && 3*2 <= 7 && Three > (1-1)";

            Dictionary<string, double> preDefinedVariables = new Dictionary<string, double>
            {
				{ "NumberOfCards", 30.1 },
				{ "Three", 3 }
            };

            BRE bRE = new BRE();
            object result = bRE.EvaluateSentence(input, preDefinedVariables);

            Assert.Equal(true, result);
        }

        [Fact]
        public void Text_To_AST_To_Text()
		{
			string input = "NumberOfCards > 30 && Three > (1 + 1)";
            
            Dictionary<string, double> preDefinedVariables = new Dictionary<string, double>
            {
				{ "NumberOfCards", 30.1 },
                { "Three", 3 }
            };

            BRE bRE = new BRE();
            var ast = bRE.SentenceToExpression(input, preDefinedVariables);

			String result = bRE.ExpressionToSentence(ast, preDefinedVariables);

			Assert.Equal(input, result);
		}
    }
}
