using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class OutputlnNode : INode
    {
        private readonly INode node;

        public OutputlnNode(INode node)
        {
            this.node = node;
        }

        public INode GetNode() => node;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitOutputlnNode(this);
        }
    }
}
