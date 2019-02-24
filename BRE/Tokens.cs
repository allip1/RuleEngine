using System;
using System.Collections.Generic;

namespace BRE
{

    public interface IToken 
	{
		TextTokenType OriginalType
		{
			get;
		}
       
        Int32 LeftBindingPower { get; }
        Int32 RightBindingPower { get; }

		IExpression NullDetonation();

		IExpression LeftDetonation(IExpression expression);
        
	}

	public class Token : IToken
    {
		protected Parser Parser;

		public TextTokenType OriginalType { get { return this.OriginalToken.Type; }}


        private IValue value;
        public IValue Value
        {
            get
            {
                return (value ?? (value = parseRawDataToValue()));

            }
        }

        internal virtual IValue parseRawDataToValue()
        {
            return null;
        }

        protected readonly TextToken OriginalToken;

		public Token(TextToken token , Parser parser)
        {
            this.OriginalToken = token;
			this.Parser = parser;
        }
        
        public Int32 LeftBindingPower { get; protected set; }
        public Int32 RightBindingPower { get; protected set; }


        public virtual IExpression NullDetonation()
        {
            throw new NotImplementedException();
        }

        public virtual IExpression LeftDetonation(IExpression left)
        {
            throw new NotImplementedException();
        }

    }

	public class EndToken : Token
	{
		public EndToken(TextToken token, Parser parser) : base(token, parser)
		{
		}
	}

	public class NumberToken : Token
    {

        public NumberToken(TextToken token, Parser parser) 
			: base(token, parser)
        {
        }

        public override IExpression NullDetonation()
        {
            return new ConstantExpression(this);
        }

        internal override IValue parseRawDataToValue()
        {
            return new NumberValue(Convert.ToDouble(this.OriginalToken.RawDataString));
        }
    }

	public class VariableToken : Token
    {
		public VariableToken(TextToken token, Parser parser)
            : base(token, parser)
        {
        }

        public override IExpression NullDetonation()
        {
            return new VariableExpression(this);
        }

        internal override IValue parseRawDataToValue()
        {
            return new VariableValue(this.OriginalToken.RawDataString);
        }
    }
       
	public class EndTernaryToken : Token
    {
		public EndTernaryToken(TextToken token, Parser parser)
     : base(token, parser)
        {
        }
    }

    
	public class StopParentheseToken : Token
    {
		public StopParentheseToken(TextToken token, Parser parser)
     : base(token, parser)
        {
        }
    }

	public class StartParentheseToken : Token
	{
		public StartParentheseToken(TextToken token, Parser parser)
     : base(token, parser)
        {
        }

		public override IExpression NullDetonation()
        {
			var expression = new ExpressionNode(this.Parser.ParseExpression(0));
			this.Parser.Advance(TextTokenType.ParentheseStop);

			return expression;
        }

        internal override IValue parseRawDataToValue()
        {
            return new NumberValue(Convert.ToDouble(this.OriginalToken.RawDataString));
        }
	}

    public class OperatorToken : Token
    {
        
		private Dictionary<TokenOperator, int> OperatorPrecedence = new Dictionary<TokenOperator, int>()
        {
			{ TokenOperator.Ternary, 20 },

            { TokenOperator.Add, 50 },
			{ TokenOperator.Subtract, 50 },

			{ TokenOperator.Multiply, 60 },
			{ TokenOperator.Divide, 60 },

			{ TokenOperator.Or, 30 },
            { TokenOperator.And, 30 },

			{ TokenOperator.Equal, 40 },     
			{ TokenOperator.GreaterThan, 40 },      
			{ TokenOperator.GreaterThanOrEqual, 40 },       
			{ TokenOperator.LessThan, 40 },    
			{ TokenOperator.LessThanOrEqual, 40 }         

        };

		TokenOperator Operator;

        public OperatorToken(TextToken token, Parser parser)
            : base(token, parser)
        {
			this.Operator = (TokenOperator)token.Data;
			this.LeftBindingPower = this.OperatorPrecedence[this.Operator];
        }
        
        public override IExpression LeftDetonation(IExpression left)
        {
			
			switch(Operator)
			{
				case TokenOperator.Ternary:
            	    var first = left;
                    var second = this.Parser.ParseExpression(0);
                    this.Parser.Advance(TextTokenType.TernaryEnd);
                    var third = this.Parser.ParseExpression(0);
                    return new TernaryExpressionNode(first, second, third);    

				case TokenOperator.Add:
					return new AddExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower));
				case TokenOperator.Subtract:
                    return new SubtractExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower));
				case TokenOperator.Multiply:
                    return new MultiplyExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower));
				case TokenOperator.Divide:
                    return new DivideExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower));
				case TokenOperator.Equal:
					return new EqualExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.And:
                    return new AndExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.Or:
                    return new OrExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.GreaterThan:
                    return new GreaterThanExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.GreaterThanOrEqual:
                    return new GreaterThanOrEqualExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.LessThan:
                    return new LessThanExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
				case TokenOperator.LessThanOrEqual:
                    return new LessThanOrEqualExpressionNode(left, this.Parser.ParseExpression(this.LeftBindingPower - 1));
			}

			throw new Exception("Unsupported operator");
        }
    }
}
