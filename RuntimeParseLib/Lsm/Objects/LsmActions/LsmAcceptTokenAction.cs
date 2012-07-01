/*
 * LsmAcceptTokenAction.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmAcceptTokenAction : LsmAction, ILsmExpressable
    {
        int _tokenTypeID;

        public Expression GetExpression(LsmContext lsmContext)
        {
            return LsmCommonExpressions.AcceptToken(lsmContext, _tokenTypeID);
        }

        public LsmAcceptTokenAction(int typeID)
        {
            _tokenTypeID = typeID;
        }
    }
}
