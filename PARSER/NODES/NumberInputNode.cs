using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class NumberInputNode : INode
    {
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitNumberInputNode(this);
        }
    }
}
