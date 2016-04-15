using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    class VarParser : Parser
    {
        internal VarParser(XmlReader reader) : base(reader, "variable")
        {
            var tmp = new VarNode();

            // While not at a </variable>
            while (!((reader.NodeType == XmlNodeType.EndElement) && (reader.Name.Equals("method"))))
            {
                // Get the attribute type, then advance to the data
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "name":
                        Reader.Read();
                        tmp.Info = reader.Value;
                        ReadEndTag(attribute);
                        break;
                    case "type":
                        reader.Read();
                        tmp.NodeType = reader.Value;
                        ReadEndTag(attribute);
                        break;
                    default:
                        throw new XmlException("Invalid subnode");
                }
            }

            ReadEndTag("variable");
        }
    }
}
