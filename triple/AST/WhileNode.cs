using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class WhileNode : IBinaryNode
    {
        private readonly ConditionNode condition;
        private readonly INode thenBody;

        public WhileNode(ConditionNode condition, INode thenBody)
        {
            this.condition = condition;
            this.thenBody = thenBody;
        }

        public string GetValue() => null;
        public INode GetLeft() => null;
        public ConditionNode GetCondition() => condition;
        public INode GetRight() => thenBody;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitWhileNode(this);
        }
    }
}
