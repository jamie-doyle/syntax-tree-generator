using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// An XML document
    /// </summary>
    class XmlSyntaxTree
    {
        /// <summary>
        /// The parent of the node(s) represented by the given XML
        /// </summary>
        public Node BaseNode { get; private set; }

        /// <summary>
        /// Deserialize the program represented 
        /// </summary>
        /// <param name="documentPath">Path to the XML document to use</param>
        /// <returns>Top node (e.g., ClassNode) generated from the XML. NULL if XML contains no Node</returns>
        public Node Import (string documentPath)
        {
            // check XML doc is valid
            var directory = Path.GetFullPath(documentPath);
            
            if (Path.GetExtension(directory)?.ToLower() != ".xml")
                throw new ArgumentException("The file \"" + documentPath +  "\" is not an XML document.");

            // Try loading the XML doc to check it's valid before parsing it
            try
            {
                var doc = new XmlDocument();
                doc.Load(directory);
            }
            catch (XmlException)
            {
                throw new XmlException("The given file's XML is not valid.");
            }
            
            // Get nodes from XML
            Node head = null;
            
            using (var reader = new XmlTextReader(documentPath) { WhitespaceHandling = WhitespaceHandling.None })
            {
                // All 


                //while (reader.Read())
                //{ 
                    // If this is the first class found, create node
                    if (head == null)
                        head = ParseClass(reader);
                //}

            }

            return head;
        }

        /// <summary>
        /// Parses XML data to a ClassNode
        /// </summary>
        /// <param name="reader">XmlReader containing XML stream to get Class from</param>
        /// <returns></returns>
        private static ClassNode ParseClass(XmlReader reader)
        {
            var tmp = new ClassNode();

            // Read class attributes
            while (reader.Read())
            {
                // exit while if we've reached end of the class
                if (reader.Name.ToLower() == "class" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                // skip comments
                if (reader.NodeType == XmlNodeType.Comment)
                    continue;

                // Get the attribute type, then advance to the data
                var attribute = reader.Name.ToLower();
                reader.Read();

                switch (attribute)
                {
                    case "accesslevel":
                        tmp.ClassAccessLevel = GetAccessLevel(reader.Value);
                        break;
                    case "name":
                        tmp.Info = reader.Value;
                        break;
                    case "isstatic":
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "method":
                        // TODO parse methods
                        var method = ParseMethod(reader);
                        tmp.AddSubnode(method);
                        break;
                    default:
                        throw new InvalidDataException();
                }

                // Get end tag of this element
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }

            return tmp;
        }

        private static MethodNode ParseMethod(XmlReader reader)
        {
            var tmp = new MethodNode();

            while (reader.Name.ToLower() != "method" && reader.NodeType != XmlNodeType.EndElement)
            {
                // If we aren't at an open element tag, advance reader
                if (reader.NodeType != XmlNodeType.Element)
                    reader.Read();

                // skip comments
                if (reader.NodeType == XmlNodeType.Comment)
                    reader.Read();

                // Get the attribute type, then advance to the data
                var attribute = reader.Name.ToLower();
                reader.Read();

                switch (attribute)
                {
                    case "accesslevel":
                        tmp.MethodAccessLevel = GetAccessLevel(reader.Value);
                        break;
                    case "isstatic":
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "name":
                        tmp.Info = reader.Value;
                        break;
                    case "parameter":
                        // Parameters stored in left subnode set
                        var param = ParseVar(reader);
                        tmp.Subnodes.First().AddSubnode(param);
                        break;
                    // TODO Method statements
                }

                // Get end tag of this element
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }
            
            return tmp;
        }

        /// <summary>
        /// Parses variables or parameters
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static VarNode ParseVar(XmlReader reader)
        {
            var tmp = new VarNode();

            while (reader.Read())
            {
                // Exit if variable/parameter ended
                if (reader.Name.ToLower() == "method" )
                    if (reader.NodeType == XmlNodeType.EndElement)
                    break;

                // skip comments
                if (reader.NodeType == XmlNodeType.Comment)
                    continue;

                // Get the attribute type, then advance to the data
                var attribute = reader.Name.ToLower();
                reader.Read();

                switch (attribute)
                {
                    case "name":
                        tmp.Info = reader.Value;
                        break;
                    case "type":
                        tmp.Info = reader.Value;
                        break;
                }

                // advance reader to end tag
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }

            return tmp;
        }

        private static Node.AccessLevel GetAccessLevel(string parsed)
        {
            switch (parsed.ToLower().Trim())
            {
                case "private":
                    return Node.AccessLevel.Private;
                case "public":
                    return Node.AccessLevel.Public;
                case "internal":
                    return Node.AccessLevel.Public;
                case "protected":
                    return Node.AccessLevel.Protected;
            }

            throw new InvalidDataException("\"" + parsed + "\" is not a valid access level");
        }
    }
}