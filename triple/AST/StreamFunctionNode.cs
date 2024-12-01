using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class StreamFunctionNode : INode
    {
        private readonly TypeNode returnType;
        private readonly CompoundNode body;
        private readonly INode returnExpression;

        public StreamFunctionNode(TypeNode returnType, CompoundNode body, INode returnExpression = null)
        {
            this.returnType = returnType;
            this.body = body;
            this.returnExpression = returnExpression;
        }

        public TypeNode GetReturnType() => returnType;
        public CompoundNode GetBody() => body;
        public INode GetReturnExpression() => returnExpression;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitStreamFunctionNode(this);
        }
    }
}
