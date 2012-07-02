/*
 * LsmCommonExpressions.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public static class LsmCommonExpressions
    {
        public static Expression CharMatch(LsmContext lsmContext, char constant)
        {
            return Expression.Equal(
                lsmContext.currentCharVariable,
                Expression.Constant(constant));
        }

        public static Expression CallCharMatchFunc(LsmContext lsmContext,
            Func<char, bool> func)
        {
            return Expression.Call(
                null,
                func.Method,
                lsmContext.currentCharVariable);
        }

        public static Expression EndOfStream(LsmContext lsmContext)
        {
            return Expression.Equal(
                lsmContext.peekValueVariable,
                Expression.Constant(-1));
        }

        public static Expression IfElseChain(IEnumerable<ILsmExpressable> expressableConditions,
            Expression defaultExpression, LsmContext lsmContext)
        {
            List<ILsmExpressable> expressables = expressableConditions.Reverse().ToList();

            ConditionalExpression lastOuterExpression = expressables[0].GetExpression(lsmContext)
                as ConditionalExpression;
            expressables.RemoveAt(0);

            lastOuterExpression = Expression.IfThenElse(
                lastOuterExpression.Test,
                lastOuterExpression.IfTrue,
                defaultExpression);

            while (expressables.Count > 0)
            {
                ConditionalExpression outerExpression = expressables[0].GetExpression(lsmContext)
                    as ConditionalExpression;

                lastOuterExpression = Expression.IfThenElse(outerExpression.Test,
                    outerExpression.IfTrue,
                    lastOuterExpression);

                expressables.RemoveAt(0);
            }


            return lastOuterExpression;
        }

        public static Expression NewToken(LsmContext lsmContext, int typeID)
        {
            return Expression.New(
                typeof(LsmToken).GetConstructor(
                    new [] {typeof(int), typeof(string), typeof(int)}),
                Expression.Constant(typeID),
                LsmCommonExpressions.GetTokenText(lsmContext),
                lsmContext.tokenStartVariable);
        }

        public static Expression NewTokenList()
        {
            return Expression.New(
                typeof(List<LsmToken>).GetConstructor(Type.EmptyTypes));
        }

        public static Expression NewStringBuilder()
        {
            return Expression.New(
                typeof(StringBuilder).GetConstructor(Type.EmptyTypes));
        }

        public static Expression GetTokenText(LsmContext lsmContext)
        {
            return Expression.Call(
                lsmContext.tokenTextVariable,
                typeof(StringBuilder).GetMethod("ToString", Type.EmptyTypes));
        }

        public static Expression IncrementCharIndex(LsmContext lsmContext)
        {
            return Expression.PostIncrementAssign(lsmContext.charIndexVariable);
        }

        public static Expression ReadChar(LsmContext lsmContext)
        {
            return Expression.Call(
                lsmContext.textReaderParameter,
                typeof(TextReader).GetMethod("Read", Type.EmptyTypes));
        }

        public static Expression PeekValue(LsmContext lsmContext)
        {
            return Expression.Call(
                lsmContext.textReaderParameter,
                typeof(TextReader).GetMethod("Peek", Type.EmptyTypes));
        }

        public static Expression ClearTokenText(LsmContext lsmContext)
        {
            return Expression.Call(
                lsmContext.tokenTextVariable,
                typeof(StringBuilder).GetMethod("Clear", Type.EmptyTypes));
        }

        public static Expression AcceptChar(LsmContext lsmContext)
        {
            return Expression.Call(
                lsmContext.tokenTextVariable,
                typeof(StringBuilder).GetMethod("Append", new[] { typeof(char) }),
                lsmContext.currentCharVariable);
        }

        public static Expression AssignCurrentChar(LsmContext lsmContext)
        {
            return Expression.Assign(
                lsmContext.currentCharVariable,
                Expression.Call(
                    null,
                    typeof(Convert).GetMethod("ToChar", new[] {typeof(int)}),
                    LsmCommonExpressions.PeekValue(lsmContext)));
        }

        public static Expression AcceptToken(LsmContext lsmContext, int typeID)
        {
            return Expression.Call(
                lsmContext.tokenListVariable,
                typeof(List<LsmToken>).GetMethod("Add", new[] {typeof(LsmToken)}),
                LsmCommonExpressions.NewToken(lsmContext, typeID));
        }

        public static Expression ChangeState(LsmContext context, int stateID)
        {
            return Expression.Assign(
                context.currentStateVariable,
                Expression.Constant(stateID));
        }

        public static Expression AssignPeekValue(LsmContext lsmContext)
        {
            return Expression.Assign(
                lsmContext.peekValueVariable,
                LsmCommonExpressions.PeekValue(lsmContext));
        }

        public static Expression MarkTokenStart(LsmContext lsmContext)
        {
            return Expression.Assign(
                lsmContext.tokenStartVariable,
                lsmContext.charIndexVariable);
        }

        public static Expression LetterMatch(LsmContext lsmContext)
        {
            return Expression.Call(
                null,
                typeof(char).GetMethod(
                    "IsLetter",
                    new[] { typeof(char) }),
                lsmContext.currentCharVariable);
        }

        public static Expression DigitMatch(LsmContext lsmContext)
        {
            return Expression.Call(
                null,
                typeof(char).GetMethod(
                    "IsDigit",
                    new[] { typeof(char) }),
                lsmContext.currentCharVariable);
        }

        public static Expression AlphaNumericMatch(LsmContext lsmContext)
        {
            return Expression.Call(
                null,
                typeof(char).GetMethod(
                    "IsLetterOrDigit",
                    new[] { typeof(char) }),
                lsmContext.currentCharVariable);
        }

        public static Expression WhiteSpaceMatch(LsmContext lsmContext)
        {
            return Expression.Call(
                null,
                typeof(char).GetMethod(
                    "IsWhiteSpace",
                    new[] { typeof(char) }),
                lsmContext.currentCharVariable);
        }
    }
}
