using Conefabric.FairyTales.Web.Controls;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class TitleAbbreviatorTests
    {
        [Test]
        public void ShouldDivideOnWhitespace()
        {
            var maxlenght = 5;
            var abbreviator = new TitleAbbreviator(maxlenght);
            var title =
                abbreviator.Abbreviate("A story");
            Assert.That(title, Is.EqualTo("A ..."));
        }

        [Test]
        public void ShouldOnlyAbbreviateOnLenghtsGreaterThanMax()
        {
            var what = "A story";
            var abbreviator = new TitleAbbreviator(what.Length);
            var title = abbreviator.Abbreviate(what);
            Assert.That(title, Is.EqualTo("A story"));
        }

        [Test]
        public void ShouldAbbreviateEvenOnNoWhiteSpaces()
        {
            var title = new TitleAbbreviator(4).Abbreviate("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Assert.That(title, Is.EqualTo("aaaa ..."));
        }

        [Test]
        public void ShouldDivideOnSecondWhiteSpaceAsWell()
        {
            var maxlenght = 10;
            var abbreviator = new TitleAbbreviator(maxlenght);
            var title = abbreviator.Abbreviate("My title that should be abbreviated");
            Assert.That(title, Is.EqualTo("My title ..."));
        }
    }
}