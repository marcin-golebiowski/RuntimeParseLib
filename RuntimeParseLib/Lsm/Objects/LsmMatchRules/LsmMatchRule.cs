/*
 * LsmMatchRule.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public abstract class LsmMatchRule
    {
        LsmActionCollection _actions = new LsmActionCollection();

        public LsmActionCollection Actions
        {
            get { return _actions; }
        }

        public virtual int GetEvaluationOrder()
        {
            return 0;
        }
    }
}
