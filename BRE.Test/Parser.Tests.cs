using System;
using Xunit;

namespace BRE.Test
{
    public class ParserTests
    {

		[Fact]
        public void Basic_Boolean_Logic_Got_Correct_AST()
        {

            string input = "1 + 3 > 1 && 1 + 1 > 0";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            var BinaryAst = (BinaryExpressionNode)AST;

			var left = (BinaryExpressionNode)BinaryAst.Left;

            var right = (BinaryExpressionNode)BinaryAst.Right;

			Assert.Equal(BinaryExpressionType.GreaterThan, left.BinaryType);
            Assert.Equal(BinaryExpressionType.GreaterThan, right.BinaryType);

        }


		[Fact]
        public void Add_Operator_Has_Higher_Precedence_Than_Adding()
        {

            string input = "1 && 1 + 1";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            var BinaryAst = (BinaryExpressionNode)AST;

            var left = (ConstantExpression)BinaryAst.Left;

			var right = (BinaryExpressionNode)BinaryAst.Right;

            Assert.Equal(BinaryExpressionType.And, BinaryAst.BinaryType);
            Assert.Equal(1.0, (double)left.Value.Value);
            Assert.Equal(BinaryExpressionType.Add, right.BinaryType);

        }

		[Fact]
        public void Variables_Can_Be_Found_By_Name()
        {

            string input = "1+Apple";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            var BinaryAst = (BinaryExpressionNode)AST;

            var left = (ConstantExpression)BinaryAst.Left;

			var right = (VariableExpression)BinaryAst.Right;
         
			Assert.Equal(BinaryExpressionType.Add, BinaryAst.BinaryType);
			Assert.Equal(1.0, (double)left.Value.Value);
			Assert.Equal("Apple", right.VariableValue.VariableName);

        }

		[Fact]
        public void Parentheses_Affect_The_Order_Of_AST()
        {

			string input = "1*(2+2.2)+3;";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

			var BinaryAst = (BinaryExpressionNode)AST;

			var left = (BinaryExpressionNode)BinaryAst.Left;

			var leftRight = (ExpressionNode)left.Right;

			var targetExpression = (BinaryExpressionNode)leftRight.Expression;

			Assert.Equal(BinaryExpressionType.Add, targetExpression.BinaryType);
			Assert.Equal(2.0, (double)targetExpression.Left.Value.Value);
			Assert.Equal(2.2, (double)targetExpression.Right.Value.Value);      

        }

		[Fact]
		public void Order_Of_Adding_And_Multiplyging_Is_Correct()
        {

			string input = "1*2+3;";

            var TextTokens = TextTokenizer.TextTokenize(input);

			Parser parsers = new Parser(TextTokens);

			var AST = parsers.ParseExpression();

			Assert.Equal(ExpressionType.BinaryOperation, AST.Type);
			Assert.Equal(BinaryExpressionType.Add, ((BinaryExpressionNode)AST).BinaryType);

        }

		[Fact]
        public void Order_Of_Adding_And_Multiplyging_Is_Correct2()
        {

            string input = "1+2*3;";

            var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AST = parsers.ParseExpression();

            Assert.Equal(ExpressionType.BinaryOperation, AST.Type);
            Assert.Equal(BinaryExpressionType.Add, ((BinaryExpressionNode)AST).BinaryType);

        }
        
    }
}
