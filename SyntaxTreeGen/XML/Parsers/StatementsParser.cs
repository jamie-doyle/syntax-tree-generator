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
        internal StatementsParser(XmlReader xmlreader) : base(xmlreader, "statements")
        {
            Result = new NodeListNode();

            // While not at a </statements> tag
            while (!((Reader.NodeType == XmlNodeType.EndElement) && (Reader.Name.Equals("statements"))))
            {
                // get node type
                var kind = Reader.Name.ToLower().Trim();
                switch (kind)
                {
                    case "assign":
                        Result.AddSubnode(new AssignParser(Reader).Result);
                        break;
                    case "operation":
                        Result.AddSubnode(new OperationParser(Reader).Result);
                        break;

                    case "call":
                        // todo ParseCall(reader);
                        continue;
                    case "if":
                        // todo ParseIf(reader);
                        continue;
                    case "variable":
                        Result.AddSubnode(new VarParser(Reader).Result);
                        continue;
                    
                    // todo a loop node (e.g. while)
                }
                
            }

            ReadEndTag("statements");
        }
    }
}