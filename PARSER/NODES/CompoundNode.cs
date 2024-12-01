using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class CompoundNode : INode
    {
        private readonly List<INode> children;

        public CompoundNode(List<INode> children)
        {
            this.children = children;
        }

        public List<INode> GetChildren() => children;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitCompoundNode(this);
        }
    }
}
