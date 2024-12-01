using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class InputNode : INode
    {
        //интерпретация в readline
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitInputNode(this);
        }
    }
}
