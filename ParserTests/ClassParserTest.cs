using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace ParserTests
{
    [TestClass]
    public class ClassParserTest
    {
        // class XML that should work
        private const string GoodXml = "<class><accessLevel>public</accessLevel><isStatic>true</isStatic><name>Test</name></class>";

        // Valid XML, but totally wrong for a class parser
        private const string BadXml = "<wrong> </wrong>";

        // Mostly valid, but missing </class>
        private const string UnclosedClassXml = "<class><accessLevel>public</accessLevel><isStatic>true</isStatic><name>Test</name>";

        // mostly valid, but unclosed tag in child element
        private const string UnclosedNameXml = "<class><accessLevel>public</accessLevel><isStatic>true</isStatic><name>Test</class>";

        private XmlReader _reader;
        
        [TestMethod]
        public void TestGoodXml()
        {
            // Try reading correct XML
            _reader = XmlReader.Create(new StringReader(GoodXml));
            _reader.Read();
            var result = (ClassNode)new ClassParser(_reader).Result;

            // Should create a Class "public static class Test () { }"
            Assert.AreEqual(Node.AccessLevel.Public, result.ClassAccessLevel);
            Assert.AreEqual(true, result.IsStatic);
            Assert.AreEqual("Test", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(BadXml));
            _reader.Read();

            // should throw an exception
            var result = (ClassNode)new ClassParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void TestUnclosedClassXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedClassXml));
            _reader.Read();

            // should throw an exception
            var result = (ClassNode)new ClassParser(_reader).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), "the tag name was not closed")]
        public void TestUnclosedNameXml()
        {
            // load bad xml to reader
            _reader = XmlReader.Create(new StringReader(UnclosedNameXml));
            _reader.Read();

            // should throw an exception
            var result = (ClassNode)new ClassParser(_reader).Result;
        }
    }
}