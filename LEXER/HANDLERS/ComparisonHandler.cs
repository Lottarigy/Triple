using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class ComparisonHandler : TokenHandler
    {
        private readonly Dictionary<string, TokenType> comparisons = new Dictionary<string, TokenType>
        {
            { "==", TokenType.EQL },
            { "!=", TokenType.NEQ },
            { ">=", TokenType.GEQ },
            { "<=", TokenType.LEQ },
            { ">", TokenType.GRT },
            { "<", TokenType.LSS }
        };

        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            // Проверяем сначала двусимвольные операторы
            foreach (var op in comparisons.Keys.Where(op => op.Length == 2))
            {
                if (input.Substring(position).StartsWith(op))
                {
                    tokens.Add(new Token(comparisons[op], op));
                    for (int i = 0; i < op.Length; i++)
                        Advance(ref current, ref position, input);
                    return true;
                }
            }

            // Проверяем односимвольные операторы
            foreach (var op in comparisons.Keys.Where(op => op.Length == 1))
            {
                if (input[position] == op[0])
                {
                    tokens.Add(new Token(comparisons[op], op));
                    Advance(ref current, ref position, input);
                    return true;
                }
            }

            return false;
        }
    }
}
