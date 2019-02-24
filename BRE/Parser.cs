using System;
using System.Linq;
using System.Collections.Generic;

namespace BRE
{
    public class Parser
    {
		private readonly List<TextToken> OriginalTokens;
		private readonly List<IToken> Tokens;

		private Int32 TokenIndex;
        
        private IToken Token
		{
			get
			{
				return TokenIndex < Tokens.Count ? Tokens[TokenIndex] : null;
			}
		}
       
        public void Advance(TextTokenType? expectedType = null)
		{
			if (expectedType != null && Token.OriginalType != expectedType)
            {
                throw new Exception(String.Format("Expected token of type {0}", expectedType));
            }

			if (TokenIndex >= Tokens.Count - 1 )
				return;
			
			TokenIndex++;         
		}

        public Parser(List<TextToken> tokens)
        {
			this.OriginalTokens = tokens;
			this.Tokens = OriginalTokens.Select(this.CreateTokenFromTextToken)
				                        .ToList();
			TokenIndex = 0;
        }

        private IToken CreateTokenFromTextToken(TextToken token)
		{
			switch (token.Type)
			{
				case TextTokenType.Number:
					return new NumberToken(token, this);
				case TextTokenType.Word:
                    return new VariableToken(token, this);
				case TextTokenType.Operator:
					return new OperatorToken(token, this);
				case TextTokenType.ParentheseStart:
                    return new StartParentheseToken(token, this);
				case TextTokenType.ParentheseStop:
                    return new StopParentheseToken(token, this);
				case TextTokenType.TernaryEnd:
					return new EndTernaryToken(token, this);
				case TextTokenType.EndOfSentence:
					return new EndToken(token, this);
			}

			throw new NotSupportedException("Unsupported token type");

		}

		public IExpression ParseExpression(Int32 rightBindingPower = 0)
        {
            IToken token = this.Token;
            this.Advance();
            
			IExpression left = token.NullDetonation();

			while (rightBindingPower < this.Token.LeftBindingPower)
            {
                token = this.Token;
                this.Advance();
                left = token.LeftDetonation(left);
            }

            return left;
        }      
          
    }
}
