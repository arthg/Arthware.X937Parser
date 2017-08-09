using Arthware.X937Parser.Models;
using System.Collections.Generic;
using System.IO;

namespace Arthware.X937Parser
{
    public interface IX937FileParser
    {
        IEnumerable<X937Return> GetX937Returns(Stream stream);
    }
}
