using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triple.AST;
using triple.AST.LITERALS;

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
        void VisitInputlnNode(InputlnNode node);
        void VisitNumberInputlnNode(NumberInputlnNode node);
        void VisitIfNode(IfNode node);
        void VisitWhileNode(WhileNode node);
        void VisitDoWhileNode(DoWhileNode node);
        void VisitConditionNode(ConditionNode node);
        void VisitSetLiteralNode(SetLiteralNode node);
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
