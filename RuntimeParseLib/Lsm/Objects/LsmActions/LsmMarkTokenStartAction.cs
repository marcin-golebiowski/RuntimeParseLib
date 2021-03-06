﻿/*
 * 
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmMarkTokenStartAction : LsmAction, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            return LsmCommonExpressions.MarkTokenStart(lsmContext);
        }

        public override string ToString()
        {
            return "Mark Position as Token Start";
        }
    }
}
