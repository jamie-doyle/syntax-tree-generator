﻿using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class IfParser : Parser
    {
        internal IfParser(XmlReader xmlReader) : base(xmlReader, "if")
        {
            Node condition = null;
            Node body = null;
            Node elseBody = null;
            
            // while not </if>
            while (!(Reader.NodeType == XmlNodeType.EndElement && Reader.Name.Equals("if")))
            {
                // Internal parsers will advance reader
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

                    case "else":
                        // Move to <statements>
                        Reader.Read();
                        elseBody = new StatementsParser(Reader).Result;
                        // get end
                        ReadEndTag("else");
                        break;

                    default:
                        throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                }  
            }

            ReadEndTag("if");

            if (condition == null)
                throw Exception.Generate(Reader, Exception.ErrorType.InvalidIf);

            Result = elseBody != null ? new IfNode(condition, body, elseBody) : new IfNode(condition, body);
        }
    }
}