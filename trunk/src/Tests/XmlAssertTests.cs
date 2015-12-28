using System;
using Cint.XmlAsserter.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Threading;
using System.Globalization;

namespace Cint.XmlAsserter.Tests
{
    [TestFixture]
    public class XmlAssertFixture
    {
        private string bigXml =
            @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<report xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
<survey>
<id>66666</id>
<name>My Survey</name>
</survey>
<issue>
<issuer>John Doe</issuer>
<date>2007-10-15</date>
</issue>
<period>
<start>2007-10-01</start>
<end>2007-10-07</end>
<week>42</week>
</period>

<questions>
<question xsi:type=""basicQuestionSingleChoice"" > 
<text>The Basic Single text</text> 
<helpText>The Basic Single HELP text</helpText>
<id>Q1</id>
<alternatives>
<alternative>
<text>Alt1</text>
<value>1</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<value>2</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</question>

<question xsi:type=""basicQuestionMultiChoice"" >
<text>The Basic Multi text</text>
<helpText>The Basic Multi HELP text</helpText> 
<alternatives>
<alternative> 
<text>Alt1</text> 
<id>Q2_1</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<id>Q2_2</id>
<answers></answers>
</alternative>
<alternative> 
<text>Alt3</text> 
<id>Q2_3</id>
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives> 
</question>

<question xsi:type=""matrixQuestionSingleChoice"" >
<text>The Matrix Single text</text>
<helpText>The Matrix Single HELP text</helpText> 
<row>
<text>row text</text>
<id>Q3_1</id>
<alternatives>
<alternative>
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>2</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<id>Q3_2</id>
<alternatives>
<alternative> 
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>1</value> 
<answers></answers>
</alternative>
</alternatives> 
</row>
</question>

<question xsi:type=""matrixQuestionMultiChoice"" >
<text>The Matrix Multi text</text>
<helpText>The Matrix Multi HELP text</helpText> 
<row>
<text>row text</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_1_1</id>
<answers></answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_1_2</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_2_1</id>
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_2_2</id>
<answers>
<respondent id=""123"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</row>
</question> 

<question xsi:type=""labeledFreeTextQuestion"" >
<text>What do you think of these beers?</text>
<helpText>Help text for LabeledFreeText</helpText> 
<field> 
<DEFANGED_label>Samuel Adams</DEFANGED_label>
<id>Q5_1</id>
<alternatives>
<alternative>
<text>Ž„„„ckligt!</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Skitgott f”r fan!</text> 
<answers> 
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Lite sm†gott</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field> 
<DEFANGED_label>Elefant”l</DEFANGED_label>
<id>Q5_2</id>
<alternatives>
<alternative>
<text>Jovars...</text> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Fy fan!</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative> 
</alternatives>
</field>
</question>

<question xsi:type=""freeTextQuestion"" >
<text>Rank your favorite beers.</text>
<helpText>Help text for FreeText</helpText> 
<field>
<position>1</position>
<id>Q6_1</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Norrlands Guld</text> 
<answers>
<respondent id=""234"" /> 
</answers>
</alternative>
<alternative>
<text>Maredsous</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field>
<position>2</position>
<id>Q6_2</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Carlsberg</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" /> 
</answers>
</alternative> 
</alternatives>
</field>
</question> 
</questions>
<respondents>
<respondent>
<id>123</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>234</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>345</id> 
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent> 
</respondents>
</report>";

        private string bigSlightlyDifferentXml =
            @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<report xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
<survey>
<id>66666</id>
<name>My Survey</name>
</survey>
<issue>
<issuer>John Doe</issuer>
<date>2007-10-15</date>
</issue>
<period>
<start>2007-10-01</start>
<end>2007-10-07</end>
<week>42</week>
</period>

<questions>
<question xsi:type=""basicQuestionSingleChoice"" > 
<text>The Basic Single text</text> 
<helpText>The Basic Single HELP text</helpText>
<id>Q1</id>
<alternatives>
<alternative>
<text>Alt1</text>
<value>1</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<value>2</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</question>

<question xsi:type=""basicQuestionMultiChoice"" >
<text>The Basic Multi text</text>
<helpText>The Basic Multi HELP text</helpText> 
<alternatives>
<alternative> 
<text>Alt1</text> 
<id>Q2_1</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<id>Q2_2</id>
<answers></answers>
</alternative>
<alternative> 
<text>Alt3</text> 
<id>Q2_3</id>
<answers>
<respondent id=""used to be 123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives> 
</question>

<question xsi:type=""matrixQuestionSingleChoice"" >
<text>The Matrix Single text</text>
<helpText>The Matrix Single HELP text</helpText> 
<row>
<text>row text</text>
<id>Q3_1</id>
<alternatives>
<alternative> 
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>2</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<id>Q3_2</id>
<alternatives>
<alternative> 
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>1</value> 
<answers></answers>
</alternative>
</alternatives> 
</row>
</question>

<question xsi:type=""matrixQuestionMultiChoice"" >
<text>The Matrix Multi text</text>
<helpText>The Matrix Multi HELP text</helpText> 
<row>
<text>row text</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_1_1</id>
<answers></answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_1_2</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_2_1</id>
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_2_2</id>
<answers>
<respondent id=""123"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</row>
</question> 

<question xsi:type=""labeledFreeTextQuestion"" >
<text>What do you think of these beers?</text>
<helpText>Help text for LabeledFreeText</helpText> 
<field> 
<DEFANGED_label>Samuel Adams</DEFANGED_label>
<id>Q5_1</id>
<alternatives>
<alternative>
<text>Ž„„„ckligt!</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Skitgott f”r fan!</text> 
<answers> 
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Lite sm†gott</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field> 
<DEFANGED_label>Elefant”l</DEFANGED_label>
<id>Q5_2</id>
<alternatives>
<alternative>
<text>Jovars...</text> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Fy fan!</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative> 
</alternatives>
</field>
</question>

<question xsi:type=""freeTextQuestion"" >
<text>Rank your favorite beers.</text>
<helpText>Help text for FreeText</helpText> 
<field>
<position>1</position>
<id>Q6_1</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Norrlands Guld</text> 
<answers>
<respondent id=""234"" /> 
</answers>
</alternative>
<alternative>
<text>Maredsous</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field>
<position>2</position>
<id>Q6_2</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Carlsberg</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" /> 
</answers>
</alternative> 
</alternatives>
</field>
</question> 
</questions>
<respondents>
<respondent>
<id>123</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>234</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>345</id> 
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent> 
</respondents>
</report>";

        private string bigXmlWithLessWhitespace =
            @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<report xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
<survey><id>66666</id>
<name>My Survey</name>
</survey>
<issue><issuer>John Doe</issuer>
<date>2007-10-15</date>
</issue><period>
<start>2007-10-01</start>
<end>2007-10-07</end>
<week>42</week>
</period><questions>
<question xsi:type=""basicQuestionSingleChoice"" > 
<text>The Basic Single text</text> 
<helpText>The Basic Single HELP text</helpText>
<id>Q1</id>
<alternatives>
<alternative>
<text>Alt1</text>
<value>1</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<value>2</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</question>
<question xsi:type=""basicQuestionMultiChoice"" >
<text>The Basic Multi text</text>
<helpText>The Basic Multi HELP text</helpText> 
<alternatives>
<alternative> 
<text>Alt1</text> 
<id>Q2_1</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<id>Q2_2</id><answers></answers>
</alternative>
<alternative> 
<text>Alt3</text> 
<id>Q2_3</id>
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives> 
</question>
<question xsi:type=""matrixQuestionSingleChoice"" ><text>The Matrix Single text</text>
<helpText>The Matrix Single HELP text</helpText> 
<row>
<text>row text</text>
<id>Q3_1</id>
<alternatives>
<alternative> 
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>2</value> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<id>Q3_2</id>
<alternatives>
<alternative> 
<text>Alt1</text> 
<value>1</value> 
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative>
<alternative> 
<text>Alt2</text> 
<value>1</value> 
<answers></answers>
</alternative>
</alternatives> 
</row>
</question>

<question xsi:type=""matrixQuestionMultiChoice"" >
<text>The Matrix Multi text</text>
<helpText>The Matrix Multi HELP text</helpText> 
<row>
<text>row text</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_1_1</id>
<answers></answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_1_2</id>
<answers>
<respondent id=""123"" />
</answers>
</alternative>
</alternatives>
</row>
<row>
<text>row text2</text>
<alternatives>
<alternative>
<text>Alt1</text>
<id>Q4_2_1</id>
<answers>
<respondent id=""123"" />
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Alt2</text>
<id>Q4_2_2</id>
<answers>
<respondent id=""123"" />
<respondent id=""345"" />
</answers>
</alternative>
</alternatives>
</row>
</question> 

<question xsi:type=""labeledFreeTextQuestion"" >
<text>What do you think of these beers?</text>
<helpText>Help text for LabeledFreeText</helpText> 
<field> 
<DEFANGED_label>Samuel Adams</DEFANGED_label>
<id>Q5_1</id>
<alternatives>
<alternative>
<text>Ž„„„ckligt!</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Skitgott f”r fan!</text> 
<answers> 
<respondent id=""234"" />
</answers>
</alternative>
<alternative>
<text>Lite sm†gott</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field> 
<DEFANGED_label>Elefant”l</DEFANGED_label>
<id>Q5_2</id>
<alternatives>
<alternative>
<text>Jovars...</text> 
<answers>
<respondent id=""123"" />
</answers>
</alternative>
<alternative>
<text>Fy fan!</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" />
</answers>
</alternative> 
</alternatives>
</field>
</question>

<question xsi:type=""freeTextQuestion"" >
<text>Rank your favorite beers.</text>
<helpText>Help text for FreeText</helpText> 
<field>
<position>1</position>
<id>Q6_1</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Norrlands Guld</text> 
<answers>
<respondent id=""234"" /> 
</answers>
</alternative>
<alternative>
<text>Maredsous</text> 
<answers>
<respondent id=""345"" /> 
</answers>
</alternative>
</alternatives>
</field>
<field>
<position>2</position>
<id>Q6_2</id> 
<alternatives>
<alternative>
<text>Guinness</text> 
<answers>
<respondent id=""123"" /> 
</answers>
</alternative>
<alternative>
<text>Carlsberg</text> 
<answers>
<respondent id=""234"" />
<respondent id=""345"" /> 
</answers>
</alternative> 
</alternatives>
</field>
</question> 
</questions>
<respondents>
<respondent>
<id>123</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>234</id>
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent>
<respondent>
<id>345</id> 
<started>2007-10-09T01:02:03</started>
<completed>2007-10-09T02:03:04</completed>
</respondent> 
</respondents>
</report>
";

        private void AssertAndVerifyXmlAssertMessage(string expected, string actual, string expectedMessageFromXmlAssert)
        {
            try
            {
                XmlAssert.AreEqual(expected, actual);
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo(expectedMessageFromXmlAssert));
            }
        }

        [Test]
        public void AsserterShouldBeAbleToDistinguishTagNames()
        {
            string input =
                @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Story xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                  <Abbreviation>FT-1</Abbreviation>
                                </Story>";
            string testAgainst =
                @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Story xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                  <Abbriviation>FT-1</Abbriviation>
                                </Story>";

            try
            {
                XmlAssert.AreEqual(input, testAgainst);
                throw new Exception("XmlAssert.AreEqual should have thrown.");
            }
            catch (AssertionException)
            {
            }
        }

        [Test]
        public void AttributesInDifferentOrderShouldBeOk()
        {
            string xmlSingleElementWithAttributes1 = @"<root attr1='attr1' attr2='attr2' attr3='attr3'/>";
            string xmlSingleElementWithAttributes2 = @"<root attr1='attr1' attr3='attr3' attr2='attr2'/>";
            XmlAssert.AreEqual(xmlSingleElementWithAttributes1, xmlSingleElementWithAttributes2);
        }

        [Test]
        public void BadElementAttribute()
        {
            AssertAndVerifyXmlAssertMessage(
                "<xml attribute='attr1'/>"
                , "<xml attribute='attr'/>"
                , "Attribute values (attr1) and (attr) differ in element (xml)");
        }

        [Test]
        public void BadElementValue()
        {
            AssertAndVerifyXmlAssertMessage("<xml>Element</xml>"
                                            , "<xml>Element1</xml>"
                                            , "Expected node value (Element) but was (Element1)");
        }

        [Test]
        public void BigXmlDocumentIsSameAsSelfWithLotsOfWhitespaceRemoved()
        {
            XmlAssert.AreEqual(bigXml, bigXmlWithLessWhitespace);
        }

        [Test]
        public void BigXmlDocumentIsSameWithSelf()
        {
            XmlAssert.AreEqual(bigXml, bigXml);
        }

        [Test]
        public void BigXmlDocumentsDiffer()
        {
            AssertAndVerifyXmlAssertMessage(
                bigXml
                , bigSlightlyDifferentXml
                , "Attribute values (123) and (used to be 123) differ in element (respondent)");
        }

        [Test]
        public void CheckErrorMessageWithNoRoot()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            AssertAndVerifyXmlAssertMessage("", "<>", "Root element is missing.");
        }

        [Test]
        public void DifferentAmountOfChildNodesShouldBeDetected()
        {
            string xml = "<xml><child1/><child2/><child3/></xml>"
                   ,
                   other = "<xml><child1/><child2/></xml>";
            AssertAndVerifyXmlAssertMessage(xml, other,
                                            "xml doesn't have the same amount of child nodes as xml (expected: 3, got: 2)");
            AssertAndVerifyXmlAssertMessage(other, xml,
                                            "xml doesn't have the same amount of child nodes as xml (expected: 2, got: 3)");
        }

        [Test]
        public void DifferentNamespacesAreDetected()
        {
            string xmlWithNamespaces =
                @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
                <report xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
                </report>";
            string xmlWithoutNamespaces =
                @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
                <report xmlns:xsi=""http://www.w3.org/2002/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
                </report>";

            AssertAndVerifyXmlAssertMessage(
                xmlWithNamespaces
                , xmlWithoutNamespaces
                ,
                "Attribute values (http://www.w3.org/2001/XMLSchema-instance) and (http://www.w3.org/2002/XMLSchema-instance) differ in element (report)");
        }

        [Test]
        public void MissingNamespacesAreDetected()
        {
            string xmlWithNamespaces =
                @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
                <report xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""CintSchema.xsd"">
                </report>";
            string xmlWithoutNamespaces =
                @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
                <report>
                </report>";
            AssertAndVerifyXmlAssertMessage(
                xmlWithNamespaces
                , xmlWithoutNamespaces
                , "Number of attributes differ in node (report)");
        }

        [Test]
        public void SameNumberOfAttributesButNotSameAttributesShouldNotBeOk()
        {
            string xmlSingleElementWithAttributes1 = @"<root attr1='attr' attr2='attr2'/>";
            string xmlSingleElementWithAttributes2 = @"<root attr1='attr' attr3='attr3'/>";
            AssertAndVerifyXmlAssertMessage(
                xmlSingleElementWithAttributes1
                , xmlSingleElementWithAttributes2
                , "(attr2) should exist in both nodes but it does not.");
        }

        [Test]
        public void WhitespaceShouldNotMatter()
        {
            XmlAssert.AreEqual("<xml></xml>", "<xml />");
            XmlAssert.AreEqual("<xml></xml>", "<xml></xml>");
            XmlAssert.AreEqual("<xml></xml>", "<xml></xml> ");
            XmlAssert.AreEqual("<xml></xml>", " <xml></xml> ");
            XmlAssert.AreEqual("<xml></xml>", " <xml> </xml> ");
            XmlAssert.AreEqual("<xml></xml>", " <xml > </xml> ");
            XmlAssert.AreEqual("<xml></xml>", " <xml > </xml > ");
            XmlAssert.AreEqual("<xml></xml>", " <xml >  </xml > ");
            XmlAssert.AreEqual("<xml><tag/></xml>", " <xml > <tag /> </xml > ");
            XmlAssert.AreEqual("<xml><tag attribute='attribute'/></xml>",
                               " <xml > <tag attribute ='attribute' /> </xml > ");
            XmlAssert.AreEqual("<xml><tag attribute='attribute'/></xml>",
                               " <xml > <tag attribute = 'attribute' /> </xml > ");
            XmlAssert.AreEqual("<xml><tag attribute='attribute'/></xml>",
                               @" <xml > 
<tag attribute = 'attribute' /> 
</xml > ");
        }
    }
}