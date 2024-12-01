using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;
using triple.PARSER.NODES.LITERALS;

namespace triple.PARSER.NODES
{
    public class SetCallNode : INode
    {
        private readonly INode setNode;
        private readonly NumberLiteralNode index;
        private readonly bool isCountOperation;

        // Конструктор для вызова элемента массива
        public SetCallNode(INode setNode, NumberLiteralNode index)
        {
            this.setNode = setNode;
            this.index = index;
            isCountOperation = false;
        }

        // Конструктор для вызова length
        public SetCallNode(INode setNode, bool isCountOperation)
        {
            this.setNode = setNode;
            index = null;
            this.isCountOperation = isCountOperation;
        }

        public INode GetSetNode() => setNode;
        public NumberLiteralNode GetIndex() => index;
        public bool IsCountOperation() => isCountOperation;

        public string GetValue() => null;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitSetCallNode(this);
        }
    }
}
