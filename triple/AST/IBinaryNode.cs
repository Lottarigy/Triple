using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.AST
{
    public interface IBinaryNode : INode
    {
        INode GetLeft();
        INode GetRight();
    }
}
