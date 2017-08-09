using System;
using System.Collections.Generic;
using System.IO;
using Arthware.X937Parser.Models;

namespace Arthware.X937Parser
{
    public sealed class X937Parser : IX937FileParser
    {
        public IEnumerable<X937Return> GetX937Returns(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
