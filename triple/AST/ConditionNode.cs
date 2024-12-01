using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class ConditionNode : IBinaryNode
    {
        private readonly string value;
        private readonly INode left;
        private readonly INode right;

        public ConditionNode(string value, INode left = null, INode right = null)
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
            visitor.VisitConditionNode(this);
        }
    }
}

