using System.Xml;
using NUnit.Framework;

namespace Cint.XmlAsserter.Core
{
    public class XmlAssert
    {
        private static string Message = "";

        public static void AreEqual(string expected, string actual)
        {
            var expectedDocument = new XmlDocument();
            expectedDocument.LoadXml(expected);
            var actualDocument = new XmlDocument();
            actualDocument.LoadXml(actual);
            for (int i = 0; i < expectedDocument.ChildNodes.Count; i++)
            {
                bool same = CompareNodes(expectedDocument.ChildNodes[i], actualDocument.ChildNodes[i]);
                if (!same)
                    Assert.Fail(Message);
            }
        }

        private static bool CompareNodes(XmlNode expectedNode, XmlNode actualNode)
        {
            if (expectedNode.Name != actualNode.Name)
                return
                    ReportError(string.Format("Expected node name ({0}) but was ({1})", expectedNode.Name,
                                              actualNode.Name));

            if (expectedNode.Value != actualNode.Value)
                return
                    ReportError(string.Format("Expected node value ({0}) but was ({1})", expectedNode.Value,
                                              actualNode.Value));

            if (expectedNode.NodeType == XmlNodeType.Element)
            {
                if (expectedNode.Attributes.Count != actualNode.Attributes.Count)
                    return ReportError(string.Format("Number of attributes differ in node ({0})", expectedNode.Name));

                if (expectedNode.Attributes.Count > 0)
                {
                    foreach (XmlAttribute attribute in expectedNode.Attributes)
                    {
                        XmlAttribute otherNodesAttribute = actualNode.Attributes[attribute.Name];
                        if (otherNodesAttribute == null)
                            return
                                ReportError(string.Format("({0}) should exist in both nodes but it does not.",
                                                          attribute.Name));
                        if (attribute.Value != otherNodesAttribute.Value)
                            return
                                ReportError(string.Format("Attribute values ({0}) and ({1}) differ in element ({2})",
                                                          attribute.Value, otherNodesAttribute.Value, expectedNode.Name));
                    }
                }
                if (expectedNode.HasChildNodes)
                {
                    if (expectedNode.ChildNodes.Count != actualNode.ChildNodes.Count)
                        return
                            ReportError(
                                string.Format("{0} doesn't have the same amount of child nodes as {1} (expected: {2}, got: {3})"
                                              , expectedNode.Name
                                              , actualNode.Name
                                              , expectedNode.ChildNodes.Count
                                              , actualNode.ChildNodes.Count
                                    ));

                    for (int i = 0; i < expectedNode.ChildNodes.Count; i++)
                    {
                        if (!CompareNodes(expectedNode.ChildNodes[i], actualNode.ChildNodes[i]))
                            return false;
                    }
                }
            }
            return true;
        }

        private static bool ReportError(string message)
        {
            Message = message;
            return false;
        }
    }
}