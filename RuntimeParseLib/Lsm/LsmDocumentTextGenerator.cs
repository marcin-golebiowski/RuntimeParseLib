using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RuntimeParseLib.Lsm
{
    public class LsmDocumentTextGenerator
    {
        TextWriter _writer;
        public LsmDocumentTextGenerator(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteDocument(LsmDocument document)
        {
            _writer.WriteLine("Entry State: " + document.StartState.ID);
            foreach (LsmState state in document.States)
                WriteState(state);
        }

        private void WriteState(LsmState state)
        {
            _writer.WriteLine(state.ToString());
            foreach (LsmMatchRule matchRule in state.MatchRules)
                WriteMatchRule(matchRule);

            _writer.WriteLine("    Default");
            foreach (LsmAction defaultAction in state.DefaultActions)
                _writer.WriteLine("        " + defaultAction.ToString());


            _writer.WriteLine("    Post Stream");
            foreach (LsmAction postStreamAction in state.PostStreamActions)
                _writer.WriteLine("        " + postStreamAction.ToString());
        }

        private void WriteMatchRule(LsmMatchRule rule)
        {
            _writer.WriteLine("    " + rule.ToString());
            foreach (LsmAction action in rule.Actions)
                _writer.WriteLine("        " + action.ToString());
        }
    }
}
