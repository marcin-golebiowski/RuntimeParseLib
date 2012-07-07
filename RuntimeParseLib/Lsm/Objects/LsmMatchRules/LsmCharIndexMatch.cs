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

    public class LsmCharIndexMatchRule : LsmMatchRule, ILsmExpressable
    {
        int _charIndex;

        public LsmCharIndexMatchRule(int charIndex)
        {
        
            _charIndex = charIndex;
        }

    
        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                Expression.Equal(
                    lsmContext.charIndexInStateVariable,
                    Expression.Constant(_charIndex)),
                Actions.GetExpression(lsmContext),
                Expression.Empty());
        }

        public int CharIndex
        {
            get { return _charIndex; }
        }

        public override string ToString()
        {
            return string.Format("Char Index in State is {0}", _charIndex);
        }

        public override int GetEvaluationOrder()
        {
            return -1;
        }
    }
}
