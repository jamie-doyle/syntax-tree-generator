using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class IfParserTest
    {
        // working XML - if ( TRUE ) { }
        private const string GoodXml = "<if><condition><constant><value>true</value></constant></condition><body><statements></statements></body></if>";
        
        // wrong type of XML
        private const string BadXml = "<wrongtag> </wrongtag>";

        // unclosed tag in child element
        private const string UnclosedTagXml = "<if><condition><constant><value>true</value></constant></condition><body><statements></statements></if>";

        // Missing end </if>
        private const string UnclosedIfXml = 
            "<if><condition><constant><value>true</value></constant></condition><body><statements></statements></body>";

        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();
            var result = (IfNode)new IfParser(_reader).Result;

            // Get the first line
            var firstLine = result.ToString().Substring(0, result.ToString().IndexOf('\n'));

            // Should be "if (true)". checking next line is redundant (will be empty as no body statements)
            Assert.AreEqual("if (true)", firstLine.Trim());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (IfNode)new IfParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "the tag body was not closed")]
        public void TestUnclosedTagXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedTagXml));
            _reader.Read();

            // should throw an exception
            var result = (IfNode)new IfParser(_reader).Result;
        }
    }
}