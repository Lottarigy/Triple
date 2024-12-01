using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using triple.INTERPRETER;
using triple.LEXER;
using triple.PARSER;
using triple.AST;

namespace triple
{
    class Program
    {
        public static void InterpreterBenchMark(string fileName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ILexer lexer = new Lexer();
            IParser parser = new Parser();
            IInterpreter interpreter = new Interpreter();
            //string fileName = "D:/test.trp";
            string code = File.ReadAllText(fileName);
            List<IToken> tokens = lexer.Tokenize(code);
            INode node = parser.Parse(tokens);
            interpreter.Interpret(node);

            stopwatch.Stop();
            Console.WriteLine((double)stopwatch.ElapsedMilliseconds / 1000 + " s");
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Select a file");
                return;
            }

            string fileName = args[0];

            try
            {
                InterpreterBenchMark(fileName);
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{fileName}' not found");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
    }
}
