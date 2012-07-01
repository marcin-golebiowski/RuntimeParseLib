/*
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
    public interface ILsmExpressable
    {
        Expression GetExpression(LsmContext lsmContext);
    }
}
