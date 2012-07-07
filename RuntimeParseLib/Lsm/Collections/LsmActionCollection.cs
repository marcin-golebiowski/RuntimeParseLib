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

            IEnumerable<ILsmExpressable> expressableActions
                = (from action
                   in this
                   where action is ILsmExpressable
                   select action as ILsmExpressable);

            return Expression.Block(expressableActions.Select((sa)=>sa.GetExpression(lsmContext)));
        }

        public LsmStateTransitionAction GetTransitionAction()
        {
            return (from action
                    in this
                    where action is LsmStateTransitionAction
                    select action as LsmStateTransitionAction).SingleOrDefault();
        }

        public LsmState GetTransitionDestination()
        {
            LsmStateTransitionAction transitionAction = GetTransitionAction();

            return (transitionAction == null 
                ? null 
                : transitionAction.Destination);
        }

        public void SetTransitionDestination(LsmState nextState)
        {
            LsmStateTransitionAction transitionAction = GetTransitionAction();

            if (transitionAction == null)
                this.Add(new LsmStateTransitionAction(nextState));
            else
                transitionAction.Destination = nextState;
        }

        public void EnsureAdvanceAction()
        {
            IEnumerable<LsmAdvanceAction> advanceActions
                = (from action
                   in this
                   where action is LsmAdvanceAction
                   select action as LsmAdvanceAction);

            if (advanceActions.SingleOrDefault() == null)
                this.Add(new LsmAdvanceAction());
        }

        public void EnsureClearTokenAction()
        {
            IEnumerable<LsmClearTokenTextAction> clearTokenTextActions
                = (from action
                   in this
                   where action is LsmClearTokenTextAction
                   select action as LsmClearTokenTextAction);

            if (clearTokenTextActions.SingleOrDefault() == null)
                this.Add(new LsmClearTokenTextAction());
        }

        public void EnsureAcceptCharAction()
        {
            IEnumerable<LsmAcceptCharAction> acceptTokenActions
                = (from action
                   in this
                   where action is LsmAcceptCharAction
                   select action as LsmAcceptCharAction);

            if (acceptTokenActions.SingleOrDefault() == null)
                this.Add(new LsmAcceptCharAction());
        }
    }
}
