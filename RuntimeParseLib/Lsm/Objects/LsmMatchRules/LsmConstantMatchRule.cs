/*
 * LsmConstantMatchRule.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmConstantMatchRule : LsmMatchRule, ILsmExpressable
    {
        char _constant;

        public LsmConstantMatchRule(char constant)
        {
            _constant = constant;
        }

        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                LsmCommonExpressions.CharMatch(lsmContext, _constant),
                Actions.GetExpression(lsmContext),
                Expression.Empty());
        }

        public char Constant
        {
            get { return _constant; }
        }
    }
}
