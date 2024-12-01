using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class DeclarationNode : IBinaryNode
    {
        private readonly VariableNode variable;
        private readonly TypeNode type;
        private readonly INode initialValue;

        public DeclarationNode(VariableNode variable, TypeNode type, INode initialValue = null)
        {
            this.variable = variable;
            this.type = type;
            this.initialValue = initialValue;
        }

        public string GetValue() => null;
        public INode GetLeft() => (INode)variable;
        public INode GetRight() => (INode)type;
        public INode GetInitialValue() => initialValue;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitDeclarationNode(this);
        }
    }
}

