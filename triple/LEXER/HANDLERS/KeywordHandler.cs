using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER.HANDLERS
{
    public class KeywordHandler : TokenHandler
    {
        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            //Русская локализация
            { "Поток",      TokenType.STM },
            { "число",      TokenType.NMD },
            { "строка",     TokenType.SGD },
            { "символ",     TokenType.CHD },
            { "логика",     TokenType.BLD },
            { "истина",     TokenType.BLL },
            { "ложь",       TokenType.BLL },
            { "множество",  TokenType.STD },
            { "количество", TokenType.STC },
            { "ничто",      TokenType.VDD },
            { "пусть",      TokenType.LET },
            { "если",       TokenType.TIF },
            { "инесли",     TokenType.ELI },
            { "иначе",      TokenType.ELS },
            { "пока",       TokenType.WHL },
            { "исполнять",  TokenType.TDO },
            { "вернуть",    TokenType.RTN },
            { "Вводлн",       TokenType.INP },
            { "Вывод",      TokenType.OUT },
            { "Выводлн",    TokenType.OTL },
            { "структура",  TokenType.STR },
            { "граница",    TokenType.BND },
            
            //Английская локализация
            { "Stream",     TokenType.STM },
            { "num",        TokenType.NMD },
            { "stg",        TokenType.SGD },
            { "chr",        TokenType.CHD },
            { "bool",       TokenType.BLD },
            { "tr",         TokenType.BLL },
            { "fls",        TokenType.BLL },
            { "set",        TokenType.STD },
            { "count",      TokenType.STC },
            { "void",       TokenType.VDD },
            { "let",        TokenType.LET },
            { "if",         TokenType.TIF },
            { "elif",       TokenType.ELI },
            { "else",       TokenType.ELS },
            { "while",      TokenType.WHL },
            { "do",         TokenType.TDO },
            { "return",     TokenType.RTN },
            { "Inputln",      TokenType.INP },
            { "Output",     TokenType.OUT },
            { "Outputln",   TokenType.OTL },
            { "struct",     TokenType.STR },
            { "bound",      TokenType.BND },

            //Беларуская локализация
            { "Струмень",   TokenType.STM },
            { "лік",        TokenType.NMD },
            { "радок",      TokenType.SGD },
            { "сімвал",     TokenType.CHD },
            { "логіка",     TokenType.BLD },
            { "ісціна",     TokenType.BLL },
            { "хлусня",     TokenType.BLL },
            { "мноства",    TokenType.STD },
            { "колькасць",  TokenType.STC },
            { "нішто",      TokenType.VDD },
            { "хай",        TokenType.LET },
            { "калі",       TokenType.TIF },
            { "ікалі",      TokenType.ELI },
            { "інакш",      TokenType.ELS },
            { "пакуль",     TokenType.WHL },
            { "выконваць",  TokenType.TDO },
            { "вярнуць",    TokenType.RTN },
            { "Уводлн",       TokenType.INP },
            { "Вывад",      TokenType.OUT },
            { "Вывадлн",    TokenType.OTL },
            { "канструкцыя",TokenType.STR },
            { "мяжа",       TokenType.BND },
        };

        public override bool TryHandle(ref char current, ref int position, string input, List<IToken> tokens)
        {
            if (char.IsLetter(current))
            {
                StringBuilder identifier = new StringBuilder();
                while (char.IsLetterOrDigit(current))
                {
                    identifier.Append(current);
                    Advance(ref current, ref position, input);
                }

                string value = identifier.ToString();
                if (keywords.TryGetValue(value, out var tokenType))
                {
                    tokens.Add(new Token(tokenType, value));
                }
                else
                {
                    tokens.Add(new Token(TokenType.TID, value));
                }
                return true;
            }
            return false;
        }
    }
}

