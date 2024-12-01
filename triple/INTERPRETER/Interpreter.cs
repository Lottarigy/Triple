using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.LEXER;
using triple.AST;
using triple.AST.LITERALS;

namespace triple.INTERPRETER
{
    class Interpreter : IInterpreter, INodeVisitor
    {
        private SymbolTable<object> symbolTable = new SymbolTable<object>();
        private readonly Stack<Symbol<object>> stack = new Stack<Symbol<object>>();
        private readonly Dictionary<string, FunctionNode> functionTable = new Dictionary<string, FunctionNode>();
        private FunctionNode currentExecutingFunction;

        public void Interpret(INode node)
        {
            node.Accept(this);
        }

        public void VisitProgramNode(ProgramNode node)
        {
            // Сначала интерпретируем глобальные функции и структуры
            foreach (var globalNode in node.GetGlobalNodes())
            {
                globalNode.Accept(this);
            }

            // Затем выполняем основной поток
            node.GetStreamFunction().Accept(this);
        }

        private bool TypesMatch(string expectedType, string actualType)
        {
            if (expectedType == "num" && actualType == "num") return true;
            if (expectedType == "stg" && actualType == "stg") return true;
            if (expectedType == "bool" && actualType == "bool") return true;
            if (expectedType == "chr" && actualType == "chr") return true;
            if (expectedType == "set" && actualType == "set") return true;
            if (expectedType == "void" && actualType == "void") return true;
            return false; // Если типы не совпадают
        }

        private string GetTypeName(object value)
        {
            if (value is int || value is double) return "num";
            if (value is string) return "stg";
            if (value is bool) return "bool";
            if (value is char) return "chr";
            if (value == "void") return "void";
            if (value is List<object>) return "set";
            throw new Exception($"Unexpected return type: {value.GetType().Name}");
        }

        #region GLOBAL STATEMENTS
        public void VisitStreamFunctionNode(StreamFunctionNode node)
        {
            // Выполняем тело потока
            node.GetBody().Accept(this);

            // Если есть возврат значения
            if (node.GetReturnExpression() != null)
            {
                node.GetReturnExpression().Accept(this);
                var returnValue = stack.Pop().GetValue();

                // Проверяем, соответствует ли возвращаемое значение ожидаемому типу
                string expectedType = node.GetReturnType().GetValue();
                string actualType = GetTypeName(returnValue);

                if (!TypesMatch(expectedType, actualType))
                {
                    throw new Exception($"Type mismatch in function: expected {expectedType}, but got {actualType}.");

                }

                Console.WriteLine();
                Console.WriteLine($"The program exited with the code: {returnValue}");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"The program exited with the code: {0}");
            }
        }

        public void VisitStructureNode(StructureNode node)
        {
            symbolTable.Define(node.GetName(), new Symbol<object>("struct", node.GetFunctions()));
        }

        public void VisitFunctionNode(FunctionNode node)
        {
            // Сохраняем функцию в таблице
            functionTable[node.GetName()] = node;
        }
        #endregion

        #region STATEMENTS
        public void VisitStructureCallNode(StructureCallNode node)
        {
            // Получаем структуру из таблицы символов
            var structSymbol = symbolTable.Lookup(node.GetStructName());
            if (structSymbol == null || structSymbol.GetType() != "struct")
            {
                throw new Exception($"Unidentified structure '{node.GetStructName()}'");
            }

            // Проверяем наличие функции в структуре
            var structFunctions = (Dictionary<string, FunctionNode>)structSymbol.GetValue();
            if (!structFunctions.ContainsKey(node.GetFunctionName()))
            {
                throw new Exception($"Unidentified function '{node.GetFunctionName()}' in structure '{node.GetStructName()}'.");
            }

            // Выполняем функцию
            var function = structFunctions[node.GetFunctionName()];

            // Создаем временную таблицу для аргументов функции
            var previousSymbolTable = symbolTable;
            symbolTable = new SymbolTable<object>();

            var parameters = function.GetParameters();
            if (parameters.Count != node.GetArguments().Count)
            {
                throw new Exception($"Function '{node.GetFunctionName()}' in structure '{node.GetStructName()}' expected {parameters.Count} arguments.");
            }

            // Сопоставляем аргументы с параметрами
            for (int i = 0; i < parameters.Count; i++)
            {
                node.GetArguments()[i].Accept(this);
                var argumentValue = stack.Pop().GetValue();
                symbolTable.Define(parameters[i], new Symbol<object>("argument", argumentValue));
            }

            // Выполняем тело функции
            try
            {
                function.GetBody().Accept(this);
            }
            catch (ReturnException ex)
            {
                // Обрабатываем возврат значения, если есть
                stack.Push(new Symbol<object>("return", ex.Value));
            }
            finally
            {
                // Возвращаем предыдущую таблицу символов
                symbolTable = previousSymbolTable;
            }
        }

        public void VisitFunctionCallNode(FunctionCallNode node)
        {
            if (!functionTable.ContainsKey(node.GetName()))
            {
                throw new Exception($"Function '{node.GetName()}' unidentified.");
            }

            var function = functionTable[node.GetName()];
            var previousFunction = currentExecutingFunction;
            currentExecutingFunction = function;

            // Создаем временную таблицу символов для хранения аргументов
            var localSymbolTable = new SymbolTable<object>();

            // Сопоставляем параметры и аргументы
            var parameters = function.GetParameters(); // Имена параметров
            var parameterTypes = function.GetParameterTypes(); // Типы параметров
            var arguments = node.GetArguments(); // Переданные аргументы

            if (parameters.Count != arguments.Count)
            {
                throw new Exception($"Function '{node.GetName()}' expected {parameters.Count} arguments, but got {arguments.Count}.");
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                arguments[i].Accept(this); // Вычисляем значение аргумента
                var argumentValue = stack.Pop().GetValue(); // Получаем значение аргумента
                var argumentType = GetTypeName(argumentValue); // Определяем тип аргумента

                // Проверяем совместимость типов
                if (!TypesMatch(parameterTypes[i], argumentType))
                {
                    throw new Exception($"Argument type mismatch {i + 1} in function '{node.GetName()}': expected '{parameterTypes[i]}', but got '{argumentType}'.");
                }

                localSymbolTable.Define(parameters[i], new Symbol<object>(parameterTypes[i], argumentValue));
            }

            // Выполняем тело функции
            var previousSymbolTable = symbolTable;
            symbolTable = localSymbolTable;

            try
            {
                function.GetBody().Accept(this);
            }
            catch (ReturnException ex)
            {
                if (function.GetReturnType().GetValue() != "void")
                {
                    stack.Push(new Symbol<object>("return", ex.Value));
                }
            }
            finally
            {
                // Если функция должна что-то вернуть, а возврата не было
                if (function.GetReturnType().GetValue() != "void" && stack.Count == 0)
                {
                    throw new Exception($"Function '{function.GetName()}' should return a value.");
                }
                currentExecutingFunction = previousFunction;
                symbolTable = previousSymbolTable;
            }
        }

        public void VisitReturnNode(ReturnNode node)
        {
            // Проверяем текущую выполняемую функцию
            if (currentExecutingFunction == null)
            {
                throw new Exception("Return statement out of bounds function.");
            }

            // Получаем тип возвращаемого значения
            string expectedType = currentExecutingFunction.GetReturnType().GetValue();

            if (expectedType == "void")
            {
                if (node.GetExpression() != null)
                {
                    throw new Exception($"Function '{currentExecutingFunction.GetName()}' returns nothing by definition");
                }
            }
            else
            {
                // Вычисляем возвращаемое значение
                if (node.GetExpression() == null)
                {
                    throw new Exception($"Function '{currentExecutingFunction.GetName()}' should return a value of type '{expectedType}'.");
                }

                node.GetExpression().Accept(this);
                var returnValue = stack.Pop().GetValue();

                // Проверяем тип возвращаемого значения
                string actualType = GetTypeName(returnValue);
                if (!TypesMatch(expectedType, actualType))
                {
                    throw new Exception($"Type mismatch: function '{currentExecutingFunction.GetName()}' expected return type '{expectedType}', but got '{actualType}'.");
                }

                throw new ReturnException(returnValue); // Возвращаем значение
            }

            // Если это void-функция, просто завершаем выполнение
            throw new ReturnException(null);
        }

        public void VisitIfNode(IfNode node)
        {
            node.GetCondition().Accept(this);
            var condition = (bool)stack.Pop().GetValue();

            if (condition)
            {
                node.GetRight().Accept(this);
            }

            else
            {
                bool executed = false;

                // Проверка каждого блока "else if"
                foreach (var (elseIfCondition, elseIfBody) in node.GetElseIfBlocks())
                {
                    elseIfCondition.Accept(this);
                    if ((bool)stack.Pop().GetValue())
                    {
                        elseIfBody.Accept(this);
                        executed = true;
                        break;
                    }
                }

                // Если ни одно из условий не выполнено, выполняется блок "else"
                if (!executed && node.GetElseBody() != null)
                {
                    node.GetElseBody().Accept(this);
                }
            }
        }

        public void VisitWhileNode(WhileNode node)
        {
            node.GetCondition().Accept(this);
            var condition = (bool)stack.Pop().GetValue();

            while (condition)
            {
                node.GetRight().Accept(this); // Выполнить тело цикла
                node.GetCondition().Accept(this); // Переоценить условие
                condition = (bool)stack.Pop().GetValue();
            }
        }

        public void VisitDoWhileNode(DoWhileNode node)
        {
            node.GetCondition().Accept(this);
            var condition = (bool)stack.Pop().GetValue();

            do
            {
                node.GetRight().Accept(this); // Выполнить тело цикла
                node.GetCondition().Accept(this); // Переоценить условие
                condition = (bool)stack.Pop().GetValue();
            } while (condition);
        }

        public void VisitConditionNode(ConditionNode node)
        {
            node.GetLeft().Accept(this);
            var leftValue = stack.Pop().GetValue();

            // Если это переменная, извлекаем ее значение
            if (leftValue is string variableName && symbolTable.IsDefined(variableName))
            {
                leftValue = symbolTable.Lookup(variableName).GetValue();
            }

            node.GetRight().Accept(this);
            var rightValue = stack.Pop().GetValue();

            if (rightValue is string variableNameRight && symbolTable.IsDefined(variableNameRight))
            {
                rightValue = symbolTable.Lookup(variableNameRight).GetValue();
            }

            bool result;
            switch (node.GetValue())
            {
                case ">":
                    result = (double)leftValue > (double)rightValue;
                    break;
                case ">=":
                    result = (double)leftValue >= (double)rightValue;
                    break;
                case "<":
                    result = (double)leftValue < (double)rightValue;
                    break;
                case "<=":
                    result = (double)leftValue <= (double)rightValue;
                    break;
                case "==":
                    result = (double)leftValue == (double)rightValue;
                    break;
                case "!=":
                    result = (double)leftValue != (double)rightValue;
                    break;
                default:
                    throw new Exception("Unexpected comparison operator: " + node.GetValue());
            }

            stack.Push(new Symbol<object>("bool", result));
        }

        public void VisitAssignationNode(AssignationNode node)
        {
            string varName = node.GetLeft().GetValue(); // Имя переменной
            if (!symbolTable.IsDefined(varName))
            {
                throw new Exception("Unidentified variable: " + varName);
            }

            string varType = symbolTable.Lookup(varName)?.GetType();//? - null support
            node.GetRight().Accept(this); // Вычисляем выражение
            object value = stack.Pop().GetValue(); // Получаем результат вычисления

            if (value is List<object>)
            {
                symbolTable.Define(varName, new Symbol<object>("set", value));
            }
            else if (value is string && varType == "stg")
            {
                symbolTable.Define(varName, new Symbol<object>("stg", value));
            }
            else if (value is double && varType == "num")
            {
                symbolTable.Define(varName, new Symbol<object>("num", value));
            }
            else if (value is char && varType == "chr")
            {
                symbolTable.Define(varName, new Symbol<object>("chr", value));
            }
            else if (value is bool && varType == "bool")
            {
                symbolTable.Define(varName, new Symbol<object>("bool", value));
            }
            else
            {
                throw new Exception($"Incompatibility between declaration type and expression");
            }

            symbolTable.Define(varName, new Symbol<object>("num", value));
        }

        public void VisitDeclarationNode(DeclarationNode node)
        {

            string varName = node.GetLeft().GetValue();
            string varType = node.GetRight().GetValue(); // Тип переменной

            //если есть начальное значение => интерпретируем его и сохраняем
            if (node.GetInitialValue() != null)
            {
                node.GetInitialValue().Accept(this);

                object value = stack.Pop().GetValue();

                // Проверяем на совместимость типов
                if (value is List<object> && varType == "set")
                {
                    symbolTable.Define(varName, new Symbol<object>("set", value));
                }
                else if (value is string && varType == "stg")
                {
                    symbolTable.Define(varName, new Symbol<object>("stg", value));
                }
                else if (value is double && varType == "num")
                {
                    symbolTable.Define(varName, new Symbol<object>("num", value));
                }
                else if (value is char && varType == "chr")
                {
                    symbolTable.Define(varName, new Symbol<object>("chr", value));
                }
                else if (value is bool && varType == "bool")
                {
                    symbolTable.Define(varName, new Symbol<object>("bool", value));
                }
                else
                {
                    throw new Exception($"Incompatibility between declaration type and expression");
                }
            }
            else
            {
                symbolTable.Define(varName, null);
            }
        }

        public void VisitCompoundNode(CompoundNode node)
        {
            foreach (var child in node.GetChildren())
            {
                child.Accept(this);
            }
        }

        public void VisitVariableNode(VariableNode node)
        {
            string varName = node.GetValue();
            Symbol<object> value = symbolTable.Lookup(varName);
            if (value != null) stack.Push(value);
            else throw new Exception("Null pointer exception: " + varName);
        }
        #endregion

        #region EXPRESSIONS
        public void VisitBinaryOperationNode(BinaryOperationNode node)
        {
            try
            {
                switch (node.GetValue())
                {
                    case "+":
                        {
                            EvaluatePls(node);
                            break;
                        }
                    case "-":
                        {
                            EvaluateMin(node);
                            break;
                        }
                    case "*":
                        {
                            EvaluateMul(node);
                            break;
                        }
                    case "%":
                        {
                            EvaluateMod(node);
                            break;
                        }
                    case "^":
                        {
                            EvaluateExp(node);
                            break;
                        }
                    case "==":
                        {
                            EvaluateEquals(node);
                            break;
                        }
                    case ">":
                        {
                            EvaluateGreaterThan(node);
                            break;
                        }
                    case "<":
                        {
                            EvaluateLessThan(node);
                            break;
                        }
                    case ">=":
                        {
                            EvaluateGreaterThanOrEqual(node);
                            break;
                        }
                    case "<=":
                        {
                            EvaluateLessThanOrEqual(node);
                            break;
                        }
                    default:
                        {
                            EvaluateDiv(node);
                            break;
                        }
                }
            }
            catch (Exception)
            {
                throw new Exception("Incompatible types");
            }
        }

        public void VisitNumberNode(NumberLiteralNode node)
        {
            double number;
            string value = node.GetValue();

            // Проверяем, не является ли строка нечисловым идентификатором
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number))//поддержка нотации плав. точки вместо запятой
            {
                stack.Push(new Symbol<object>("num", number));
            }
            else
            {
                // Если значение не является числом, проверяем, может ли это быть идентификатором переменной
                if (symbolTable.IsDefined(value))
                {
                    object variableValue = symbolTable.Lookup(value).GetValue();
                    if (variableValue is double)
                    {
                        stack.Push(new Symbol<object>("num", variableValue));
                    }
                    else
                    {
                        throw new FormatException($"Expected a numeric value but got it: {variableValue}");
                    }
                }
                else
                {
                    throw new FormatException($"Cannot convert string '{value}' in number.");
                }
            }
        }

        public void VisitStringNode(StringLiteralNode node)
        {

            stack.Push(new Symbol<object>("stg", node.GetValue()));
        }

        public void VisitCharNode(CharLiteralNode node)
        {
            char charValue = node.GetValue()[0];
            stack.Push(new Symbol<object>("chr", charValue));
        }

        public void VisitBoolNode(BoolLiteralNode node)
        {
            bool boolValue = node.GetValue() == "tr";
            stack.Push(new Symbol<object>("bool", boolValue));
        }

        public void VisitSetLiteralNode(SetLiteralNode node)
        {
            List<object> values = new List<object>();
            foreach (var element in node.GetElements())
            {
                element.Accept(this); // Интерпретировать каждый элемент массива
                values.Add(stack.Pop().GetValue()); // Получить значение и сохранить
            }
            stack.Push(new Symbol<object>("set", values));
        }

        public void VisitSetCallNode(SetCallNode node)
        {
            node.GetSetNode().Accept(this); // Интерпретируем узел массива
            var set = stack.Pop().GetValue() as List<object>;

            if (node.IsCountOperation())
            {
                stack.Push(new Symbol<object>("num", set.Count));
            }
            else
            {
                node.GetIndex().Accept(this); // Интерпретируем узел индекса
                var indexValue = stack.Pop().GetValue();

                // Если индекс — это переменная, получаем ее значение
                if (indexValue is string variableName && symbolTable.IsDefined(variableName))
                {
                    indexValue = symbolTable.Lookup(variableName).GetValue();
                }

                int index = Convert.ToInt32(indexValue); // Преобразуем в int

                if (set == null || index < 0 || index >= set.Count)
                {
                    throw new Exception("Index out of bounds");
                }

                // Извлекаем элемент по индексу и помещаем его в стек
                stack.Push(new Symbol<object>("num", set[index]));
            }
        }

        public void VisitOutputNode(OutputNode node)
        {
            node.GetNode().Accept(this);
            Console.Write(stack.Pop().GetValue());
        }

        public void VisitOutputlnNode(OutputlnNode node)
        {
            node.GetNode().Accept(this);
            Console.WriteLine(stack.Pop().GetValue());
        }

        public void VisitInputlnNode(InputlnNode node)
        {
            string input = Console.ReadLine();
            stack.Push(new Symbol<object>("stg", input));
        }

        public void VisitNumberInputlnNode(NumberInputlnNode node)
        {
            double input = Convert.ToDouble(Console.ReadLine());
            stack.Push(new Symbol<object>("num", input));
        }
        #endregion

        #region OPERATORS
        private void EvaluatePls(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);

            object rightValue = stack.Pop().GetValue();
            if (rightValue is string rightVarName && symbolTable.IsDefined(rightVarName))
            {
                rightValue = symbolTable.Lookup(rightVarName).GetValue();
            }

            object leftValue = stack.Pop().GetValue();
            if (leftValue is string leftVarName && symbolTable.IsDefined(leftVarName))
            {
                leftValue = symbolTable.Lookup(leftVarName).GetValue();
            }

            if (leftValue is string && rightValue is string)
            {
                stack.Push(new Symbol<object>("stg", leftValue.ToString() + rightValue.ToString()));
            }
            else if (leftValue is double && rightValue is double)
            {
                stack.Push(new Symbol<object>("num", (double)leftValue + (double)rightValue));
            }
            else
            {
                throw new Exception("Incompatible types for the addition operation");
            }
        }

        private void EvaluateMin(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);
            double val1 = (double)stack.Pop().GetValue();
            double val2 = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("num", val2 - val1));
        }

        private void EvaluateMul(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);
            double val1 = (double)stack.Pop().GetValue();
            double val2 = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("num", val2 * val1));
        }

        private void EvaluateDiv(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);
            double val1 = (double)stack.Pop().GetValue();
            double val2 = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("num", val2 / val1));
        }

        private void EvaluateMod(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);
            double val1 = (double)stack.Pop().GetValue();
            double val2 = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("num", val2 % val1));
        }

        private void EvaluateExp(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            node.GetRight().Accept(this);
            double val1 = (double)stack.Pop().GetValue();
            double val2 = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("num", Math.Pow(val2, val1)));
        }

        private void EvaluateEquals(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            bool leftValue = (bool)stack.Pop().GetValue();
            node.GetRight().Accept(this);
            bool rightValue = (bool)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("bool", leftValue == rightValue));
        }

        private void EvaluateGreaterThan(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            double leftValue = (double)stack.Pop().GetValue();
            node.GetRight().Accept(this);
            double rightValue = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("bool", leftValue > rightValue));
        }

        private void EvaluateLessThan(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            double leftValue = (double)stack.Pop().GetValue();
            node.GetRight().Accept(this);
            double rightValue = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("bool", leftValue < rightValue));
        }

        private void EvaluateGreaterThanOrEqual(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            double leftValue = (double)stack.Pop().GetValue();
            node.GetRight().Accept(this);
            double rightValue = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("bool", leftValue >= rightValue));
        }

        private void EvaluateLessThanOrEqual(BinaryOperationNode node)
        {
            node.GetLeft().Accept(this);
            double leftValue = (double)stack.Pop().GetValue();
            node.GetRight().Accept(this);
            double rightValue = (double)stack.Pop().GetValue();
            stack.Push(new Symbol<object>("bool", leftValue <= rightValue));
        }
        #endregion 
    }
}
