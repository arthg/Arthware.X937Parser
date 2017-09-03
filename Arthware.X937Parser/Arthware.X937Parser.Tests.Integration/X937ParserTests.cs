using NUnit.Framework;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace Arthware.X937Parser.Tests.Integration
{
    public abstract class X937ParserTests
    {
        private const string SAMPLE_FORWARD_PRESENTMENT_X9_FILE = @"SampleX9ReturnFiles\Sample.x9";
        //this file  not included in version control:  may contain production information
        private const string SAMPLE_MULTIPLE_RETURN_X9_FILE = @"SampleX9ReturnFiles\BT302IMGBO.CACHETRETURN.20170208123227.x937";

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
            public void Should_interogate_the_file_and_emit_no_returns_when_the_X9_file_is_for_forward_presentment()
            {
                //arrange
                using (var filestream = new FileStream(SAMPLE_FORWARD_PRESENTMENT_X9_FILE, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //act
                    var returns = _sut.GetX937Returns(filestream).ToArray();

                    //assert
                    returns.Should().BeEmpty();
                }
            }

            [Test]
            public void Should_emit_all_the_returns_when_multiple_returns_are_present_on_the_X9_file()
            {
                //arrange
                using (var filestream = new FileStream(SAMPLE_MULTIPLE_RETURN_X9_FILE, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //act
                    var returns = _sut.GetX937Returns(filestream).ToArray();

                    //assert
                    returns.Count().Should().Be(2);
                }
            }
        }
    }
}
