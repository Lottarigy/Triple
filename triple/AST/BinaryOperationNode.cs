using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class BinaryOperationNode : IBinaryNode
    {
        private readonly string value;
        private readonly INode left;
        private readonly INode right;

        public BinaryOperationNode(string value, INode left, INode right)
        {
            this.value = value;
            this.left = left;
            this.right = right;
        }

        public string GetValue() => value;
        public INode GetLeft() => left;
        public INode GetRight() => right;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitBinaryOperationNode(this);
        }
    }
}

