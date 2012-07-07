/*
 * LsmStatePathBuilder.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuntimeParseLib.Lsm
{
    public class LsmStatePathBuilder
    {
        LsmState _rootState = null;
        LsmState _lastState = null;
        LsmDocument _document = null;
        public LsmStatePathBuilder(LsmDocument document, LsmState rootState)
        {
            if (rootState == null)
                throw new ArgumentNullException("rootState");

            if (document == null)
                throw new ArgumentNullException("document");

            _document = document;
            _rootState = rootState;
            _lastState = rootState;
        }

        public void MoveToState(LsmState state)
        {
            _lastState = state;
        }

        public void AppendText(string text)
        {
            foreach (char character in text)
                AppendCharacter(character);
        }

        public void AppendCharacterClassLoop(LsmCharacterClass charClass)
        {
            LsmClassMatchRule constantRule
                = _lastState.MatchRules.GetRuleByClass(charClass);

            if (constantRule == null)
            {
                constantRule = new LsmClassMatchRule(charClass);

                _lastState.MatchRules.Add(constantRule);

                if (_lastState == _rootState)
                    constantRule.Actions.EnsureClearTokenAction();
            }

            constantRule.Actions.EnsureAcceptCharAction();
            constantRule.Actions.EnsureAdvanceAction();

            constantRule.Actions.SetTransitionDestination(_lastState);
        }

        public void AppendCharacterClass(LsmCharacterClass charClass)
        {
            LsmClassMatchRule constantRule
                = _lastState.MatchRules.GetRuleByClass(charClass);

            if (constantRule == null)
            {
                constantRule = new LsmClassMatchRule(charClass);

                _lastState.MatchRules.Add(constantRule);

                if (_lastState == _rootState)
                    constantRule.Actions.EnsureClearTokenAction();
            }

            constantRule.Actions.EnsureAcceptCharAction();
            constantRule.Actions.EnsureAdvanceAction();

            _lastState = constantRule.Actions.GetTransitionDestination();
            if (_lastState == null)
            {
                _lastState = new LsmState();
                _document.States.Add(_lastState);
                constantRule.Actions.SetTransitionDestination(_lastState);
            }
        }

        public void AppendCharacter(char character)
        {
            LsmConstantMatchRule constantRule
                = _lastState.MatchRules.GetRuleByConstant(character);

            if (constantRule == null)
            {
                constantRule = new LsmConstantMatchRule(character);

                _lastState.MatchRules.Add(constantRule);

                if (_lastState == _rootState)
                    constantRule.Actions.EnsureClearTokenAction();
            }

            constantRule.Actions.EnsureAcceptCharAction();
            constantRule.Actions.EnsureAdvanceAction();

            _lastState = constantRule.Actions.GetTransitionDestination();
            if (_lastState == null)
            {
                _lastState = new LsmState();
                _document.States.Add(_lastState);
                constantRule.Actions.SetTransitionDestination(_lastState);
            }
        }

        public void CompleteToken(int tokenID)
        {
            _lastState.DefaultActions.Add(new LsmAcceptTokenAction(tokenID));
            _lastState.DefaultActions.SetTransitionDestination(_rootState);

            _lastState.PostStreamActions.Add(new LsmAcceptTokenAction(tokenID));
        }
    }
}
