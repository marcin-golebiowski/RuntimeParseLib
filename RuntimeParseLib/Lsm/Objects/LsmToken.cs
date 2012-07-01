/*
 * LsmToken.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuntimeParseLib.Lsm
{
    public class LsmToken
    {
        int _typeID;
        string _text;
        int _charIndex;

        public LsmToken(int typeID, string text, int charIndex)
        {
            _typeID = typeID;
            _text = text;
            _charIndex = charIndex;
        }

        public int TypeID
        {
            get { return _typeID; }
        }

        public int CharIndex
        {
            get { return _charIndex; }
        }

        public string Text
        {
            get { return _text; }
        }
    }
}
