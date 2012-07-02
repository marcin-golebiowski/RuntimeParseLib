/*
 * LsmChangeStateAction.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmStateTransitionAction : LsmAction, ILsmExpressable
    {
        LsmState _newState;

        public Expression GetExpression(LsmContext lsmContext)
        {
            return LsmCommonExpressions.ChangeState(lsmContext, _newState.ID);
        }

        public LsmStateTransitionAction(LsmState newState)
        {
            _newState = newState;
        }

        public LsmState Destination
        {
            get { return _newState; }
            set { _newState = value; }
        }

        public override string ToString()
        {
            return String.Format("Transition to State {0}", Destination.ID);
        }
    }
}
