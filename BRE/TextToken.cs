using System;
using System.Collections.Generic;

namespace BRE
{
	public enum TextTokenType
	{
		Number,
        Word,
        ParentheseStart,
        ParentheseStop,
        TernaryEnd,
        Operator,
        EndOfSentence
	}

    public interface IValue
	{
		ValueType Type { get; }
		Object Value { get; }
	}

	public class BooleanValue : IValue
	{
		public BooleanValue(bool value)
		{
			this.value = value;
		}
        
		private bool value;
              
		public ValueType Type
        {
            get
            {
                return ValueType.Logical;
            }         
        }

		public Boolean LogicalValue
        {
            get
            {
                return this.value;
            }
        }

        public Object Value
        {
            get
            {
                return this.value;
            }
        }
	}

	public class StringValue : IValue
	{
		public StringValue(string value)
		{
			this.value = value;
		}

		private string value;

		public ValueType Type
        {
            get
            {
                return ValueType.String;
            }

        }

        public String ValueString
		{
			get
			{
				return this.value;
			}
		}

        public Object Value
		{
			get
			{
				return this.value;
			}
		}
	}

	public class NumberValue : IValue
	{

        public NumberValue(double value)
		{
			this.value = value;
		}

		public ValueType Type 
		{
			get 
			{
				return ValueType.Number;
			}

		}

		private double value;

        public Double DoubleValue 
		{
			get
			{
				return this.value;
			}
		}

		public Object Value
		{
			get
			{
				return this.value;
			}
		}
	}

	public class VariableValue : IValue
	{

        public VariableValue(String name)
		{
			this.VariableName = name;
		}

		public String VariableName { get; set; }

		ValueType type = ValueType.DontKnowYet;
		private const string initialValue = "DONT KNOW YET";
		Object value = initialValue;

		public ValueType Type
        {
            get
            {
				return type;
            }
        }
        
        public Object Value
        {
            get
            {
                return this.value;
            }
        }
	}
    
    public enum ValueType
	{
		DontKnowYet = 0,
		Number,
        Logical,
        String
	}
    
    public enum TokenOperator
	{
		Ternary,
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
  
	public class TextToken
	{
		public TextToken()
		{
			RawData = new List<char>();
		}
            
		public List<Char> RawData { get; set; }
		public String RawDataString 
		{ 
			get
			{
				return new String(this.RawData.ToArray());
			}
		}
		
		public TextTokenType Type { get; set; }

		private Object data;
              
		public Object Data
		{
			get
			{
				return (data ?? (data = parseRawData()));

			}
		}
              
		private Object parseRawData()
		{
			switch (this.Type){
				case TextTokenType.Number:
	               
					return Convert.ToDouble(this.RawDataString);

				case TextTokenType.Operator:

					switch (this.RawDataString) {
						case "?":
                            return TokenOperator.Ternary;
						case "+":
							return TokenOperator.Add;
						case "-":
							return TokenOperator.Subtract;
						case "*":
							return TokenOperator.Multiply;
						case "/":
							return TokenOperator.Divide;
						case "==":
                            return TokenOperator.Equal;
						case "&&":
							return TokenOperator.And;
						case "||":
                            return TokenOperator.Or;
						case ">":
                            return TokenOperator.GreaterThan;
						case ">=":
                            return TokenOperator.GreaterThanOrEqual;
						case "<":
                            return TokenOperator.LessThan;
						case "<=":
                            return TokenOperator.LessThanOrEqual;
					}
					throw new Exception("unsupported operator");

				case TextTokenType.ParentheseStart:
					return TextTokenType.ParentheseStart;
				case TextTokenType.ParentheseStop:
					return TextTokenType.ParentheseStop;
				case TextTokenType.TernaryEnd:
                    return TextTokenType.TernaryEnd;
	    		case TextTokenType.Word:
               
					return this.RawDataString;


				case TextTokenType.EndOfSentence:
					return TextTokenType.EndOfSentence;
			}

			throw new Exception("Unsupported object type");
		}
    }
}
