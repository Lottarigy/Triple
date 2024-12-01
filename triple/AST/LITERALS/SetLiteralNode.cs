using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST.LITERALS
{
    public class SetLiteralNode : INode
    {
        private readonly List<INode> elements;

        public SetLiteralNode(List<INode> elements)
        {
            this.elements = elements;
        }

        public List<INode> GetElements() => elements;

        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitSetLiteralNode(this);
        }
    }
}
