using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST.LITERALS
{
    public class StringLiteralNode : INode
    {
        private readonly string value;

        public StringLiteralNode(string value)
        {
            this.value = value;
        }

        public string GetValue() => value;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitStringNode(this);
        }
    }
}
