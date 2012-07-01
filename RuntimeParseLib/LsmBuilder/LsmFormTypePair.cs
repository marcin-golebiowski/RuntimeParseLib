using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuntimeParseLib.LsmBuilder
{
    class LsmTextTypePair
    {
        string _text;
        int _TypeID;

        public LsmTextTypePair(string text, int typeID)
        {
            _text = text;
            _TypeID = typeID;
        }

        public string Text
        {
            get { return _text; }
        }

        public int TypeID
        {
            get { return _TypeID; }
        }
    }
}
