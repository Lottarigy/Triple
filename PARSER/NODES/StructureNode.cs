using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class StructureNode : INode
    {
        private readonly string name;
        private readonly Dictionary<string, FunctionNode> functions;

        public StructureNode(string name, List<FunctionNode> functions)
        {
            this.name = name;
            this.functions = new Dictionary<string, FunctionNode>();
            foreach (var function in functions)
            {
                this.functions[function.GetName()] = function;
            }
        }

        public string GetName() => name;
        public Dictionary<string, FunctionNode> GetFunctions() => functions;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitStructureNode(this);
        }
    }
}
