/*
 * LsmStateCollection.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmStateCollection : ObservableCollection<LsmState>, ILsmExpressable
    {
        int _lastAssignedID = -1;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Replace)
                foreach (LsmState newState in e.NewItems)
                    newState.ID = ++_lastAssignedID;

            base.OnCollectionChanged(e);
        }

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
