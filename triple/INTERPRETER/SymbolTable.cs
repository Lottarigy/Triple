using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.INTERPRETER
{
    public class SymbolTable<G>
    {
        private readonly Dictionary<string, Symbol<G>> symbolTable;
        public SymbolTable()
        {
            symbolTable = new Dictionary<string, Symbol<G>>();
        }

        public void Define(string varName, Symbol<G> symbol)
        {
            if (symbolTable.ContainsKey(varName))
            {
                symbolTable[varName] = symbol;
            }
            else
            {
                symbolTable.Add(varName, symbol);
            }
        }

        public Symbol<G> Lookup(string varName)
        {
            return symbolTable[varName];
        }

        public bool IsDefined(string varName)
        {
            return symbolTable.ContainsKey(varName);
        }
    }
}
