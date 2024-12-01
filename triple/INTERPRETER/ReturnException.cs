using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.INTERPRETER
{
    public class ReturnException : Exception
    {
        public object Value { get; }

        public ReturnException(object value)
        {
            Value = value;
        }
    }
}
