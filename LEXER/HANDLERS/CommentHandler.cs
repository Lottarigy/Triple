using System;
using System.Collections.Generic;

namespace triple.LEXER.HANDLERS
{
    public class CommentHandler : TokenHandler
    {
        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            // Однострочный комментарий
            if (input.Substring(position).StartsWith("ком") ||
                input.Substring(position).StartsWith("com") ||
                input.Substring(position).StartsWith("кам"))
            {
                while (position < input.Length && input[position] != '\n')
                {
                    Advance(ref current, ref position, input);
                }
                // Пропускаем комментарий, ничего не добавляем в tokens
                return true;
            }

            // Многострочный комментарий
            if (input.Substring(position).StartsWith("--"))
            {
                position += 2; // Пропускаем начальные "--"
                while (position < input.Length - 1)
                {
                    if (input[position] == '-' && input[position + 1] == '-')
                    {
                        position += 2; // Пропускаем закрывающие "--"
                        Advance(ref current, ref position, input);
                        return true; // Завершаем обработку комментария
                    }
                    Advance(ref current, ref position, input);
                }

                // Если цикл завершился, а закрывающего "--" нет, выбрасываем ошибку
                throw new Exception($"An incomplete multiline comment starting at position {position - 2}.");
            }

            return false; // Если комментарий не распознан
        }
    }

}
