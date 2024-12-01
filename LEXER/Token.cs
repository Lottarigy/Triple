using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER
{
    public class Token : IToken
    {
        private readonly TokenType kind;
        private readonly string value;

        public Token(TokenType kind, string value)
        {
            this.kind = kind;
            this.value = value;
        }

        public new TokenType GetType() => kind;

        public string GetValue() => value;

        public override string ToString() => string.Format("Token({0}, {1})", kind.ToString(), value);

    }
}
