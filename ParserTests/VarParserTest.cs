using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class VarParserTest
    {
        // correct XML representation of "int x"
        private const string IntVarXml = "<variable><type>int</type><name>x</name></variable>";
        
        // wrong type of XML
        private const string BadXml = "<wrongtag> </wrongtag>";

        // Correct XML, other than an unclosed "name" tag 
        private const string UnclosedTagXml = "<variable><type>int</type><name>x</variable>";
        
        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodVariableXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(IntVarXml));
            _reader.Read();

            // parse results
            var result = (VarNode)new VarParser(_reader).Result;
            
            // Assert type and name
            Assert.AreEqual("int", result.NodeType);
            Assert.AreEqual("x", result.Info);
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