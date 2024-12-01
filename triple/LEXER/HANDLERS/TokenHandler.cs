using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public abstract class TokenHandler
    {
        public abstract bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens);

        protected void Advance(ref char current, ref int position, string input)
        {
            position++;
            current = position < input.Length ? input[position] : '\0';
        }
    }
}

