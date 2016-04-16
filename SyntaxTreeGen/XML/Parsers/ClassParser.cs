using System;
using System.IO;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    class ClassParser : Parser
    {
        /// <summary>
        /// Parses XML data to a ClassNode
        /// </summary>
        /// <param name="xmlreader">Reader pointed to a Class element</param>
        internal ClassParser(XmlReader xmlreader) : base (xmlreader, "class")
        {
            var tmp = new ClassNode();
            
            // While not on a "</class>" tag
            while (!((Reader.NodeType == XmlNodeType.EndElement) && (Reader.Name.Equals("class"))))
            {
                // Get the attribute type
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "accesslevel":
                        Reader.Read();
                        tmp.ClassAccessLevel = Node.GetAccessLevel(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    case "name":
                        Reader.Read();
                        tmp.Info = Reader.Value;
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    case "isstatic":
                        Reader.Read();
                        tmp.IsStatic = bool.Parse(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    case "method":
                        tmp.AddSubnode(new MethodParser(Reader).Result);
                        break;

                    // TODO extend to support internal classes (only supports one class per file now)
                    case "class":
                        throw new NotImplementedException();
                        
                    default:
                        throw new InvalidDataException();
                }  
            }
            
            ReadEndTag("class");
            Result = tmp;
        } 
    }
}