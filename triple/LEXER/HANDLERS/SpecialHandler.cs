using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class SpecialHandler : TokenHandler
    {
        private readonly Dictionary<string, TokenType> specialTokens = new Dictionary<string, TokenType>
        {
            { ">>", TokenType.FNC },
            { "<<", TokenType.SRC },
            { "->", TokenType.VCT },
            { "=>", TokenType.ARW },
            { "::", TokenType.DCL },
            { ":", TokenType.CLN },
            { "|", TokenType.VLN },
            { "[", TokenType.LSQ },
            { "]", TokenType.RSQ },
            { "{", TokenType.LBC },
            { "}", TokenType.RBC },
            { "(", TokenType.LPN },
            { ")", TokenType.RPN },
            { ".", TokenType.DOT },
            { ",", TokenType.CMA }
        };

        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            // Проверяем сначала двусимвольные токены
            foreach (var token in specialTokens.Keys.Where(key => key.Length == 2))
            {
                if (input.Substring(position).StartsWith(token))
                {
                    tokens.Add(new Token(specialTokens[token], token));
                    for (int i = 0; i < token.Length; i++)
                        Advance(ref current, ref position, input);
                    return true;
                }
            }

            // Проверяем односимвольные токены
            foreach (var token in specialTokens.Keys.Where(key => key.Length == 1))
            {
                if (current == token[0])
                {
                    tokens.Add(new Token(specialTokens[token], token));
                    Advance(ref current, ref position, input);
                    return true;
                }
            }

            return false;
        }
    }
}

