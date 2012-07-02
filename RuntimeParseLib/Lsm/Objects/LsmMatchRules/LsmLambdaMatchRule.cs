/*
 * LsmLambdaMatchRule.cs
 * Author: Guido Arbia
 * 
 * I do not recommend using this class anymore.
 * Token path builder classes will need to assess the specificity of a
 * match rule to determine proper precedence, and the specificity of a
 * lambda cannot be easily asessed. This file will be removed in the
 * near future.
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

        public override string ToString()
        {
            return ("(Non-Displayable)"); 
        }
    }
}
