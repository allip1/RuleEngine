using System;
using System.Collections.Generic;

namespace BRE
{
    public class TextTokenizer
    {
        public TextTokenizer()
        {
        }

		public static List<TextToken> TextTokenize(String text)
        {
            List<TextToken> tokens = new List<TextToken>();

            int textLength = text.Length;

            int index = 0;

            //reading character by character until the end of input
            while (index < textLength)
            {

                //if space or tab, just skip
                if (char.IsWhiteSpace(text[index]))
                {
                    index++;

                }
				// if starts with ":" then its ternary ending
				else if (text[index] == ':')
                {
                    TextToken token = new TextToken();
                    token.Type = TextTokenType.TernaryEnd;
					token.RawData.Add(':');
                    tokens.Add(token);
                    index++;

                }
                // if starts with "(" then pick that up and continue
                else if (text[index] == '(')
                {
                    TextToken token = new TextToken();
                    token.Type = TextTokenType.ParentheseStart;
                    token.RawData.Add('(');
                    tokens.Add(token);
                    index++;
                    
                }
                // if starts with "(" then pick that up and continue
                else if (text[index] == ')')
                {
                    TextToken token = new TextToken();
                    token.Type = TextTokenType.ParentheseStop;
                    token.RawData.Add(')');
                    tokens.Add(token);
                    index++;

                }
                //if token starts with letter, and continues
                //with letter or digit, then its a word
                else if (char.IsLetter(text[index]))
                {

                    TextToken token = new TextToken();
                    token.Type = TextTokenType.Word;

                    do
                    {
                        token.RawData.Add(text[index]);
                        index++;
                    } while (index < textLength && char.IsLetterOrDigit(text[index]));

                    tokens.Add(token);

                }
                //if token starts with number, then its float or int
                else if (char.IsDigit(text[index]))
                {

                    TextToken token = new TextToken();
					token.Type = TextTokenType.Number;

                    do
                    {
                        token.RawData.Add(text[index]);
                        index++;
                    } while (index < textLength && char.IsDigit(text[index]));

                    //if next is "." then its float
                    if (index < textLength && text[index] == '.')
                    {
                        token.RawData.Add('.');
                        index++;

                        do
                        {
                            token.RawData.Add(text[index]);
                            index++;
                        } while (index < textLength && char.IsDigit(text[index]));

                    }
                      
                    tokens.Add(token);

                }
                //if dual operator ==, <=, >=, &&, ||
                else if (IsDualOperator(text, index))
				{
					TextToken token = new TextToken();
                    token.Type = TextTokenType.Operator;
                    token.RawData.Add(text[index]);
                    index++;
					token.RawData.Add(text[index]);
                    tokens.Add(token);
                    index++;
                    
				}
					
                //if is operator such as "+-/*" then pick it up
                else if (IsOperator(text[index]))
                {
                    TextToken token = new TextToken();
                    token.Type = TextTokenType.Operator;
                    token.RawData.Add(text[index]);
                    tokens.Add(token);
                    index++;

                }
    
                //picking end of sentence
                else if (text[index] == ';')
                {
                    TextToken token = new TextToken();
                    token.Type = TextTokenType.EndOfSentence;
                    token.RawData.Add(';');
                    tokens.Add(token);
                    index++;
                }

                else
                {
                    throw new Exception(string.Format("Not valid token {0}  at {1}", text[index], index));
                }

            }

            return tokens;

        }

        private static bool IsDualOperator(string text, int index)
		{
			return IsDualOperatorSymbol(text[index])
			                	&& index < text.Length - 1
								&& IsDualOperatorSymbol(text[index + 1]); 
		}

        private static bool IsDualOperatorSymbol(char c)
		{
			return c == '=' || c == '<' || c == '>' || c == '&' || c == '|';
		}


        private static bool IsOperator(char c)
        {
			return c == '+' || c == '-' || c == '*' || c == '/' || c == '<' || c == '>' || c == ':' || c == '?';
        }
    }
}
