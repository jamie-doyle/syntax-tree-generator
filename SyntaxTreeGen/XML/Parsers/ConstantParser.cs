using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    /// <summary>
    /// Parses constant nodes
    /// </summary>
    internal class ConstantParser : Parser
    {
        internal ConstantParser(XmlReader reader) : base(reader, "constant")
        {
            var tmp = new ConstantNode();

            // subnode should be "value"
            if (Reader.Name.ToLower() != "value")
                throw Exception.Generate(Reader, Exception.ErrorType.NoValue);

            // Read value of <value>
            Reader.Read();
            tmp.ConstVal = reader.Value;
            Reader.Read();
            ReadEndTag("value");
            
            // Get </constant> end tag
            ReadEndTag("constant");

            Result = tmp;
        }
    }
}
