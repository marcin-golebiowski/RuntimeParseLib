/*
 * LsmContext.cs
 * Author: Guido Arbia
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.IO;

namespace RuntimeParseLib.Lsm
{
    public class LsmContext
    {
        public ParameterExpression textReaderParameter
            = Expression.Parameter(typeof(TextReader), "textReader");

        public ParameterExpression currentCharVariable
            = Expression.Parameter(typeof(Char), "currentChar");

        public  ParameterExpression tokenTextVariable
            = Expression.Parameter(typeof(StringBuilder), "tokenText");

        public  ParameterExpression charIndexVariable
            = Expression.Parameter(typeof(int), "charIndex");

        public ParameterExpression tokenListVariable
            = Expression.Parameter(typeof(List<LsmToken>), "tokenList");

        public ParameterExpression currentStateVariable
            = Expression.Parameter(typeof(int), "currentState");

        public ParameterExpression peekValueVariable
            = Expression.Parameter(typeof(int), "peekValue");

        public ParameterExpression tokenStartVariable
            = Expression.Parameter(typeof(int), "tokenStartIndex");

        public IEnumerable<ParameterExpression> GetLocalVariables()
        {
            return new [] { tokenTextVariable, charIndexVariable,
                        tokenListVariable, currentStateVariable,
                        peekValueVariable, currentCharVariable,
                        tokenStartVariable };
        }

        public BlockExpression GetInitializationBlock(int startStateID)
        {
            return Expression.Block(
                Expression.Assign(
                    tokenTextVariable,
                    LsmCommonExpressions.NewStringBuilder()),
                Expression.Assign(
                    tokenListVariable,
                    LsmCommonExpressions.NewTokenList()),
                Expression.Assign(
                    charIndexVariable,
                    Expression.Constant(0)),
                Expression.Assign(
                    tokenStartVariable,
                    Expression.Constant(0)),
                Expression.Assign(
                    currentStateVariable,
                    Expression.Constant(startStateID)));
        }
    }
}
