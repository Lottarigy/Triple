using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class FunctionNode : INode
    {
        private readonly string name;
        private readonly List<string> parametersNames;
        private readonly List<string> parameterTypes;
        private readonly TypeNode returnType;
        private readonly INode body;

        public FunctionNode(string name, List<string> parametersNames, List<string> parameterTypes, TypeNode returnType, INode body)
        {
            this.name = name;
            this.parametersNames = parametersNames;
            this.parameterTypes = parameterTypes;
            this.returnType = returnType;
            this.body = body;
        }

        public string GetName() => name;
        public List<string> GetParameters() => parametersNames;
        public List<string> GetParameterTypes() => parameterTypes;
        public TypeNode GetReturnType() => returnType;
        public INode GetBody() => body;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitFunctionNode(this);
        }
    }
}
