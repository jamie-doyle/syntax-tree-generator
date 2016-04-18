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
        /// <summary>
        /// Parses lists of statements to a NodeListNode
        /// </summary>
        /// <param name="xmlreader"></param>
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
                    case "variable":
                        Result.AddSubnode(new VarParser(Reader).Result);
                        break;
                    case "if":
                        Result.AddSubnode(new IfParser(Reader).Result);
                        break;
                    case "while":
                        Result.AddSubnode(new WhileParser(Reader).Result);
                        break;
                    case "externalcall":
                        Result.AddSubnode(new ExternalCallParser(Reader).Result);
                        break;
                    default:
                        throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                }
                
            }

            ReadEndTag("statements");
        }
    }
}