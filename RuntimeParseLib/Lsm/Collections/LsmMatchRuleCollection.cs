/*
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

        public LsmConstantMatchRule GetRuleByConstant(char character)
        {
            IEnumerable<LsmConstantMatchRule> constantMatchRules
                = (from matchRule
                   in this
                   where matchRule is LsmConstantMatchRule
                   select matchRule as LsmConstantMatchRule); 
           
            return constantMatchRules.SingleOrDefault(
                constantMatchRule => constantMatchRule.Constant == character);
        }

        //Method untested.
        public LsmClassMatchRule GetRuleByClass(LsmCharacterClass charClass)
        {
            IEnumerable<LsmClassMatchRule> classMatchRules
                = (from matchRule
                   in this
                   where matchRule is LsmClassMatchRule
                   select matchRule as LsmClassMatchRule);

            return classMatchRules.SingleOrDefault(
                classMatchRule => classMatchRule.CharacterClass == charClass);
        }
    }
}
