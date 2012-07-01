

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using RuntimeParseLib.Lsm;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LsmBuilder builder = new LsmBuilder();

            builder.CreateStringTokenPath("H", 1, true);
            builder.CreateStringTokenPath("He", 2, true);
            builder.CreateStringTokenPath("Heck", 3, true);
            builder.CreateStringTokenPath("Hello", 4, true);
            builder.CreateStringTokenPath("{", 5, false);
            builder.CreateStringTokenPath("}", 6, false);
            builder.ApplyIdentifierPaths(-1);

            /*
            LsmDocument stateDoc = new LsmDocument();
            LsmState tokenStartState = new LsmState();
            LsmState consumeLetters = new LsmState();

            tokenStartState.DefaultActions.Add(new LsmMarkTokenStartAction());
            tokenStartState.DefaultActions.Add(new LsmStateTransitionAction(consumeLetters));
            tokenStartState.DefaultActions.Add(new LsmClearTokenTextAction());
            consumeLetters.PostStreamActions.Add(new LsmAcceptTokenAction(0));

            LsmLambdaMatchRule letterRule = new LsmLambdaMatchRule((c) => char.IsLetter(c));
            letterRule.Actions.Add(new LsmAdvanceAction());
            letterRule.Actions.Add(new LsmAcceptCharAction());

            consumeLetters.DefaultActions.Add(new LsmAcceptTokenAction(0));
            consumeLetters.DefaultActions.Add(new LsmStateTransitionAction(tokenStartState));
            consumeLetters.DefaultActions.Add(new LsmAdvanceAction());

            consumeLetters.MatchRules.Add(letterRule);
            stateDoc.States.Add(tokenStartState);
            stateDoc.States.Add(consumeLetters);
            stateDoc.StartState = tokenStartState;
            */

            Func<TextReader, List<LsmToken>> parseFunc
                = builder.Document.BuildLexerFunction();

            List<LsmToken> tokens;

            while (true)
            {
                string stuff = Console.ReadLine();
                using (StringReader reader = new StringReader(stuff))
                    tokens = parseFunc(reader);

                foreach (LsmToken token in tokens)
                {
                    Console.WriteLine(token.Text + " : " + token.TypeID + " : " + token.CharIndex);
                }

            }
        }
    }
}
