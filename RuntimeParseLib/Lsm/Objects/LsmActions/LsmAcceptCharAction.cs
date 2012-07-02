/*
 * LsmAcceptCharAction.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmAcceptCharAction : LsmAction, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            return LsmCommonExpressions.AcceptChar(lsmContext);
        }

        public override string ToString()
        {
            return "Accept Character";
        }
    }
}
