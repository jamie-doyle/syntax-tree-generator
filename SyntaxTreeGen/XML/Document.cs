using System;
using System.IO;
using System.Xml;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML.Parsers;

namespace SyntaxTreeGen.XML
{
    /// <summary>
    /// An XML document
    /// </summary>
    public class Document
    {
        private readonly string _docPath;

        public Document(string path)
        {
            _docPath = path;

            // check XML doc is valid
            _docPath = Path.GetFullPath(_docPath);

            if (Path.GetExtension(_docPath).ToLower() != ".xml")
                throw new ArgumentException("The file \"" + _docPath + "\" is not an XML document.");

            // Try loading the XML doc to check it's valid
            try
            {
                var doc = new XmlDocument();
                doc.Load(_docPath);
            }
            catch (XmlException)
            {
                throw new XmlException("The given file's XML is not valid.");
            }
        }

        /// <summary>
        /// Get the ClassNode and its child nodes, as represented by this Document
        /// </summary>
        /// <returns>Top node (e.g., ClassNode) generated from the XML</returns>
        public ClassNode GetNodes()
        {
            // head stores the parsed class and its children
            ClassNode head = null;

            // Create a new reader, configured to ignore comments, whitespace and directives 
            var settings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            };
            
            using (var reader = XmlReader.Create(_docPath, settings))
            {
                // Move reader to outer class statement 
                reader.Read();

                // Do parsing
                if (reader.Name.ToLower().Trim() == "class")
                {
                    var parser = new ClassParser(reader);
                    head = (ClassNode) parser.Result;
                }
                else
                {
                    throw new ArgumentException("The given XML file doesn't contain a class");
                }
            }

            return head;
        }
    }
}