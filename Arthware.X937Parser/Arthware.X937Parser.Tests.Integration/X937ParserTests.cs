using NUnit.Framework;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace Arthware.X937Parser.Tests.Integration
{
    public abstract class X937ParserTests
    {
        private const string SAMPLE_MULTIPLE_RETURN_X9_FILE = @"SampleX9ReturnFiles\Sample.x9";

        private X937Parser _sut;

        [SetUp]
        public void PrepareX937ParserTests()
        {
            _sut = new X937Parser();
        }

        [TestFixture, Explicit]
        public sealed class GetX937ReturnsMethod : X937ParserTests
        {
            [Test]
            public void Should_interogate_the_file_for_returns_and_Xxx_when_the_X9_file_is_for_forward_presentment()
            {
                //arrange
                using (var filestream = new FileStream(SAMPLE_MULTIPLE_RETURN_X9_FILE, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //act
                    var returns = _sut.GetX937Returns(filestream).ToArray();

                    //assert
                    returns.Should().BeEmpty();
                }
            }
        }
    }
}
