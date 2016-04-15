using System;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    abstract class Parser
    {
        protected XmlReader Reader;
        internal Node Result { get; set; }

        /// <summary>
        /// Base constructor for Parsers; should be called by all derived classes. Verifies the reader is 
        /// pointed to the opening tag of the parsable data, then advances past it to its first child element.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parser"></param>
        protected Parser(XmlReader reader, string parser)
        {
            Reader = reader;

            // Calling class's name should match tag name
            if (Reader.Name.ToLower().Trim() != parser)
                throw new ArgumentException("Reader should point to a <" + parser + "> node");
            
            // Move past opening node
            Reader.Read();
        }

        /// <summary>
        /// Verifies a node is closed and advances the given reader past it.
        /// </summary>
        /// <param name="tag">Name of end tag expected</param>
        protected void ReadEndTag(string tag)
        {
            // Verify the </tag> exists
            Reader.Read();
            if ( !(Reader.NodeType == XmlNodeType.EndElement &&  Reader.Name.ToLower() != tag) )
                throw Exception.Generate(Reader, Exception.ErrorType.UnclosedTag);

            // Move reader past end tag
            Reader.Read();
        }
    }
}