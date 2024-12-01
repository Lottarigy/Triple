using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.INTERPRETER
{
    public class Symbol<G>
    {
        private readonly string type;
        private readonly G value;

        public Symbol(string type, G value)
        {
            this.type = type;
            this.value = value;
        }

        public new string GetType()
        {
            return type;
        }

        public G GetValue()
        {
            return value;
        }
    }
}
