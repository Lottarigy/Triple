using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class LiteralHandler : TokenHandler
    {
        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            if (char.IsDigit(current))
            {
                StringBuilder number = new StringBuilder();
                while (char.IsDigit(current) || current == '.')
                {
                    number.Append(current);
                    Advance(ref current, ref position, input);
                }
                tokens.Add(new Token(TokenType.NML, number.ToString()));
                return true;
            }

            if (current == '"' || current == '\'')
            {
                char quote = current;
                Advance(ref current, ref position, input);

                StringBuilder literal = new StringBuilder();
                while (current != '\0' && current != quote)
                {
                    literal.Append(current);
                    Advance(ref current, ref position, input);
                }

                Advance(ref current, ref position, input); // Закрывающая кавычка
                tokens.Add(new Token(quote == '"' ? TokenType.SGL : TokenType.CHL, literal.ToString()));
                return true;
            }

            return false;
        }
    }
}
