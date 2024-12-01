using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class AssignationNode : IBinaryNode
    {
        private readonly string value;
        private readonly VariableNode left;
        private readonly INode right;

        public AssignationNode(string value, VariableNode left, INode right)
        {
            this.value = value;
            this.left = left;
            this.right = right;
        }

        public string GetValue() => value;
        public INode GetLeft() => (INode)left;
        public INode GetRight() => right;
        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitAssignationNode(this);
        }
    }
}
