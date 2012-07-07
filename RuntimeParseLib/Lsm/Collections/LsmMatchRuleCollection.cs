﻿/*
 * LsmMatchRuleCollection.cs
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
    public class LsmMatchRuleCollection : ObservableCollection<LsmMatchRule>, ILsmExpressable
    {
        LsmState _state;

        public LsmMatchRuleCollection(LsmState state)
        {
            _state = state;
        }

        public Expression GetExpression(LsmContext lsmContext)
        {
            if (this.Count == 0)
                return _state.DefaultActions.GetExpression(lsmContext);

            IEnumerable<ILsmExpressable> matchRules 
                = (from matchRule 
                       in this
                       where (matchRule is ILsmExpressable)
                       orderby matchRule.GetEvaluationOrder() ascending
                       select matchRule as ILsmExpressable);

            return LsmCommonExpressions.IfElseChain(
                matchRules,
                _state.DefaultActions.GetExpression(lsmContext),      
                lsmContext);
        }

        //Method untested.
        public LsmConstantMatchRule GetRuleByConstant(char character)
        {
            foreach (LsmMatchRule matchRule in this)
            {
                LsmConstantMatchRule constantRule = matchRule as LsmConstantMatchRule;
                if (constantRule == null)
                    continue;

                if (constantRule.Constant == character)
                    return constantRule;
            }

            return null;
        }
    }
}
