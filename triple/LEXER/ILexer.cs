using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER
{
    public interface ILexer
    {
        List<IToken> Tokenize(string input);
    }
}

