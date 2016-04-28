using System;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class AssignParserTest
    {
        // class XML that should work: int i = 1
        
        private const string GoodXml = "<assign><variable><name>i</name><type>int</type></variable><constant><value>1</value></constant></assign>";

        // wrong for this parser
        private const string BadXml = "<wrong></wrong>";
        
        // Mostly valid, but unclosed
        private const string UnclosedClassXml = "<assign><variable><name>i</name><type>int</type></variable><constant><value>1</value></constant>";

        // mostly valid, but unclosed tag in child element
        private const string UnclosedConstantXml = "<assign><variable><name>i</name><type>int</type></variable> <constant><value>1</value></assign>";

        // Invalid - assignment to a constant (i.e.: "1 = int i;")
        private const string BadAssignXml = "<assign><const><value>1</value></const></variable><name>i</name><type>int</type><variable></assign>";
        
        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();
            var result = (AssignNode)new AssignParser(_reader).Result;

            // Should create "i = 1";
            Assert.AreEqual("i = 1;", result.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (AssignNode)new AssignParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedAssignXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedClassXml));
            _reader.Read();

            // should throw an exception
            var result = (AssignNode)new AssignParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "the tag name was not closed")]
        public void TestUnclosedConstantXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedConstantXml));
            _reader.Read();

            // should throw an exception
            var result = (AssignNode)new AssignParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "assignment must be to a variable")]
        public void TestBadAssignXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadAssignXml));
            _reader.Read();

            // should throw an exception
            var result = (AssignNode)new AssignParser(_reader).Result;
        }
    }
}