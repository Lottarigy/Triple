using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.PARSER.NODES;
using triple.PARSER.NODES.LITERALS;

namespace triple.INTERPRETER
{
    public interface INodeVisitor
    {
        void VisitCompoundNode(CompoundNode node);
        void VisitAssignationNode(AssignationNode node);
        void VisitDeclarationNode(DeclarationNode node);
        void VisitVariableNode(VariableNode node);
        void VisitBinaryOperationNode(BinaryOperationNode node);
        void VisitNumberNode(NumberLiteralNode node);
        void VisitStringNode(StringLiteralNode node);
        void VisitCharNode(CharLiteralNode node);
        void VisitBoolNode(BoolLiteralNode node);
        void VisitOutputNode(OutputNode node);
        void VisitOutputlnNode(OutputlnNode node);
        void VisitInputNode(InputNode node);
        void VisitNumberInputNode(NumberInputNode node);
        void VisitIfNode(IfNode node);
        void VisitWhileNode(WhileNode node);
        void VisitDoWhileNode(DoWhileNode node);
        void VisitConditionNode(ConditionNode node);
        void VisitSetNode(SetNode node);
        void VisitSetCallNode(SetCallNode node);
        void VisitFunctionNode(FunctionNode node);
        void VisitFunctionCallNode(FunctionCallNode node);
        void VisitReturnNode(ReturnNode node);
        void VisitStructureNode(StructureNode node);
        void VisitStructureCallNode(StructureCallNode node);
        void VisitStreamFunctionNode(StreamFunctionNode node);
        void VisitProgramNode(ProgramNode node);
    }
}
