using NUnit.Framework;
using FluentAssertions;
using System.IO;

namespace Arthware.X937Parser.Tests
{
    [TestFixture]
    public abstract class X937ParserTests
    {
        private X937Parser _sut;
        private Stream _stream;

        [SetUp]
        public void PrepareX937ParserTests()
        {
            _stream = new MemoryStream();
            _sut = new X937Parser();
        }

        public sealed class GetX937ReturnsMethod : X937ParserTests
        {
            [Test]
            public void Should_do_jack_when_there_are_no_bytes_in_the_stream()
            {
                //act
                var result = _sut.GetX937Returns(_stream);

                //assert
                result.Should().BeEmpty();
            }
        }
    }
}
