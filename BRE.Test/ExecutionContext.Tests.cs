using System;
using Xunit;

namespace BRE.Test
{
    public class ExecutionContext
    {
        public ExecutionContext()
        {
        }

		[Fact]
        public void One_Plus_One_Is_Two()
        {

            string input = "1 + 1";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

			global::BRE.ExecutionContext context = new global::BRE.ExecutionContext();

			ExecutionEngine executionEngine = new ExecutionEngine(context);

			executionEngine.ExecuteExpression(AST);

			Assert.Equal(2.0, (double)AST.EvaluatedValue.Value);
        }


		[Fact]
		public void One_Plus_Two_Is_Greater_Than_Two()
        {

            string input = "1 + 2 > 2";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            global::BRE.ExecutionContext context = new global::BRE.ExecutionContext();

            ExecutionEngine executionEngine = new ExecutionEngine(context);

            executionEngine.ExecuteExpression(AST);

			Assert.Equal(ValueType.Logical, AST.EvaluatedValue.Type);

            Assert.Equal(true, (bool)AST.EvaluatedValue.Value);
        }

		[Fact]
        public void Variable_Can_Be_Found_From_Context()
        {

            string input = "Word";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            global::BRE.ExecutionContext context = new global::BRE.ExecutionContext();

			context.AddVariableValue("word", 2.313);

            ExecutionEngine executionEngine = new ExecutionEngine(context);

            executionEngine.ExecuteExpression(AST);

            Assert.Equal(ValueType.Number, AST.EvaluatedValue.Type);

			Assert.Equal(2.313, (double)AST.EvaluatedValue.Value);
        }
        
    }
}
