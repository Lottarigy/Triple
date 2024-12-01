using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class WhitespaceHandler : TokenHandler
    {
        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            if (char.IsWhiteSpace(current))
            {
                while (char.IsWhiteSpace(current) && current != '\0')
                {
                    Advance(ref current, ref position, input);
                }
                return true;
            }
            return false;
        }
    }
}
