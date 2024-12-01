using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class DoWhileNode : IBinaryNode
    {
        private readonly INode thenBody;
        private readonly ConditionNode condition;

        public DoWhileNode(INode thenBody, ConditionNode condition)
        {
            this.thenBody = thenBody;
            this.condition = condition;
        }

        public string GetValue() => null;
        public INode GetLeft() => null;
        public ConditionNode GetCondition() => condition;
        public INode GetRight() => thenBody;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitDoWhileNode(this);
        }
    }
}
