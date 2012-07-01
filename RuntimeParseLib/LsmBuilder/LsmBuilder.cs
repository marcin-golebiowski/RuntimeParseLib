//Todo: Must work on this class.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuntimeParseLib.Lsm
{
    public enum LsmTokenBehavior
    {
        Keyword,
        Symbol
    }

    public class LsmBuilder
    {
        LsmState rootState = new LsmState();
        LsmDocument document = new LsmDocument();
        List<LsmState> keywordEndStates = new List<LsmState>();
        //int identifierTokenTypeID = 0;

        public LsmBuilder()
        {
            document.States.Add(rootState);
            document.StartState = rootState;

            rootState.DefaultActions.Add(new LsmAdvanceAction());
        }

        public LsmDocument Document
        {
            get { return document; }
        }

        /*
        public int IdentifierTokenTypeID
        {
            get { return identifierTokenTypeID; }
            set { identifierTokenTypeID = value; }
        }*/

        public void ApplyIdentifierPaths(int tokenTypeID)
        {
            //LsmState identifierFirst = new LsmState();
            LsmState identifierRest = new LsmState();

            LsmLambdaMatchRule alphaNumericRule = new LsmLambdaMatchRule((c)=>char.IsLetterOrDigit(c));
            LsmLambdaMatchRule letterRule = new LsmLambdaMatchRule((c) => char.IsLetter(c));

            letterRule.Actions.Add(new LsmStateTransitionAction(identifierRest));
            letterRule.Actions.Add(new LsmMarkTokenStartAction());

            alphaNumericRule.Actions.Add(new LsmAcceptCharAction());
            alphaNumericRule.Actions.Add(new LsmAdvanceAction());
            alphaNumericRule.Actions.Add(new LsmStateTransitionAction(identifierRest));

            rootState.MatchRules.Add(letterRule);

            identifierRest.MatchRules.Add(alphaNumericRule);
            identifierRest.DefaultActions.Add(new LsmAcceptTokenAction(tokenTypeID));
            identifierRest.DefaultActions.Add(new LsmClearTokenTextAction());
            identifierRest.DefaultActions.Add(new LsmStateTransitionAction(rootState));
            identifierRest.PostStreamActions.Add(new LsmAcceptTokenAction(tokenTypeID));

            foreach (LsmState keywordEndState in keywordEndStates)
                keywordEndState.MatchRules.Add(alphaNumericRule);

            document.States.Add(identifierRest);
        }

        public LsmState CreateStringTokenPath(string tokenText, int typeID, bool isKeyword)
        {
            LsmState state = AppendStringTokenPath(rootState, tokenText, typeID);

            if (isKeyword)
                keywordEndStates.Add(state);

            return state;
        }

        public LsmState AppendStringTokenPath(LsmState start, string tokenText, int typeID)
        {
            LsmState currState = start;
            for (int i = 0; i < tokenText.Length; i++)
            {
                char character = tokenText[i];
                LsmState nextState;
                
                LsmConstantMatchRule constantRule
                    = currState.MatchRules.GetRuleByConstant(character);
                if (constantRule == null)
                {
                    constantRule = new LsmConstantMatchRule(character);

                    nextState = new LsmState();
                    document.States.Add(nextState);


                    if (i == 0)
                        constantRule.Actions.Add(new LsmMarkTokenStartAction());

                    constantRule.Actions.Add(new LsmAcceptCharAction());
                    constantRule.Actions.Add(new LsmAdvanceAction());
                    constantRule.Actions.Add(new LsmStateTransitionAction(nextState));

                    currState.MatchRules.Add(constantRule);
                }
                else
                    nextState = constantRule.Actions.GetTransitionDestination();

                if (i == tokenText.Length - 1)
                {
                    nextState.DefaultActions.Add(new LsmAcceptTokenAction(typeID));
                    nextState.DefaultActions.Add(new LsmClearTokenTextAction());
                    nextState.DefaultActions.Add(new LsmStateTransitionAction(rootState));

                    nextState.PostStreamActions.Add(new LsmAcceptTokenAction(typeID));
                }

                currState.DefaultActions.SetTransitionDestination(rootState);

                currState = nextState;
            }

            return currState;
        }
    }
}
