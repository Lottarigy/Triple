using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.LEXER;
using triple.AST;

namespace triple.PARSER
{
    public interface IParser
    {
        INode Parse(List<IToken> tokens);
    }
}

