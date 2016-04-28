using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class WhileParserTest
    {
        // correct XML representation of "while (x == 5) { // Empty Statement Block }"
        private const string WhileXml = "<while><condition><operation><variable><name>x</name></variable><operator>==</operator><constant><value>5</value></constant></operation></condition><body><statements></statements></body></while>";
        
        // wrong type of XML
        private const string BadXml = "<if> </if>";

        // Correct XML, other than a missing </while> 
        private const string UnclosedTagXml = "<while><condition><operation><variable><name>x</name></variable><operator>==</operator><constant><value>5</value></constant></operation></condition><body><statements></statements></body>";
        
        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodWhileXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(WhileXml));
            _reader.Read();

            // parse results
            var result = (WhileNode)new WhileParser(_reader).Result;

            // Get the first line
            var firstLine = result.ToString().Substring(0, result.ToString().IndexOf('\n'));
            
            // Expecting while's signature to contain the correct condition
            Assert.AreEqual("while ((x == 5))", firstLine.Trim());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (VarNode)new VarParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedTagXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedTagXml));
            _reader.Read();

            // should throw an exception
            var result = (VarNode)new VarParser(_reader).Result;
        }
    }
}