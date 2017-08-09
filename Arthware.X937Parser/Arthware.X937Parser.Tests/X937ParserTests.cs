using NUnit.Framework;
using System.IO;

namespace Arthware.X937Parser.Tests
{
    [TestFixture]
    public abstract class X937ParserTests
    {
        private X937Parser _x937Parser;
        private Stream _stream;

        [SetUp]
        public void PrepareX937ParserTests()
        {
            _stream = new MemoryStream();
            _x937Parser = new X937Parser();
        }

        public sealed class GetX937ReturnsMethod : X937ParserTests
        {
            [Test]
            public void Should_()
            {
                //arrange
                //act
                var result = _x937Parser.GetX937Returns(_stream);

                //assert
            }
        }
    }
}
