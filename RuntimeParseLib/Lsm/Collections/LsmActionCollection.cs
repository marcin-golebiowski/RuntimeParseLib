/*
 * LsmActionCollection.cs
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
    public class LsmActionCollection : ObservableCollection<LsmAction>, ILsmExpressable
    {
        public Expression GetExpression(LsmContext lsmContext)
        {
            if (this.Count == 0)
                return Expression.Empty();

            List<ILsmExpressable> expressableActions = new List<ILsmExpressable>();

            foreach (LsmAction action in this)
            {
                ILsmExpressable expressableAction
                    = action as ILsmExpressable;

                if (expressableAction != null)
                    expressableActions.Add(expressableAction);
            }

            return Expression.Block(expressableActions.Select((sa)=>sa.GetExpression(lsmContext)));
        }

        //Method untested
        public LsmState GetTransitionDestination()
        {
            foreach (LsmAction action in this)
            {
                LsmStateTransitionAction transitionAction = action as LsmStateTransitionAction;
                if (transitionAction == null)
                    continue;

                return transitionAction.Destination;
            }

            return null;
        }

        public void SetTransitionDestination(LsmState nextState)
        {

            foreach (LsmAction action in this)
            {
                LsmStateTransitionAction transitionAction = action as LsmStateTransitionAction;
                if (transitionAction == null)
                    continue;

                transitionAction.Destination = nextState;
                return;
            }

            Add(new LsmStateTransitionAction(nextState));
        }
    }
}
