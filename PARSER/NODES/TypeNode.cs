using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.PARSER.NODES
{
    public class TypeNode : INode
    {
        private readonly string value;

        public TypeNode(string value)
        {
            this.value = value;
        }

        public string GetValue() => value;

        public void Accept(INodeVisitor visitor) { }
    }
}
