using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class FunctionCallNode : INode
    {
        private readonly string name;
        private readonly List<INode> arguments;
    
        public FunctionCallNode(string name, List<INode> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }
    
        public string GetName() => name;
        public List<INode> GetArguments() => arguments;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitFunctionCallNode(this);
        }
    }
}
