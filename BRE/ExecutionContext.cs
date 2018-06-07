using System;
using System.Collections.Generic;

namespace BRE
{
	public class ExecutionContext
    {
        Dictionary<String, NumberValue> SymbolTable;

		public ExecutionContext()
		{
			SymbolTable = new Dictionary<String, NumberValue>();
		}

        public bool ContainsVariable(string name)
		{
			return SymbolTable.ContainsKey(name.ToLower());
		}

		public void AddVariableValue(string name, double value)
        {
			SymbolTable.Add(name.ToLower(), new NumberValue(value));
        }

        public double GetVariableValue(string name)
        {
			return SymbolTable[name.ToLower()].DoubleValue;
        }
    }


    public enum VisitingOrder
	{
		PreOrder,
        InOrder,
        PostOrder
	}

    public interface IVisitor
	{
		void Visit(IExpression expression);      
	}


	class SymbolTableValidatorVisitor : IVisitor
	{
		private readonly ExecutionContext context;

		public SymbolTableValidatorVisitor(ExecutionContext context)
        {
            this.context = context;
        }

        public void Visit(IExpression expression)
        {
            if (expression is VariableExpression)
            {
                var variableExpression = (VariableExpression)expression;

				if (!this.context.ContainsVariable(variableExpression.VariableValue.VariableName))
				{
					throw new Exception("Variable " + variableExpression.VariableValue.VariableName + " missing");
				}
            }
        }
	}

	class SymbolTableVisitor : IVisitor
    {

		private readonly ExecutionContext context;

		public SymbolTableVisitor(ExecutionContext context)
		{
			this.context = context;	
		}
        
		public void Visit(IExpression expression)
        {
            if (expression is VariableExpression)
			{
				var variableExpression = (VariableExpression)expression;

				var value = this.context.GetVariableValue(variableExpression.VariableValue.VariableName);

				variableExpression.EvaluatedValue = new NumberValue(value);            
			}
        }
	}

	class PrintingVisitor : IVisitor
    {
        public void Visit(IExpression expression)
        {
			if (expression is ConstantExpression contantExpression)
			{
				contantExpression.StringValue = contantExpression.Value.Value.ToString();
			}
			else if (expression is BinaryExpressionNode binaryExpression)
			{
				binaryExpression.StringValue = binaryExpression.StringFunction(binaryExpression.Left.StringValue, binaryExpression.Right.StringValue);
			}
			else if (expression is ExpressionNode expressionNode)
			{
				expressionNode.StringValue = String.Format("({0})", expressionNode.Expression.StringValue);
			}
		}
    }

	class ExcecutionVisitor : IVisitor
	{
		public void Visit(IExpression expression)
        {
			if (expression is ConstantExpression)
			{
				var contantExpression = (ConstantExpression)expression;

				contantExpression.EvaluatedValue = contantExpression.Value;
			}
			else if (expression is BinaryExpressionNode)
			{
				var binaryExpression = (BinaryExpressionNode)expression;

				binaryExpression.EvaluatedValue = binaryExpression.BinaryFunction(binaryExpression.Left.EvaluatedValue, binaryExpression.Right.EvaluatedValue);
			}
			else if (expression is ExpressionNode)
			{
				var expressionNode = (ExpressionNode)expression;

				expressionNode.EvaluatedValue = expressionNode.Expression.EvaluatedValue;
			}
          
        }
	}
     
    public class ExecutionEngine
    {

		readonly ExecutionContext context;

        public ExecutionEngine(ExecutionContext context)
        {
			this.context = context;
        }

        public String ExpressionToString(IExpression expression)
		{
			var SymbolTableValidatorVisitor = new SymbolTableValidatorVisitor(this.context);
            var PrintingVisitor = new PrintingVisitor();

			VisitExpressionNodes(expression, VisitingOrder.PostOrder, SymbolTableValidatorVisitor);
			VisitExpressionNodes(expression, VisitingOrder.PostOrder, PrintingVisitor);

            return expression.StringValue;
		}

        public IValue ExecuteExpression(IExpression expression)
		{
			var SymbolTableVisitor = new SymbolTableVisitor(this.context);
			var ExcecutionVisitor = new ExcecutionVisitor();

			VisitExpressionNodes(expression, VisitingOrder.PostOrder, SymbolTableVisitor);
			VisitExpressionNodes(expression, VisitingOrder.PostOrder, ExcecutionVisitor);

			return expression.EvaluatedValue;
		}

		internal void VisitExpressionNodes(IExpression expression, VisitingOrder order, IVisitor visitor)
		{
			if (expression is BinaryExpressionNode)
			{
				VisitExpressionNodes((BinaryExpressionNode)expression, order, visitor);
			}
			else if (expression is ExpressionNode)
			{
				VisitExpressionNodes((ExpressionNode)expression, order, visitor);
			}
            else
			{
				visitor.Visit(expression);
			}  
		}

		internal void VisitExpressionNodes(ExpressionNode expression, VisitingOrder order, IVisitor visitor)      
		{
			VisitExpressionNodes(expression.Expression, order, visitor);
			visitor.Visit(expression);         
		}

		internal void VisitExpressionNodes(BinaryExpressionNode binaryExpression, VisitingOrder order, IVisitor visitor)
		{
			if (order == VisitingOrder.PreOrder)
			{
				visitor.Visit(binaryExpression);
				VisitExpressionNodes(binaryExpression.Left, order, visitor);
				VisitExpressionNodes(binaryExpression.Right, order, visitor);            
			}
			else if (order == VisitingOrder.InOrder)
			{
                VisitExpressionNodes(binaryExpression.Left, order, visitor);
				visitor.Visit(binaryExpression);
                VisitExpressionNodes(binaryExpression.Right, order, visitor);      
			}
			else if (order == VisitingOrder.PostOrder)
            {
                VisitExpressionNodes(binaryExpression.Left, order, visitor);
                VisitExpressionNodes(binaryExpression.Right, order, visitor);
				visitor.Visit(binaryExpression);
            }
		}
    }
}
