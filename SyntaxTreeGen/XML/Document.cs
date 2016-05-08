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
        private ClassNode _head;
        
        /// <summary>
        /// Create a new parsing document
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">The file isn't an XML document</exception>
        /// <exception cref="XmlException">The file doesn't contain valid XML</exception>
        /// <exception cref="UnauthorizedAccessException">The file couldn't be accessed</exception>
        public Document(string path)
        {
            _docPath = path;

            // check XML doc is valid
            _docPath = Path.GetFullPath(_docPath);

            if (Path.GetExtension(_docPath).ToLower() != ".xml")
                throw new ArgumentException("the file \"" + _docPath + "\" is not an XML document.");

            // Try loading the XML doc to check it's valid
            try
            {
                var doc = new XmlDocument();
                doc.Load(_docPath);
            }
            catch (XmlException)
            {
                throw new XmlException("the given file's XML is not valid.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("this file appears to be read-only.");
            }
        }

        /// <summary>
        /// Get the ClassNode and its child nodes, as represented by this Document
        /// </summary>
        /// <returns>Top node (e.g., ClassNode) generated from the XML</returns>
        public ClassNode GetNodes()
        {
            // If we've already parsed this file, return the results (no need to re-parse the same thing)
            if (_head != null)
                return _head;
            
            // Create a new reader, configured to ignore comments, whitespace and directives 
            var settings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            };
            
            using (var reader = XmlReader.Create(_docPath, settings))
            {
                // Advance reader until "class" is reached
                while (reader.Read())
                {
                    // Continue looping if this isn't a class
                    if (reader.Name.ToLower().Trim() != "class") continue;

                    // otherwise, parse class
                    var parser = new ClassParser(reader);
                    _head = (ClassNode) parser.Result;
                }
                
                // If head isn't set, throw exception
                if (_head == null)
                    throw new XmlException("the given XML file doesn't contain a valid class"); 
            }

            return _head;
        }
    }
}