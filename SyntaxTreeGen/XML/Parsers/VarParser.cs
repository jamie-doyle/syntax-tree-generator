using System.Xml;
using SyntaxTreeGen.Models;
namespace SyntaxTreeGen.XML.Parsers
{
    class VarParser : Parser
    {
        internal VarParser(XmlReader xmlreader) : base(xmlreader, "variable")
        {
            var tmp = new VarNode();

            // While not at a </variable>
            while (!((Reader.NodeType == XmlNodeType.EndElement) && (Reader.Name.Equals("variable"))))
            {
                // Get the attribute type, then advance to the data
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "name":
                        Reader.Read();
                        tmp.Info = Reader.Value;
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    case "type":
                        Reader.Read();
                        tmp.NodeType = Reader.Value;
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    case "isdeclaration":
                        Reader.Read();
                        tmp.IsDeclaration = bool.Parse(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    default:
                        throw new XmlException("Invalid subnode");
                }
            }

            Result = tmp;
            ReadEndTag("variable");
        }
    }
}
