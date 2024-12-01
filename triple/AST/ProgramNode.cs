using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public class ProgramNode : INode
    {
        private readonly List<INode> globalNodes;
        private readonly StreamFunctionNode streamFunction;

        public ProgramNode(List<INode> globalNodes, StreamFunctionNode streamFunction)
        {
            this.globalNodes = globalNodes;
            this.streamFunction = streamFunction;
        }

        public List<INode> GetGlobalNodes() => globalNodes;
        public StreamFunctionNode GetStreamFunction() => streamFunction;
        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitProgramNode(this);
        }
    }
}
