/*
 * LsmStateCollection.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmStateCollection : ObservableCollection<LsmState>, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            if (this.Count == 0)
                return Expression.Empty();

            return LsmCommonExpressions.IfElseChain(
                this,
                Expression.Empty(),
                lsmContext);
        }
    }
}
