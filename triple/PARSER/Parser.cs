using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.LEXER;
using triple.AST;
using triple.AST.LITERALS;

namespace triple.PARSER
{
    public class Parser : IParser
    {
        private TokenReader reader = null;
        private readonly bool streamFunctionDeclared;
        private readonly Dictionary<string, string> types = null;

        public Parser()
        {
            streamFunctionDeclared = false;
            types = new Dictionary<string, string>();
        }

        public INode Parse(List<IToken> tokens)
        {
            reader = new TokenReader(tokens);
            return ParseProgram();
        }

        private INode ParseProgram()
        {
            var globalNodes = new List<INode>();
            StreamFunctionNode streamFunction = null;

            while (!reader.Match(TokenType.EOF))
            {
                INode node = ParseGlobalStatement();
                if (node is StreamFunctionNode stream)
                {
                    if (streamFunction != null)
                        throw new Exception("The stream has already been identified.");
                    streamFunction = stream;
                }
                else
                {
                    globalNodes.Add(node);
                }
            }

            if (streamFunction == null)
                throw new Exception("Stream not identified.");

            return new ProgramNode(globalNodes, streamFunction);
        }

        private TypeNode ParseType()
        {
            if (reader.Match(TokenType.NMD)) { reader.Consume(TokenType.NMD); return new TypeNode("num"); }
            if (reader.Match(TokenType.SGD)) { reader.Consume(TokenType.SGD); return new TypeNode("stg"); }
            if (reader.Match(TokenType.BLD)) { reader.Consume(TokenType.BLD); return new TypeNode("bool"); }
            if (reader.Match(TokenType.CHD)) { reader.Consume(TokenType.CHD); return new TypeNode("chr"); }
            if (reader.Match(TokenType.STD)) { reader.Consume(TokenType.STD); return new TypeNode("set"); }
            if (reader.Match(TokenType.VDD)) { reader.Consume(TokenType.VDD); return new TypeNode("void"); }

            throw new Exception($"Unexpected token {reader.Current.GetType()} in the type declaration.");
        }

        private VariableNode ParseVariable()
        {
            var value = reader.Current.GetValue();
            reader.Consume(TokenType.TID);
            return new VariableNode(value);
        }

        #region GLOBAL STATEMENTS
        private INode ParseGlobalStatement()
        {
            if (reader.Match(TokenType.CLN))
            {
                reader.Consume(TokenType.CLN);

                if (reader.Match(TokenType.STM))
                    return ParseStreamFunctionGStatement();

                if (reader.Match(TokenType.TID))
                    return ParseFunctionGStatement();
            }

            if (reader.Match(TokenType.DCL))
                return ParseStructureGStatement();

            throw new Exception("Unexpected global statement.");
        }

        private INode ParseStreamFunctionGStatement()
        {
            reader.Consume(TokenType.STM);
            reader.Consume(TokenType.LPN);
            reader.Consume(TokenType.RPN);
            reader.Consume(TokenType.VCT);

            var returnType = ParseType();
            reader.Consume(TokenType.ARW);

            var bodyStatements = new List<INode>();
            while (!reader.Match(TokenType.RTN))
            {
                bodyStatements.Add(ParseStatement());
            }

            INode returnExpression = null;
            if (reader.Match(TokenType.RTN))
            {
                reader.Consume(TokenType.RTN);
                returnExpression = ParseExpression();
            }

            return new StreamFunctionNode(returnType, new CompoundNode(bodyStatements), returnExpression);
        }

        private INode ParseStructureGStatement()
        {
            reader.Consume(TokenType.DCL);
            reader.Consume(TokenType.STR);
            var structName = reader.Current.GetValue();
            reader.Consume(TokenType.TID);

            var functions = new List<FunctionNode>();
            while (!reader.Match(TokenType.BND))
            {
                reader.Consume(TokenType.CLN);
                functions.Add((FunctionNode)ParseFunctionGStatement());
            }
            reader.Consume(TokenType.BND);

            return new StructureNode(structName, functions);
        }

        #region function
        private INode ParseFunctionGStatement()
        {
            var functionName = reader.Current.GetValue();
            reader.Consume(TokenType.TID);

            reader.Consume(TokenType.LPN);
            var parameterNames = new List<string>();
            var parameterTypes = new List<string>();

            while (!reader.Match(TokenType.RPN))
            {
                var parameterName = reader.Current.GetValue();
                reader.Consume(TokenType.TID);
                reader.Consume(TokenType.VCT);
                var parameterType = ParseType().GetValue();
                parameterNames.Add(parameterName);
                parameterTypes.Add(parameterType);

                if (reader.Match(TokenType.CMA))
                    reader.Consume(TokenType.CMA);
            }
            reader.Consume(TokenType.RPN);

            reader.Consume(TokenType.VCT);
            var returnType = ParseType();

            reader.Consume(TokenType.ARW);
            INode body = null;

            if (reader.Match(TokenType.VLN))
            {
                List<INode> statements = new List<INode>();

                while (reader.Match(TokenType.VLN))
                {
                    reader.Consume(TokenType.VLN);
                    statements.Add(ParseStatement());
                }
                body = new CompoundNode(statements);

                reader.Consume(TokenType.RSQ);

                // Проверка возврата
                if (returnType.GetValue() != "void" && !ContainsReturnStatement(body))
                {
                    throw new Exception($"Function '{functionName}' with type '{returnType.GetValue()}' should return value.");
                }

                return new FunctionNode(functionName, parameterNames, parameterTypes, returnType, body);
            }
            else
            {
                body = ParseStatement();

                // Проверка возврата
                if (returnType.GetValue() != "void" && !ContainsReturnStatement(body))
                {
                    throw new Exception($"Function '{functionName}' with type '{returnType.GetValue()}' should return value.");
                }

                return new FunctionNode(functionName, parameterNames, parameterTypes, returnType, body);
            }
        }
        private bool ContainsReturnStatement(INode body)
        {
            if (body is ReturnNode) return true;
            if (body is CompoundNode compound)
            {
                return compound.GetChildren().Any(ContainsReturnStatement);
            }
            return false;
        }
        #endregion

        #endregion

        #region STATEMENTS
        private INode ParseStatement()
        {
            if (reader.Match(TokenType.LET))
                return ParseDeclarationStatement();

            if (reader.Match(TokenType.RTN))
                return ParseReturnStatement();

            if (reader.Match(TokenType.FNC))
            {
                reader.Consume(TokenType.FNC);

                if (reader.Match(TokenType.OUT))
                    return ParseOutputStatement();

                if (reader.Match(TokenType.OTL))
                    return ParseOutputlnStatement();

                if (reader.Match(TokenType.TID))
                    return ParseFunctionCallStatement();

                throw new Exception("The function is not called completely");
            }

            if (reader.Match(TokenType.TIF))
                return ParseIfStatement();

            if (reader.Match(TokenType.WHL))
                return ParseWhileStatement();

            if (reader.Match(TokenType.TDO))
                return ParseDoWhileStatement();

            if (reader.Match(TokenType.SRC))
                return ParseStructureCallStatement();

            if (reader.Match(TokenType.LBC))
                return ParseSetExpression();

            return ParseAssignmentStatement(); // Все остальные инструкции считаются присваиванием
        }

        private StructureCallNode ParseStructureCallStatement()
        {
            reader.Consume(TokenType.SRC);
            var structName = reader.Current.GetValue();
            reader.Consume(TokenType.TID);
            reader.Consume(TokenType.FNC);

            var functionName = reader.Current.GetValue();
            reader.Consume(TokenType.TID);
            reader.Consume(TokenType.LPN);

            var arguments = new List<INode>();
            while (!reader.Match(TokenType.RPN))
            {
                arguments.Add(ParseExpression());
                if (reader.Match(TokenType.CMA))
                    reader.Consume(TokenType.CMA);
            }
            reader.Consume(TokenType.RPN);

            return new StructureCallNode(structName, functionName, arguments);
        }

        private FunctionCallNode ParseFunctionCallStatement()
        {
            var functionName = reader.Current.GetValue();
            reader.Consume(TokenType.TID);
            reader.Consume(TokenType.LPN);

            var arguments = new List<INode>();
            while (!reader.Match(TokenType.RPN))
            {
                arguments.Add(ParseExpression());
                if (reader.Match(TokenType.CMA))
                    reader.Consume(TokenType.CMA);
            }
            reader.Consume(TokenType.RPN);

            return new FunctionCallNode(functionName, arguments);
        }

        private ReturnNode ParseReturnStatement()
        {
            reader.Consume(TokenType.RTN);

            var expression = ParseExpression(); // Парсим возвращаемое выражение
            return new ReturnNode(expression);
        }

        private IfNode ParseIfStatement()
        {
            reader.Consume(TokenType.TIF);
            var condition = ParseCondition();
            reader.Consume(TokenType.ARW);

            INode thenBody = null;
            List<(ConditionNode, INode)> elseIfBlocks = new List<(ConditionNode, INode)>();
            INode elseBody = null;

            if (reader.Match(TokenType.VLN))// Множественное утверждение условия
            {
                List<INode> ifStatements = new List<INode>();
                while (reader.Match(TokenType.VLN))
                {
                    reader.Consume(TokenType.VLN);
                    ifStatements.Add(ParseStatement());
                }
                reader.Consume(TokenType.RSQ);
                thenBody = new CompoundNode(ifStatements);

                while (reader.Match(TokenType.ELI))
                {
                    reader.Consume(TokenType.ELI);
                    ConditionNode elseIfCondition = ParseCondition();
                    reader.Consume(TokenType.ARW);

                    if (reader.Match(TokenType.VLN))
                    {
                        List<INode> elseIfStatements = new List<INode>();
                        while (reader.Match(TokenType.VLN))
                        {
                            reader.Consume(TokenType.VLN);
                            elseIfStatements.Add(ParseStatement());
                        }
                        elseIfBlocks.Add((elseIfCondition, new CompoundNode(elseIfStatements)));
                        reader.Consume(TokenType.RSQ);
                    }
                    else
                    {
                        INode node = ParseStatement();
                        elseIfBlocks.Add((elseIfCondition, node));
                    }
                }

                if (reader.Match(TokenType.ELS))
                {
                    reader.Consume(TokenType.ELS);
                    reader.Consume(TokenType.ARW);

                    if (reader.Match(TokenType.VLN))
                    {
                        List<INode> elseStatements = new List<INode>();
                        while (reader.Match(TokenType.VLN))
                        {
                            reader.Consume(TokenType.VLN);
                            elseStatements.Add(ParseStatement());
                        }
                        elseBody = new CompoundNode(elseStatements);
                        reader.Consume(TokenType.RSQ);
                    }
                    else
                    {
                        INode node = ParseStatement();
                        elseBody = node;
                    }
                }

                return new IfNode(condition, thenBody, elseIfBlocks, elseBody);
            }
            else// Одиночное утверждение условия
            {
                thenBody = ParseStatement();

                while (reader.Match(TokenType.ELI))
                {
                    reader.Consume(TokenType.ELI);
                    ConditionNode elseIfCondition = ParseCondition();
                    reader.Consume(TokenType.ARW);

                    if (reader.Match(TokenType.VLN))
                    {
                        List<INode> elseIfStatements = new List<INode>();
                        while (reader.Match(TokenType.VLN))
                        {
                            reader.Consume(TokenType.VLN);
                            elseIfStatements.Add(ParseStatement());
                        }
                        elseIfBlocks.Add((elseIfCondition, new CompoundNode(elseIfStatements)));
                        reader.Consume(TokenType.RSQ);
                    }
                    else
                    {
                        INode node = ParseStatement();
                        elseIfBlocks.Add((elseIfCondition, node));
                    }
                }

                if (reader.Match(TokenType.ELS))
                {
                    reader.Consume(TokenType.ELS);
                    reader.Consume(TokenType.ARW);

                    if (reader.Match(TokenType.VLN))
                    {
                        List<INode> elseStatements = new List<INode>();
                        while (reader.Match(TokenType.VLN))
                        {
                            reader.Consume(TokenType.VLN);
                            elseStatements.Add(ParseStatement());
                        }
                        elseBody = new CompoundNode(elseStatements);
                        reader.Consume(TokenType.RSQ);
                    }
                    else
                    {
                        INode node = ParseStatement();
                        elseBody = node;
                    }
                }

                return new IfNode(condition, thenBody, elseIfBlocks, elseBody);
            }
        }

        private WhileNode ParseWhileStatement()
        {
            reader.Consume(TokenType.WHL);
            var condition = ParseCondition();
            reader.Consume(TokenType.ARW);
            INode body = null;

            if (reader.Match(TokenType.VLN))
            {
                List<INode> whileStatements = new List<INode>();
                while (reader.Match(TokenType.VLN))
                {
                    reader.Consume(TokenType.VLN);
                    whileStatements.Add(ParseStatement());
                }
                body = new CompoundNode(whileStatements);

                reader.Consume(TokenType.RSQ);

                return new WhileNode(condition, body);
            }
            else
            {
                body = ParseStatement();
                return new WhileNode(condition, body);
            }
        }

        private DoWhileNode ParseDoWhileStatement()
        {
            reader.Consume(TokenType.TDO);
            reader.Consume(TokenType.ARW);
            INode body = null;

            if (reader.Match(TokenType.VLN))
            {
                List<INode> doWhileStatements = new List<INode>();
                while (reader.Match(TokenType.VLN))
                {
                    reader.Consume(TokenType.VLN);
                    doWhileStatements.Add(ParseStatement());
                }
                body = new CompoundNode(doWhileStatements);

                reader.Consume(TokenType.RSQ);

                reader.Consume(TokenType.WHL);
                ConditionNode condition = ParseCondition();
                reader.Consume(TokenType.RSQ);

                return new DoWhileNode(body, condition);
            }
            else
            {
                body = ParseStatement();
                reader.Consume(TokenType.WHL);
                ConditionNode condition = ParseCondition();
                reader.Consume(TokenType.RSQ);
                return new DoWhileNode(body, condition);
            }
        }

        private OutputNode ParseOutputStatement()
        {
            reader.Consume(TokenType.OUT);
            reader.Consume(TokenType.LPN);

            var expression = ParseExpression();
            reader.Consume(TokenType.RPN);

            return new OutputNode(expression);
        }

        private OutputlnNode ParseOutputlnStatement()
        {
            reader.Consume(TokenType.OTL);
            reader.Consume(TokenType.LPN);

            var expression = ParseExpression();
            reader.Consume(TokenType.RPN);

            return new OutputlnNode(expression);
        }

        private DeclarationNode ParseDeclarationStatement()
        {
            reader.Consume(TokenType.LET);
            var variable = ParseVariable();
            reader.Consume(TokenType.VCT);
            var type = ParseType();

            INode initializer = null;
            if (reader.Match(TokenType.ASN))
            {
                reader.Consume(TokenType.ASN);
                initializer = ParseExpression();
            }

            types[variable.GetValue()] = type.GetValue();

            return new DeclarationNode(variable, type, initializer);
        }

        private AssignationNode ParseAssignmentStatement()
        {
            var variable = ParseVariable();
            var operatorToken = reader.Current;

            reader.Consume(TokenType.ASN);

            var expression = ParseExpression();
            return new AssignationNode(operatorToken.GetValue(), variable, expression);
        }

        private ConditionNode ParseCondition()
        {
            var left = ParseExpression();

            var operatorToken = reader.Current;
            if (!IsComparisonOperator(operatorToken.GetType()))
                throw new Exception("Expected comparison operator.");
            reader.Consume(operatorToken.GetType());

            var right = ParseExpression();
            return new ConditionNode(operatorToken.GetValue(), left, right);
        }

        private bool IsComparisonOperator(TokenType type)
        {
            return type == TokenType.EQL || type == TokenType.NEQ ||
                   type == TokenType.LSS || type == TokenType.GRT ||
                   type == TokenType.LEQ || type == TokenType.GEQ;
        }
        #endregion

        #region EXPRESSIONS
        private INode ParseExpression()
        {
            if (reader.Match(TokenType.NML) || reader.Match(TokenType.TID))
                return ParseNumberExpression();
            if (reader.Match(TokenType.SGL))
                return ParseStringExpression();
            if (reader.Match(TokenType.CHL))
                return ParseCharExpression();
            if (reader.Match(TokenType.BLL))
                return ParseBoolExpression();
            if (reader.Match(TokenType.LBC))
                return ParseSetExpression();
            if (reader.Match(TokenType.NMD))
                return ParseNumberInputExpression();

            if (reader.Match(TokenType.FNC))
            {
                reader.Consume(TokenType.FNC);
                if (reader.Match(TokenType.INP)) return ParseInputExpression();
                else if (reader.Match(TokenType.TID)) return ParseFunctionCallStatement();
                else throw new Exception("The function is not called completely");
            }

            throw new Exception($"Unexpected token {reader.Current.GetType()} in expression.");
        }

        private INode ParseNumberExpression()
        {
            var node = ParseNumberTerm();

            while (reader.Match(TokenType.PLS) || reader.Match(TokenType.MIN))
            {
                var operatorToken = reader.Current;
                reader.Consume(operatorToken.GetType());
                node = new BinaryOperationNode(operatorToken.GetValue(), node, ParseNumberTerm());
            }

            return node;
        }

        private INode ParseNumberTerm()
        {
            var node = ParseNumberFactor();

            while (reader.Match(TokenType.MUL) || reader.Match(TokenType.DIV) ||
                   reader.Match(TokenType.MOD) || reader.Match(TokenType.EXP))
            {
                var operatorToken = reader.Current;
                reader.Consume(operatorToken.GetType());
                node = new BinaryOperationNode(operatorToken.GetValue(), node, ParseNumberFactor());
            }

            return node;
        }

        private INode ParseNumberFactor()
        {
            if (reader.Match(TokenType.NML))
            {
                var value = reader.Current.GetValue();
                reader.Consume(TokenType.NML);
                return new NumberLiteralNode(value);
            }

            if (reader.Match(TokenType.TID))
            {
                var variable = ParseVariable();

                // Проверяем на доступ к коллекции множества
                if (reader.Match(TokenType.LSQ))
                    return ParseSetCallExpression(variable);

                return variable;
            }

            if (reader.Match(TokenType.LPN))
            {
                reader.Consume(TokenType.LPN);
                var expression = ParseExpression();
                reader.Consume(TokenType.RPN);
                return expression;
            }

            throw new Exception($"Unexpected token {reader.Current.GetType()} in number factor.");
        }

        private INode ParseStringExpression()
        {
            var node = ParseStringTerm();

            while (reader.Match(TokenType.PLS))
            {
                var operatorToken = reader.Current;
                reader.Consume(operatorToken.GetType());
                node = new BinaryOperationNode(operatorToken.GetValue(), node, ParseStringExpression());
            }

            return node;
        }

        private INode ParseStringTerm()
        {
            if (reader.Match(TokenType.SGL))
            {
                var value = reader.Current.GetValue();
                reader.Consume(TokenType.SGL);
                return new StringLiteralNode(value);
            }

            if (reader.Match(TokenType.TID))
            {
                return ParseVariable();
            }

            throw new Exception($"Unexpected token {reader.Current.GetType()} in string term.");
        }

        private BoolLiteralNode ParseBoolExpression()
        {
            var value = reader.Current.GetValue();
            reader.Consume(TokenType.BLL);
            return new BoolLiteralNode(value);
        }

        private CharLiteralNode ParseCharExpression()
        {
            var value = reader.Current.GetValue();
            reader.Consume(TokenType.CHL);
            return new CharLiteralNode(value);
        }

        private SetLiteralNode ParseSetExpression()
        {
            reader.Consume(TokenType.LBC);

            var elements = new List<INode>();
            if (!reader.Match(TokenType.RBC))
            {
                elements.Add(ParseExpression());
                while (reader.Match(TokenType.CMA))
                {
                    reader.Consume(TokenType.CMA);
                    elements.Add(ParseExpression());
                }
            }

            reader.Consume(TokenType.RBC);
            return new SetLiteralNode(elements);
        }

        private SetCallNode ParseSetCallExpression(VariableNode setVariable)
        {
            reader.Consume(TokenType.LSQ);
            if (reader.Match(TokenType.STC))
            {
                reader.Consume(TokenType.STC);
                reader.Consume(TokenType.RSQ);
                return new SetCallNode(setVariable, true);
            }

            var indexExpression = ParseExpression();
            reader.Consume(TokenType.RSQ);
            return new SetCallNode(setVariable, new NumberLiteralNode(indexExpression.GetValue()));
        }

        private InputlnNode ParseInputExpression()
        {
            reader.Consume(TokenType.INP);
            reader.Consume(TokenType.LPN);
            reader.Consume(TokenType.RPN);

            return new InputlnNode();
        }

        private NumberInputlnNode ParseNumberInputExpression()
        {
            reader.Consume(TokenType.NMD);
            reader.Consume(TokenType.LPN);
            reader.Consume(TokenType.FNC);
            reader.Consume(TokenType.INP);
            reader.Consume(TokenType.LPN);
            reader.Consume(TokenType.RPN);
            reader.Consume(TokenType.RPN);

            return new NumberInputlnNode();
        }
        #endregion
    }
}
