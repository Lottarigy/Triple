using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class StructureCallNode : INode
    {
        private readonly string structName;
        private readonly string functionName;
        private readonly List<INode> arguments;

        public StructureCallNode(string structName, string functionName, List<INode> arguments)
        {
            this.structName = structName;
            this.functionName = functionName;
            this.arguments = arguments;
        }

        public string GetStructName() => structName;
        public string GetFunctionName() => functionName;
        public List<INode> GetArguments() => arguments;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitStructureCallNode(this);
        }
    }
}
