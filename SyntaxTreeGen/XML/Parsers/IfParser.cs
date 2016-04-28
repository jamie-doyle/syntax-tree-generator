using System;
using System.Xml;
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

                        // Dermine condition type
                        switch (Reader.Name.ToLower().Trim())
                        {
                            case "operation":
                                condition = new OperationParser(Reader).Result;
                                break;
                            // Accept constants as a condition - assume it's a valid type such as boolean
                            case "constant":
                                condition = new ConstantParser(Reader).Result;
                                break;
                            default:
                                throw Exception.Generate(Reader, Exception.ErrorType.Conditions);
                        }

                        // Close the condition section
                        ReadEndTag("condition");
                        break;

                    case "body":
                        // Move to <statements>
                        try
                        {
                            Reader.Read();
                            body = new StatementsParser(Reader).Result;
                            // read end
                            ReadEndTag("body");
                        }
                        catch (ArgumentException)
                        {
                            throw Exception.Generate(Reader, Exception.ErrorType.Statements);
                        }
                        break;

                    case "else":
                        // Move to <statements>
                        try
                        {
                            Reader.Read();
                            elseBody = new StatementsParser(Reader).Result;
                            // read end
                            ReadEndTag("else");
                        }
                        catch (ArgumentException)
                        {
                            throw Exception.Generate(Reader, Exception.ErrorType.Statements);
                        }
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