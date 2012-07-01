/*
 * LsmLambdaMatchRule.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmLambdaMatchRule : LsmMatchRule, ILsmExpressable
    {
        Func<char, bool> _matchFunc = null;

        public LsmLambdaMatchRule(Func<char, bool> matchFunc)
        {
            if (matchFunc == null)
                throw new ArgumentNullException("matchFunc");

            _matchFunc = matchFunc;
        }

        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                LsmCommonExpressions.CallCharMatchFunc(
                    lsmContext, _matchFunc),
                Actions.GetExpression(lsmContext),
                Expression.Empty());
        }
    }
}
