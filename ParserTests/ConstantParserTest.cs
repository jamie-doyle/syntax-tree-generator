using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class ConstantParserTest
    {
        // class XML that should work
        private const string GoodXml = "<constant><value>5</value></constant>";

        // incorrect xml
        private const string BadXml = "<notConst> </notConst>";

        // Mostly valid, but missing </const>
        private const string UnclosedClassXml = "<constant><value>5</value>";

        // mostly valid, but unclosed tag in child element
        private const string UnclosedNameXml = "<constant><value>5</constant>";

        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();
            var result = (ConstantNode)new ConstantParser(_reader).Result;

            // Should create a Class "public static class Test () { }"
            Assert.AreEqual("5", (string)result.ConstVal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (ConstantNode)new ConstantParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedConstantXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedClassXml));
            _reader.Read();

            // should throw an exception
            var result = (ConstantNode)new ConstantParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "the tag name was not closed")]
        public void TestUnclosedNameXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedNameXml));
            _reader.Read();

            // should throw an exception
            var result = (ConstantNode)new ConstantParser(_reader).Result;
        }
    }
}