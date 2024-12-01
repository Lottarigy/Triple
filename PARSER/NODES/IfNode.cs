using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class IfNode : IBinaryNode
    {
        private readonly ConditionNode condition;
        private readonly INode thenBody;
        private readonly List<(ConditionNode, INode)> elseIfBlocks;
        private readonly INode elseBody;

        public IfNode(ConditionNode condition, INode thenBody, List<(ConditionNode, INode)> elseIfBlocks = null, INode elseBody = null)
        {
            this.condition = condition;
            this.thenBody = thenBody;
            this.elseIfBlocks = elseIfBlocks ?? new List<(ConditionNode, INode)>();
            this.elseBody = elseBody;
        }

        public string GetValue() => null;
        public INode GetLeft() => null;
        public ConditionNode GetCondition() => condition;
        public INode GetRight() => thenBody;

        public List<(ConditionNode, INode)> GetElseIfBlocks() => elseIfBlocks;
        public INode GetElseBody() => elseBody;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitIfNode(this);
        }
    }
}
