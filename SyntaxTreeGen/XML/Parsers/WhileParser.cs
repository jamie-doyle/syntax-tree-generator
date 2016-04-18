using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class WhileParser : Parser
    {
        internal WhileParser(XmlReader xmlreader) : base(xmlreader, "while")
        {
            Node condition = null;
            Node body = null;

            while (!(Reader.NodeType == XmlNodeType.EndElement && Reader.Name.Equals("while")))
            {
                switch (Reader.Name.ToLower().Trim())
                {
                    case "condition":
                        Reader.Read();
                        condition = new OperationParser(Reader).Result;
                        ReadEndTag("condition");
                        break;

                    case "body":
                        // Move to <statements>
                        Reader.Read();
                        body = new StatementsParser(Reader).Result;
                        // read end
                        ReadEndTag("body");
                        break;
                }
            }

            Result = new WhileNode(condition, body);
            ReadEndTag("while");
        }

    }
}
