using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class MethodParserTest
    {
        // working XML, parses to "private void Main (int x) { }"
        private const string GoodXml = "<method><accessLevel>private</accessLevel><isStatic>true</isStatic><returnType>void</returnType><name>Main</name><parameters><variable><name>x</name><type>int</type></variable></parameters><statements></statements></method>";
       
        // wrong type of XML
        private const string BadXml = "<wrongtag> </wrongtag>";

        // Correct XML, other than an unclosed "parameters" tag 
        private const string UnclosedTagXml = "<method><accessLevel>private</accessLevel><isStatic>true</isStatic><returnType>void</returnType><name>Main</name><parameters><variable><name>x</name><type>int</type></variable><statements></statements></method>";
        
        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();

            // parse results
            var result = (MethodNode)new MethodParser(_reader).Result;

            // Get the first line
            var firstLine = result.ToString().Substring(0, result.ToString().IndexOf('\n'));

            // Signature of the test method should match that below
            Assert.AreEqual("private static void Main (int x)", firstLine.Trim());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (MethodNode)new MethodParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedTagXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedTagXml));
            _reader.Read();

            // should throw an exception
            var result = (MethodNode)new MethodParser(_reader).Result;
        }
    }
}