using System;
using System.Collections.Generic;

namespace BRE
{
	public interface IExpression
	{
		ExpressionType Type { get; }
		IValue Value { get; }
		IValue EvaluatedValue { get; }
		String StringValue { get; }

	}

	public enum ExpressionType
	{
		Variable,
		Operator,
		Constant,
		BinaryOperation,
		InfixrOperation,
		InfixOperation,
		Expression
	}

	public enum BinaryExpressionType
	{
		Add,
		Subtract,
		Divide,
		Multiply,
		And,
		Or,
		Not,
		GreaterThan,
		GreaterThanOrEqual,
		Equal,
		LessThan,
		LessThanOrEqual
	}

	public class ExpressionNode : IExpression
	{

		public ExpressionType Type { get; set; }
		public IValue Value { get; set; }

		public IValue EvaluatedValue { get; set; }

		public IExpression Expression;

		public String StringValue { get; set; }

		public ExpressionNode(IExpression expression)
		{
			this.Type = ExpressionType.Expression;
			this.Expression = expression;
		}
	}


	public class TernaryExpressionNode : IExpression
	{
		public ExpressionType Type { get; set; }
        public IValue Value { get; set; }
        public IValue EvaluatedValue { get; set; }
        public IExpression First { get; private set; }
        public IExpression Second { get; private set; }
		public IExpression Third { get; private set; }

        public String BinaryOperator { get; private set; }
        public String StringValue { get; set; }

		public TernaryExpressionNode(IExpression first, IExpression second, IExpression third)
		{
			this.First = first;
			this.Second = second;
			this.Third = third;
		}
	}

	public class BinaryExpressionNode : IExpression
	{

		public ExpressionType Type { get; set; }
		public IValue Value { get; set; }
		public IValue EvaluatedValue { get; set; }
		public BinaryExpressionType BinaryType { get; private set; }
		public IExpression Left { get; private set; }
		public IExpression Right { get; private set; }
		public String BinaryOperator { get; private set; }
		public String StringValue { get; set; }

		public String StringFunction(string left, string right)
		{
			return string.Format("{0} {1} {2}", left, this.BinaryOperator, right);
		}

		public virtual IValue BinaryFunction(IValue left, IValue right)
		{
			throw new Exception("Not implemented!");
		}

		public BinaryExpressionNode(IExpression left, IExpression right, BinaryExpressionType type, String binaryOperator)
		{
			this.Type = ExpressionType.BinaryOperation;
			this.BinaryType = type;
			this.Left = left;
			this.Right = right;
			this.BinaryOperator = binaryOperator;
		}
	}

	#region logical operations

	public class EqualExpressionNode : BinaryExpressionNode
    {

		public EqualExpressionNode(IExpression left, IExpression right)
			: base(left, right, BinaryExpressionType.Equal, "==")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new BooleanValue((double)left.Value == (double)right.Value);

            throw new Exception("Invalid types");

        }
    }

	public class LessThanExpressionNode : BinaryExpressionNode
    {

        public LessThanExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.LessThan, "<")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new BooleanValue((double)left.Value < (double)right.Value);

            throw new Exception("Invalid types");

        }
    }

    public class LessThanOrEqualExpressionNode : BinaryExpressionNode
    {

		public LessThanOrEqualExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.LessThanOrEqual, "<=")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new BooleanValue((double)left.Value <= (double)right.Value);

            throw new Exception("Invalid types");

        }
    }

	public class GreaterThanExpressionNode : BinaryExpressionNode
	{

		public GreaterThanExpressionNode(IExpression left, IExpression right)
			: base(left, right, BinaryExpressionType.GreaterThan, ">")
		{

		}

		public override IValue BinaryFunction(IValue left, IValue right)
		{
			if (left.Type == ValueType.Number && right.Type == ValueType.Number)
				return new BooleanValue((double)left.Value > (double)right.Value);

			throw new Exception("Invalid types");

		}
	}

	public class GreaterThanOrEqualExpressionNode : BinaryExpressionNode
    {

		public GreaterThanOrEqualExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.GreaterThanOrEqual, ">=")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new BooleanValue((double)left.Value >= (double)right.Value);

            throw new Exception("Invalid types");

        }
    }



	public class AndExpressionNode : BinaryExpressionNode
	{

		public AndExpressionNode(IExpression left, IExpression right)
			: base(left, right, BinaryExpressionType.And, "&&")
		{

		}

		public override IValue BinaryFunction(IValue left, IValue right)
		{
			if (left.Type == ValueType.Logical && right.Type == ValueType.Logical)
				return new BooleanValue((bool)left.Value && (bool)right.Value);

			throw new Exception("Invalid types");
		}
	}

	public class OrExpressionNode : BinaryExpressionNode
    {

		public OrExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.Or, "||")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Logical && right.Type == ValueType.Logical)
                return new BooleanValue((bool)left.Value || (bool)right.Value);

            throw new Exception("Invalid types");
        }
    }

    #endregion
	#region arithmetic operations


	public class MultiplyExpressionNode : BinaryExpressionNode
	{

		public MultiplyExpressionNode(IExpression left, IExpression right)
			: base(left, right, BinaryExpressionType.Multiply, "*")
		{

		}

		public override IValue BinaryFunction(IValue left, IValue right)
		{
			if (left.Type == ValueType.Number && right.Type == ValueType.Number)
				return new NumberValue((double)left.Value * (double)right.Value);

			throw new Exception("Invalid types");

		}
	}


	public class DivideExpressionNode : BinaryExpressionNode
    {

		public DivideExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.Divide, "/")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new NumberValue((double)left.Value / (double)right.Value);

            throw new Exception("Invalid types");

        }
    }

	public class SubtractExpressionNode : BinaryExpressionNode
    {

		public SubtractExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.Subtract, "-")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new NumberValue((double)left.Value - (double)right.Value);

            throw new Exception("Invalid types");

        }
    }

	public class AddExpressionNode : BinaryExpressionNode
    {

        public AddExpressionNode(IExpression left, IExpression right)
            : base(left, right, BinaryExpressionType.Add, "+")
        {

        }

        public override IValue BinaryFunction(IValue left, IValue right)
        {
            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                return new NumberValue((double)left.Value + (double)right.Value);

            throw new Exception("Invalid types");

        }
    }


#endregion


	public class ConstantExpression : IExpression
	{
		public ExpressionType Type { get; set; }
		public NumberValue NumberValue { get; set; }
		public IValue EvaluatedValue { get; set; }
		public String StringValue { get; set; }

		public IValue Value
		{
			get
			{
				return NumberValue;
			}
		}

		private NumberToken token;
		public ConstantExpression(NumberToken token)
		{
			this.token = token;
			NumberValue = (NumberValue)token.Value;
			Type = ExpressionType.Constant;
		}
	}

	public class VariableExpression : IExpression
	{
		public ExpressionType Type { get; set; }
		public VariableValue VariableValue { get; set; }
		public IValue EvaluatedValue { get; set; }

		public String StringValue
		{
			get
			{
				return this.VariableValue.VariableName;
			}
        }

		public IValue Value 
		{
			get
			{
				return VariableValue;
			}
		}

		private VariableToken token;
		public VariableExpression(VariableToken token)
        {
            this.token = token;
			VariableValue =  (VariableValue)token.Value;
            Type = ExpressionType.Variable;
        }
    }
}
