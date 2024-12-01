using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.PARSER.NODES;

namespace triple.INTERPRETER
{
    public interface IInterpreter
    {
        void Interpret(INode node);
    }
}
