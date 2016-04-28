using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class OperationParserTest
    {
        // Working XML - should parse to "(x == 5)"
        private const string GoodXml = "<operation><variable><name>x</name></variable><operator>==</operator><constant><value>5</value></constant></operation>";
        
        // wrong type of XML
        private const string BadXml = "<wrongtag> </wrongtag>";

        // Correct XML, other than an unclosed "parameters" tag 
        private const string UnclosedTagXml = "<operation><variable><name>x</name></variable><operator>==<constant><value>5</value></constant></operation>";
        
        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();

            // parse results
            var result = (OperationNode)new OperationParser(_reader).Result;
            
            // Signature of the test method should match that below
            Assert.AreEqual("(x == 5)", result.ToString().Trim());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (OperationNode)new OperationParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedTagXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedTagXml));
            _reader.Read();

            // should throw an exception
            var result = (OperationNode)new OperationParser(_reader).Result;
        }
    }
}