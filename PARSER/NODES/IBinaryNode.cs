using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.PARSER.NODES
{
    public interface IBinaryNode : INode
    {
        INode GetLeft();
        INode GetRight();
    }
}
