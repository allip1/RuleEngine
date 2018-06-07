using System;
using System.Collections.Generic;

namespace BRE
{
    public class BRE
    {
		public BRE()
		{
		}

        public object EvaluateSentence(string input, Dictionary<String, Double> preDefinedVariables)
		{
			var TextTokens = TextTokenizer.TextTokenize(input);

			Parser parsers = new Parser(TextTokens);

			var AbstractSyntaxTree = parsers.ParseExpression();

			ExecutionContext context = variableMapToContext(preDefinedVariables);

			ExecutionEngine executionEngine = new ExecutionEngine(context);

			executionEngine.ExecuteExpression(AbstractSyntaxTree);

			return AbstractSyntaxTree.EvaluatedValue.Value;
		}

		private static ExecutionContext variableMapToContext(Dictionary<string, double> preDefinedVariables)
		{
			ExecutionContext context = new ExecutionContext();

			foreach (var variable in preDefinedVariables)
				context.AddVariableValue(variable.Key, variable.Value);
			return context;
		}

		public IExpression SentenceToExpression(string input, Dictionary<String, Double> preDefinedVariables)
		{
			var TextTokens = TextTokenizer.TextTokenize(input);

            Parser parsers = new Parser(TextTokens);

            var AbstractSyntaxTree = parsers.ParseExpression();

			return AbstractSyntaxTree;
		}

		public String ExpressionToSentence(IExpression abstractSyntaxTree, Dictionary<String, Double> preDefinedVariables)
		{

			ExecutionContext executionContext = variableMapToContext(preDefinedVariables);

			ExecutionEngine executionEngine = new ExecutionEngine(executionContext);

            executionEngine.ExpressionToString(abstractSyntaxTree);
   
			return abstractSyntaxTree.StringValue;
   
		}
    }
}
