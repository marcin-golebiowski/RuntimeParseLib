using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RuntimeParseLib.Lsm
{
    public delegate int LsmTokenizeFunction(TextReader reader, List<LsmToken> tokens);
}
