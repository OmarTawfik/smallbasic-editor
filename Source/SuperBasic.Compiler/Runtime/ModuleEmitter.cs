﻿// <copyright file="ModuleEmitter.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Utilities;

    internal sealed class ModuleEmitter
    {
        private int jumpLabelCounter = 1;
        private List<BaseInstruction> instructions = new List<BaseInstruction>();

        public ModuleEmitter(BoundStatementBlock module)
        {
            this.EmitStatement(module);
            this.RemoveTransientInstructions();
        }

        public IReadOnlyList<BaseInstruction> Instructions => this.instructions;

        private void EmitStatement(BaseBoundStatement statement)
        {
            switch (statement)
            {
                case BoundStatementBlock blockStatement: this.EmitBlockStatement(blockStatement); break;
                case BoundIfStatement ifStatement: this.EmitIfStatement(ifStatement); break;
                case BoundWhileStatement whileStatement: this.EmitWhileStatement(whileStatement); break;
                case BoundForStatement forStatement: this.EmitForStatement(forStatement); break;
                case BoundLabelStatement labelStatement: this.EmitLabelStatement(labelStatement); break;
                case BoundGoToStatement goToStatement: this.EmitGoToStatement(goToStatement); break;
                case BoundSubModuleInvocationStatement subModuleInvocationStatement: this.EmitSubModuleInvocationStatement(subModuleInvocationStatement); break;
                case BoundLibraryMethodInvocationStatement libraryMethodInvocaationStatement: this.EmitLibraryMethodInvocationStatement(libraryMethodInvocaationStatement); break;
                case BoundEventAssignmentStatement eventAssignmentStatement: this.EmitEventAssignmentStatement(eventAssignmentStatement); break;
                case BoundVariableAssignmentStatement variableAssignmentStatement: this.EmitVariableAssignmentStatement(variableAssignmentStatement); break;
                case BoundPropertyAssignmentStatement propertyAssignmentStatement: this.EmitPropertyAssignmentStatement(propertyAssignmentStatement); break;
                case BoundArrayAssignmentStatement arrayAssignmentStatement: this.EmitArrayAssignmentStatement(arrayAssignmentStatement); break;
                default: throw ExceptionUtilities.UnexpectedValue(statement);
            }
        }

        private void EmitBlockStatement(BoundStatementBlock statement)
        {
            foreach (BaseBoundStatement child in statement.Body)
            {
                this.EmitStatement(child);
            }
        }

        private void EmitIfStatement(BoundIfStatement statement)
        {
            string endOfBlockLabel = this.GenerateJumpLabel();

            emitPart(statement.IfPart.Condition, statement.IfPart.Body);

            foreach (BoundElseIfPart elseIfPart in statement.ElseIfParts)
            {
                emitPart(elseIfPart.Condition, elseIfPart.Body);
            }

            if (!statement.ElsePartOpt.IsDefault())
            {
                this.EmitBlockStatement(statement.ElsePartOpt.Body);
            }

            this.instructions.Add(new TransientLabelInstruction(endOfBlockLabel, statement.Syntax.EndIfToken.Range));

            void emitPart(BaseBoundExpression condition, BoundStatementBlock body)
            {
                string endOfPartLabel = this.GenerateJumpLabel();

                this.EmitExpression(condition);
                this.instructions.Add(new TransientConditionalGoToInstruction(null, endOfPartLabel, this.instructions.Last().Range));

                this.EmitBlockStatement(body);

                TextRange endOfPartRange = this.instructions.Last().Range;
                this.instructions.Add(new TransientUnconditionalGoToInstruction(endOfBlockLabel, endOfPartRange));
                this.instructions.Add(new TransientLabelInstruction(endOfPartLabel, endOfPartRange));
            }
        }

        private void EmitWhileStatement(BoundWhileStatement statement)
        {
            string startOfLoopLabel = this.GenerateJumpLabel();
            string endOfLoopLabel = this.GenerateJumpLabel();

            this.instructions.Add(new TransientLabelInstruction(startOfLoopLabel, statement.Syntax.WhileToken.Range));
            this.EmitExpression(statement.Condition);
            this.instructions.Add(new TransientConditionalGoToInstruction(null, endOfLoopLabel, statement.Syntax.Condition.Range));

            this.EmitBlockStatement(statement.Body);

            TextRange endOfLoopRange = this.instructions.Last().Range;
            this.instructions.Add(new TransientUnconditionalGoToInstruction(startOfLoopLabel, endOfLoopRange));
            this.instructions.Add(new TransientLabelInstruction(endOfLoopLabel, endOfLoopRange));
        }

        private void EmitForStatement(BoundForStatement statement)
        {
            string beforeCheckLabel = this.GenerateJumpLabel();
            string positiveLoopLabel = this.GenerateJumpLabel();
            string negativeLoopLabel = this.GenerateJumpLabel();
            string afterCheckLabel = this.GenerateJumpLabel();
            string endOfBlockLabel = this.GenerateJumpLabel();

            this.EmitExpression(statement.FromExpression);
            this.instructions.Add(new StoreVariableInstruction(statement.Identifier, statement.Syntax.IdentifierToken.Range));
            this.instructions.Add(new TransientLabelInstruction(beforeCheckLabel, statement.Syntax.IdentifierToken.Range));

            if (!statement.StepExpressionOpt.IsDefault())
            {
                this.EmitExpression(statement.StepExpressionOpt);

                TextRange stepRange = statement.Syntax.StepClauseOpt.StepToken.Range;
                this.instructions.Add(new PushValueInstruction(new NumberValue(0), stepRange));
                this.instructions.Add(new LessThanInstruction(stepRange));
                this.instructions.Add(new TransientConditionalGoToInstruction(negativeLoopLabel, positiveLoopLabel, stepRange));
            }

            TextRange toExpressionRange = statement.Syntax.ToExpression.Range;

            this.instructions.Add(new TransientLabelInstruction(positiveLoopLabel, toExpressionRange));
            this.EmitExpression(statement.ToExpression);
            this.instructions.Add(new LoadVariableInstruction(statement.Identifier, toExpressionRange));
            this.instructions.Add(new LessThanInstruction(toExpressionRange));
            this.instructions.Add(new TransientConditionalGoToInstruction(endOfBlockLabel, afterCheckLabel, toExpressionRange));

            this.instructions.Add(new TransientLabelInstruction(negativeLoopLabel, toExpressionRange));
            this.instructions.Add(new LoadVariableInstruction(statement.Identifier, toExpressionRange));
            this.EmitExpression(statement.ToExpression);
            this.instructions.Add(new LessThanInstruction(toExpressionRange));
            this.instructions.Add(new TransientConditionalGoToInstruction(endOfBlockLabel, null, toExpressionRange));

            this.instructions.Add(new TransientLabelInstruction(afterCheckLabel, toExpressionRange));

            this.EmitBlockStatement(statement.Body);

            TextRange endForRange = statement.Syntax.EndForToken.Range;
            this.instructions.Add(new LoadVariableInstruction(statement.Identifier, endForRange));

            if (statement.StepExpressionOpt.IsDefault())
            {
                this.instructions.Add(new PushValueInstruction(new NumberValue(1), endForRange));
            }
            else
            {
                this.EmitExpression(statement.StepExpressionOpt);
            }

            this.instructions.Add(new AddInstruction(endForRange));
            this.instructions.Add(new StoreVariableInstruction(statement.Identifier, endForRange));
            this.instructions.Add(new TransientUnconditionalGoToInstruction(beforeCheckLabel, endForRange));

            this.instructions.Add(new TransientLabelInstruction(endOfBlockLabel, endForRange));
        }

        private void EmitLabelStatement(BoundLabelStatement statement)
        {
            this.instructions.Add(new TransientLabelInstruction(statement.Label, statement.Syntax.LabelToken.Range));
        }

        private void EmitGoToStatement(BoundGoToStatement statement)
        {
            this.instructions.Add(new TransientUnconditionalGoToInstruction(statement.Label, statement.Syntax.GoToToken.Range));
        }

        private void EmitSubModuleInvocationStatement(BoundSubModuleInvocationStatement statement)
        {
            this.instructions.Add(new InvokeSubModuleInstruction(statement.SubModule, statement.Syntax.Range));
        }

        private void EmitLibraryMethodInvocationStatement(BoundLibraryMethodInvocationStatement statement)
        {
            if (statement.Library == "Program" && statement.Method == "Pause")
            {
                this.instructions.Add(new PauseInstruction(statement.Syntax.Range));
            }

            foreach (BaseBoundExpression argument in statement.Arguments)
            {
                this.EmitExpression(argument);
            }

            this.instructions.Add(new MethodInvocationInstruction(statement.Library, statement.Method, statement.Syntax.Range));
        }

        private void EmitEventAssignmentStatement(BoundEventAssignmentStatement statement)
        {
            this.instructions.Add(new SetEventCallBackInstruction(statement.Library, statement.EventName, statement.SubModule, statement.Syntax.Range));
        }

        private void EmitVariableAssignmentStatement(BoundVariableAssignmentStatement statement)
        {
            this.EmitExpression(statement.Expression);
            this.instructions.Add(new StoreVariableInstruction(statement.Variable, statement.Syntax.Range));
        }

        private void EmitPropertyAssignmentStatement(BoundPropertyAssignmentStatement statement)
        {
            this.EmitExpression(statement.Expression);
            this.instructions.Add(new StorePropertyInstruction(statement.Library, statement.Property, statement.Syntax.Range));
        }

        private void EmitArrayAssignmentStatement(BoundArrayAssignmentStatement statement)
        {
            foreach (BaseBoundExpression index in statement.Indices.Reverse())
            {
                this.EmitExpression(index);
            }

            this.instructions.Add(new StoreArrayElementInstruction(statement.Array, statement.Indices.Count, statement.Syntax.Range));
        }

        private void EmitExpression(BaseBoundExpression expression)
        {
            switch (expression)
            {
                case BoundUnaryExpression unaryExpression: this.EmitUnaryExpression(unaryExpression); break;
                case BoundBinaryExpression binaryExpression: this.EmitBinaryExpression(binaryExpression); break;
                case BoundArrayAccessExpression arrayAccessExpression: this.EmitArrayAccessExpression(arrayAccessExpression); break;
                case BoundLibraryPropertyExpression libraryPropertyExpression: this.EmitLibraryPropertyExpression(libraryPropertyExpression); break;
                case BoundLibraryMethodInvocationExpression libraryMethodInvocationExpression: this.EmitLibraryMethodInvocationExpression(libraryMethodInvocationExpression); break;
                case BoundVariableExpression variableExpression: this.EmitVariableExpression(variableExpression); break;
                case BoundStringLiteralExpression stringLiteralExpression: this.EmitStringLiteralExpression(stringLiteralExpression); break;
                case BoundNumberLiteralExpression numberLiteralExpression: this.EmitNumberLiteralExpression(numberLiteralExpression); break;
                case BoundParenthesisExpression parenthesisExpression: this.EmitExpression(parenthesisExpression.Expression); break;
                default: throw ExceptionUtilities.UnexpectedValue(expression);
            }
        }

        private void EmitUnaryExpression(BoundUnaryExpression expression)
        {
            switch (expression.Kind)
            {
                case TokenKind.Minus:
                    {
                        this.EmitExpression(expression.Expression);
                        this.instructions.Add(new UnaryMinusInstruction(expression.Syntax.Range));
                        break;
                    }

                default:
                    {
                        throw ExceptionUtilities.UnexpectedValue(expression.Kind);
                    }
            }
        }

        private void EmitBinaryExpression(BoundBinaryExpression expression)
        {
            switch (expression.Kind)
            {
                case TokenKind.Or: this.EmitLogicalOrExpression(expression); break;
                case TokenKind.And: this.EmitLogicalAndExpression(expression); break;

                case TokenKind.Equal: this.EmitArithmeticBinaryExpression(expression, new EqualInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.NotEqual: this.EmitArithmeticBinaryExpression(expression, new EqualInstruction(expression.Syntax.Range), negateResult: true, reverseArgs: false); break;

                case TokenKind.LessThan: this.EmitArithmeticBinaryExpression(expression, new LessThanInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.LessThanOrEqual: this.EmitArithmeticBinaryExpression(expression, new LessThanOrEqualInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.GreaterThan: this.EmitArithmeticBinaryExpression(expression, new LessThanInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: true); break;
                case TokenKind.GreaterThanOrEqual: this.EmitArithmeticBinaryExpression(expression, new LessThanOrEqualInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: true); break;

                case TokenKind.Plus: this.EmitArithmeticBinaryExpression(expression, new AddInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.Minus: this.EmitArithmeticBinaryExpression(expression, new SubtractInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.Multiply: this.EmitArithmeticBinaryExpression(expression, new MultiplyInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;
                case TokenKind.Divide: this.EmitArithmeticBinaryExpression(expression, new DivideInstruction(expression.Syntax.Range), negateResult: false, reverseArgs: false); break;

                default: throw ExceptionUtilities.UnexpectedValue(expression.Kind);
            }
        }

        private void EmitLogicalOrExpression(BoundBinaryExpression expression)
        {
            string trySecondLabel = this.GenerateJumpLabel();
            string trueLabel = this.GenerateJumpLabel();
            string falseLabel = this.GenerateJumpLabel();
            string endLabel = this.GenerateJumpLabel();

            TextRange expressionRange = expression.Syntax.Range;

            this.EmitExpression(expression.Left);
            this.instructions.Add(new TransientConditionalGoToInstruction(trueLabel, trySecondLabel, expressionRange));

            this.instructions.Add(new TransientLabelInstruction(trySecondLabel, expressionRange));
            this.EmitExpression(expression.Right);
            this.instructions.Add(new TransientConditionalGoToInstruction(null, falseLabel, expressionRange));

            TextRange endOfOperatorRange = this.instructions.Last().Range;

            this.instructions.Add(new TransientLabelInstruction(trueLabel, endOfOperatorRange));
            this.instructions.Add(new PushValueInstruction(StringValue.True, endOfOperatorRange));
            this.instructions.Add(new TransientUnconditionalGoToInstruction(endLabel, endOfOperatorRange));

            this.instructions.Add(new TransientLabelInstruction(falseLabel, endOfOperatorRange));
            this.instructions.Add(new PushValueInstruction(StringValue.False, endOfOperatorRange));

            this.instructions.Add(new TransientLabelInstruction(endLabel, endOfOperatorRange));
        }

        private void EmitLogicalAndExpression(BoundBinaryExpression expression)
        {
            string falseLabel = this.GenerateJumpLabel();
            string endLabel = this.GenerateJumpLabel();

            TextRange expressionRange = expression.Syntax.Range;

            this.EmitExpression(expression.Left);
            this.instructions.Add(new TransientConditionalGoToInstruction(null, falseLabel, expressionRange));
            this.EmitExpression(expression.Right);
            this.instructions.Add(new TransientConditionalGoToInstruction(null, falseLabel, expressionRange));

            TextRange endOfOperatorRange = this.instructions.Last().Range;

            this.instructions.Add(new PushValueInstruction(StringValue.True, endOfOperatorRange));
            this.instructions.Add(new TransientUnconditionalGoToInstruction(endLabel, endOfOperatorRange));

            this.instructions.Add(new TransientLabelInstruction(falseLabel, endOfOperatorRange));
            this.instructions.Add(new PushValueInstruction(StringValue.False, endOfOperatorRange));

            this.instructions.Add(new TransientLabelInstruction(endLabel, endOfOperatorRange));
        }

        private void EmitArithmeticBinaryExpression(BoundBinaryExpression expression, BaseInstruction comparisonInstruction, bool negateResult, bool reverseArgs)
        {
            if (reverseArgs)
            {
                this.EmitExpression(expression.Right);
                this.EmitExpression(expression.Left);
            }
            else
            {
                this.EmitExpression(expression.Left);
                this.EmitExpression(expression.Right);
            }

            this.instructions.Add(comparisonInstruction);

            if (negateResult)
            {
                this.instructions.Add(new NegateBooleanInstruction(expression.Syntax.Range));
            }
        }

        private void EmitArrayAccessExpression(BoundArrayAccessExpression expression)
        {
            foreach (BaseBoundExpression index in expression.Indices.Reverse())
            {
                this.EmitExpression(index);
            }

            this.instructions.Add(new LoadArrayElementInstruction(expression.Array, expression.Indices.Count, expression.Syntax.Range));
        }

        private void EmitLibraryPropertyExpression(BoundLibraryPropertyExpression expression)
        {
            this.instructions.Add(new LoadPropertyInstruction(expression.Library, expression.Property, expression.Syntax.Range));
        }

        private void EmitLibraryMethodInvocationExpression(BoundLibraryMethodInvocationExpression expression)
        {
            if (expression.Library == "TextWindow")
            {
                if (expression.Method == "Read")
                {
                    this.instructions.Add(new BlockOnStringInputInstruction(expression.Syntax.Range));
                }
                else if (expression.Method == "ReadNumber")
                {
                    this.instructions.Add(new BlockOnNumberInputInstruction(expression.Syntax.Range));
                }
            }

            foreach (BaseBoundExpression index in expression.Arguments)
            {
                this.EmitExpression(index);
            }

            this.instructions.Add(new MethodInvocationInstruction(expression.Library, expression.Method, expression.Syntax.Range));
        }

        private void EmitVariableExpression(BoundVariableExpression expression)
        {
            this.instructions.Add(new LoadVariableInstruction(expression.Variable, expression.Syntax.Range));
        }

        private void EmitStringLiteralExpression(BoundStringLiteralExpression expression)
        {
            this.instructions.Add(new PushValueInstruction(StringValue.Create(expression.Value), expression.Syntax.Range));
        }

        private void EmitNumberLiteralExpression(BoundNumberLiteralExpression expression)
        {
            this.instructions.Add(new PushValueInstruction(new NumberValue(expression.Value), expression.Syntax.Range));
        }

        private void RemoveTransientInstructions()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            for (var i = 0; i < this.instructions.Count; i++)
            {
                if (this.instructions[i] is TransientLabelInstruction label)
                {
                    map.Add(label.Label, i);
                    this.instructions.RemoveAt(i);
                    i--;
                }
            }

            for (var i = 0; i < this.instructions.Count; i++)
            {
                switch (this.instructions[i])
                {
                    case TransientUnconditionalGoToInstruction goTo:
                        {
                            this.instructions[i] = new UnconditionalJumpInstruction(map[goTo.Label], goTo.Range);
                            break;
                        }

                    case TransientConditionalGoToInstruction goTo:
                        {
                            this.instructions[i] = new ConditionalJumpInstruction(replace(goTo.TrueLabelOpt), replace(goTo.FalseLabelOpt), goTo.Range);
                            break;
                        }
                }
            }

            int? replace(string label) => label.IsDefault() ? default(int) : map[label];
        }

        private string GenerateJumpLabel() => $"internal_$$_{this.jumpLabelCounter++}";
    }
}
