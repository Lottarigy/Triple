﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.INTERPRETER;

namespace triple.AST
{
    public interface INode : INodeVisitable
    {
        string GetValue();
    }
}
