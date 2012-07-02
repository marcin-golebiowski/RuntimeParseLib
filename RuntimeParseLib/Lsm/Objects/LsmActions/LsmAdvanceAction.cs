/*
 * LsmAdvanceAction.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmAdvanceAction : LsmAction, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.Block(
                LsmCommonExpressions.ReadChar(lsmContext),
                LsmCommonExpressions.IncrementCharIndex(lsmContext));
        }

        public override string ToString()
        {
            return "Advance Character Position";
        }
    }
}
