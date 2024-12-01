using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class OperatorHandler : TokenHandler
    {
        private readonly Dictionary<string, TokenType> operators = new Dictionary<string, TokenType>
        {
            { "+", TokenType.PLS },
            { "-", TokenType.MIN },
            { "*", TokenType.MUL },
            { "/", TokenType.DIV },
            { "%", TokenType.MOD },
            { "^", TokenType.EXP },
            { "=", TokenType.ASN }
        };

        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            foreach (var op in operators)
            {
                if (input.Substring(position).StartsWith(op.Key))
                {
                    tokens.Add(new Token(op.Value, op.Key));
                    for (int i = 0; i < op.Key.Length; i++)
                        Advance(ref current, ref position, input);
                    return true;
                }
            }
            return false;
        }
    }
}
