using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using triple.LEXER.HANDLERS;
using triple.INTERPRETER;

namespace triple.LEXER
{
    public class Lexer : ILexer
    {
        private string input;
        private int position;
        private char current;

        private readonly List<TokenHandler> handlers = new List<TokenHandler>();

        public Lexer()
        {
            // Важен порядок! Сначала CommentHandler потом SpecialHandler
            handlers.Add(new CommentHandler());
            handlers.Add(new SpecialHandler());
            handlers.Add(new WhitespaceHandler());
            handlers.Add(new ComparisonHandler());
            handlers.Add(new OperatorHandler());
            handlers.Add(new LiteralHandler());
            handlers.Add(new KeywordHandler());
        }

        public List<IToken> Tokenize(string input)
        {
            this.input = input;
            position = 0;
            current = input.Length > 0 ? input[0] : '\0';

            List<IToken> tokens = new List<IToken>();

            while (current != '\0')
            {
                bool matched = false;

                foreach (var handler in handlers)
                {
                    if (handler.TryHandle(ref current, ref position, input, tokens))
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    throw new Exception($"Unreсognized token on position {position}: {current}");
                }
            }

            tokens.Add(new Token(TokenType.EOF, null));
            return tokens;
        }
    }

}
