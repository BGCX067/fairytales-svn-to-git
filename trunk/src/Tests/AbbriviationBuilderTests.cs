using System;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class AbbriviationBuilderTests
    {
        [Test]
        public void AbbreviationsShouldBeInUpperCase()
        {
            Assert.AreEqual("FT", AbbriviationBuilder.GetAbbriviation("fairy tales"));
            Assert.AreEqual("TRO", AbbriviationBuilder.GetAbbriviation("troy"));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void CastExceptionOnNullInput()
        {
            AbbriviationBuilder.GetAbbriviation(null);
        }

        [Test]
        public void DivideUpToThreeCharsIfWhiteSpaceExists()
        {
            Assert.AreEqual("FBB", AbbriviationBuilder.GetAbbriviation("Foo Bar Baz"));
        }

        [Test]
        public void ShouldAllowProjectsWithNamesLessThanThreeCharacters()
        {
            Assert.AreEqual("ST", AbbriviationBuilder.GetAbbriviation("st"));
            Assert.AreEqual("X", AbbriviationBuilder.GetAbbriviation("x"));
        }

        [Test]
        public void SubStringToThreeIfNoWhiteSpaces()
        {
            Assert.AreEqual("FOO", AbbriviationBuilder.GetAbbriviation("FooBar"));
        }

        [Test]
        public void NumbersWithPreceedingWhiteSpacesShouldNotBeTrunkated()
        {
            Assert.AreEqual("P11", AbbriviationBuilder.GetAbbriviation("Project 11"));
        }

        [Test]
        public void DashesShouldNotBeConsidered()
        {
            Assert.AreEqual("QAC", AbbriviationBuilder.GetAbbriviation("QA-CPX"));
        }

        [Test]
        public void DoubleWhiteSpaceShouldNotAffectOutput()
        {
            Assert.AreEqual("QC", AbbriviationBuilder.GetAbbriviation("QA  CPX"));
        }

    }
}