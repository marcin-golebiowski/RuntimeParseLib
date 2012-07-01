/*
 * LsmClearTokenAction.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmClearTokenTextAction : LsmAction, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            return LsmCommonExpressions.ClearTokenText(lsmContext);
        }
    }
}
