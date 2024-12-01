﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST.LITERALS
{
    public class CharLiteralNode : INode
    {
        private readonly string value;

        public CharLiteralNode(string value)
        {
            this.value = value;
        }

        public string GetValue() => value;

        public void Accept(INodeVisitor visitor)
        {
            visitor.VisitCharNode(this);
        }
    }
}