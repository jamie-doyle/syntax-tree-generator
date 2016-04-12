using System;
using System.Linq;
using System.IO;
using System.Xml;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// An XML document
    /// </summary>
    public static class XmlSyntaxTree
    {
        /// <summary>
        /// Deserialize the program represented 
        /// </summary>
        /// <param name="documentPath">Path to the XML document to use</param>
        /// <returns>Top node (e.g., ClassNode) generated from the XML. NULL if XML contains no Node</returns>
        public static Node Import (string documentPath)
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
                // Find the first outer class statement, parse it and its subnodes
                while (reader.Read())
                    if (reader.Name.ToLower().Trim() == "class")
                        head = ParseClass(reader);
                
                if (head == null)
                    throw new XmlException("The given XML file doesn't contain a class");
                
            }

            return head;
        }

        /// <summary>
        /// Parses XML data to a ClassNode
        /// </summary>
        /// <param name="reader">Reader pointed to a "<class>...</class>" or its initial descendant</param>
        /// <returns></returns>
        private static ClassNode ParseClass(XmlReader reader)
        {
            // Expecting a <class> node
            if (reader.Name.ToLower().Trim() != "class")
                throw new XmlException("Reader isn't pointed to a <class> node ");

            var tmp = new ClassNode();

            // Keep parsing until we reach a "</class>" tag
            while (reader.Read())
            {
                // Get the attribute type
                var attribute = reader.Name.ToLower();

                switch (attribute)
                {
                    case "accesslevel":
                        reader.Read();
                        tmp.ClassAccessLevel = GetAccessLevel(reader.Value);
                        break;
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "isstatic":
                        reader.Read();
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "method":
                        // Method handles entire <method> block, so go to next subnode after parsing 
                        var method = ParseMethod(reader);
                        tmp.AddSubnode(method);
                        continue;
                        
                    // todo extend to support internal classes (only supports one class per file now)
                    case "class":
                        return tmp;
                    
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
        
        /// <summary>
        /// Parses methods and their subnodes
        /// </summary>
        /// <param name="reader">Reader pointed to a "<method></method>" or its initial descendant</param>
        /// <returns></returns>
        private static MethodNode ParseMethod(XmlReader reader)
        {
            // if we're on a <method> tag, advance the reader
            if (reader.Name.ToLower().Trim() != "method")
                throw new XmlException("<method> tag expected");
               
            var tmp = new MethodNode();

            // While not at a "</method>" tag
            while (reader.Read())
            {
                // skip comments
                if (reader.NodeType == XmlNodeType.Comment)
                    continue;

                // Get the attribute type
                var attribute = reader.Name.ToLower();
                
                switch (attribute)
                {
                    case "accesslevel":
                        // advance to data and read it
                        reader.Read();
                        tmp.MethodAccessLevel = GetAccessLevel(reader.Value);
                        break;
                    case "isstatic":
                        reader.Read();
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "parameter":
                        var param = ParseVar(reader);
                        // Add new parameter to existing 
                        tmp.Parameters = tmp.Parameters.Concat(new[] {param});
                        continue;

                    case "returntype":
                        reader.Read();
                        tmp.ReturnType = reader.Value;
                        break;
                        
                    case "statements":
                        if (tmp.Statements.Any())
                            throw new XmlException("Methods cannot contain more than one set of statements");

                        var res = ParseStatements(reader);
                        tmp.Statements = res.Subnodes;
                        break;

                    // Exit when close method reached
                    case "method":
                        return tmp;
                }

                // Get end tag of this element
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement) 
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }
             
            return tmp;
        }

        private static Node ParseStatements(XmlReader reader)
        {
            if (reader.Name.ToLower().Trim() != "statements")
                throw new XmlException("Reader should point to a root <statements> node");

            var tmp = new NodeListNode();

            while (reader.Read())
            {
                // get node type
                var kind = reader.Name.ToLower().Trim();
                switch (kind)
                {
                    case "assign":
                        // todo ParseAssign(reader);
                        continue;
                    case "call":
                        // todo ParseCall(reader);
                        continue;
                    case "if":
                        // todo ParseIf(reader);
                        continue;
                    case "variable":
                        var res = ParseVar(reader);
                        tmp.AddSubnode(res);
                        continue;
                    case "statements":
                        return tmp;
                }
            }

            throw new XmlException("<statements> list was not closed");
        }

        /// <summary>
        /// Parses variables or parameters
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static VarNode ParseVar(XmlReader reader)
        {
            // Expecting a reader pointed to a <variable> or <parameter>
            if ( reader.Name.ToLower().Trim() != "variable" && reader.Name.ToLower().Trim() != "parameter")
                throw new XmlException("Reader should point to a root <variable> or <parameter> node");

            var tmp = new VarNode();
            
            while (reader.Read() ) 
            {
                // Get the attribute type, then advance to the data
                var attribute = reader.Name.ToLower();
                
                switch (attribute)
                {
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "type":
                        reader.Read();
                        tmp.NodeType = reader.Value;
                        break;
                    case "parameter":
                    case "variable":
                        return tmp;
                    default:
                        throw new XmlException("Invalid subnode");
                }

                // advance reader to end tag
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }

            return tmp;
        }

        private static AssignNode ParseAssign(XmlReader reader)
        {
            // Expecting a reader pointed to <assign>
            if (reader.Name.ToLower().Trim() != "assign")
                throw new XmlException("Reader should point to a root <assign> node");

            var tmp = new AssignNode();

            while (reader.Read())
            {
                switch (reader.Name.ToLower())
                {
                    //case "variable":

                }
            }

            return null;
        }

        /// <summary>
        /// Get the access level associated with a string
        /// </summary>
        /// <param name="parsed"></param>
        /// <returns></returns>
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