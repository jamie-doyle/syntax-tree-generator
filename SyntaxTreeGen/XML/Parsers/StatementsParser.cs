using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    class StatementsParser : Parser
    {
        internal StatementsParser(XmlReader reader) : base(reader, "statements")
        {
            Result = new NodeListNode();

            while (reader.Read())
            {
                // get node type
                var kind = reader.Name.ToLower().Trim();
                switch (kind)
                {
                    case "assign":
                        var parse = new AssignParser(reader);
                        Result.AddSubnode(parse.Result);
                        continue;
                    case "call":
                        // todo ParseCall(reader);
                        continue;
                    case "if":
                        // todo ParseIf(reader);
                        continue;
                    case "variable":
                        var parser = new VarParser(reader);
                        Result.AddSubnode(parser.Result);
                        continue;
                    
                    // todo a loop node (e.g. while)
                }
                
            }

            ReadEndTag("statements");
        }
    }
}