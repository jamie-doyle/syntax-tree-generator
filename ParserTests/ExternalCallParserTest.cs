using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class ExternalCallParserTest
    {
        // class XML that should work - System.Console.WriteLine( "TEST" )
        private const string GoodXml = "<externalCall><isStatic>true</isStatic><qualifier>System</qualifier><qualifier>Console</qualifier><qualifier>WriteLine</qualifier><parameters><constant><value>\"TEST\"</value></constant></parameters></externalCall>";

        // wrong type of XML
        private const string BadXml = "<wrongtag> </wrongtag>";

        //but unclosed tag in child element
        private const string UnclosedNameXml = "<externalCall><isStatic>true</isStatic><qualifier>System<qualifier>Console</qualifier><qualifier>WriteLine</qualifier><parameters><constant><value>\"TEST\"</value></constant></parameters></externalCall>";

        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();
            var result = (ExternalClassNode)new ExternalCallParser(_reader).Result;

            // Should create "System.Console.WriteLine"
            Assert.AreEqual("System", result.Qualifiers[0]);
            Assert.AreEqual("Console", result.Qualifiers[1]);
            Assert.AreEqual("WriteLine", result.Qualifiers[2]);

            // Should have param, constant value "TEST"
            Assert.AreEqual("\"TEST\"", ((ConstantNode)result.Parameters[0]).ConstVal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (ExternalClassNode)new ExternalCallParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "the tag qualifier was not closed")]
        public void TestUnclosedQualifierXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedNameXml));
            _reader.Read();

            // should throw an exception
            var result = (ExternalClassNode)new ExternalCallParser(_reader).Result;
        }
    }
}