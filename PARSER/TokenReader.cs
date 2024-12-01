using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.LEXER;

namespace triple.PARSER
{
    public class TokenReader
    {
        private readonly IEnumerator<IToken> iterator = null;
        public IToken Current { get; private set; }

        public TokenReader(IEnumerable<IToken> tokens)
        {
            iterator = tokens.GetEnumerator();
            Advance();
        }

        public void Advance()
        {
            if (iterator.MoveNext())
                Current = iterator.Current;
        }

        public bool Match(TokenType expectedType)
        {
            return Current != null && Current.GetType() == expectedType;
        }

        public void Consume(TokenType expectedType)
        {
            if (!Match(expectedType))
                throw new Exception($"Token expected {expectedType}, but got {Current.GetType()}.");
            Advance();
        }

        public TokenType PeekNextType()
        {
            var tempIterator = iterator;
            tempIterator.MoveNext();
            return tempIterator.Current?.GetType() ?? TokenType.EOF;
        }

        public IToken PeekNext()
        {
            var tempIterator = iterator;
            tempIterator.MoveNext();
            return tempIterator.Current;
        }
    }
}
