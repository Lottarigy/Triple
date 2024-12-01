using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class ReturnNode : INode
    {
        private readonly INode expression;

        public ReturnNode(INode expression)
        {
            this.expression = expression;
        }

        public INode GetExpression() => expression;

        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitReturnNode(this);
        }
    }
}
