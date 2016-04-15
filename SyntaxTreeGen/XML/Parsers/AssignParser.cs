using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class AssignParser : Parser
    {
        internal AssignParser(XmlReader reader) : base(reader, "assign")
        {
            // Get left (assignment target)
            if (Reader.Name.ToLower().Trim() != "variable")
                throw Exception.Generate(Reader, Exception.ErrorType.BadAssignLeft);
            
            var left = new VarParser(Reader).Result;
            
            // Get right (assignment value)
            Node right;
            Parser parser;
            
            switch (Reader.Name.ToLower().Trim())
            {
                case "variable":
                    parser = new VarParser(Reader);
                    right = parser.Result;
                    ReadEndTag("variable");
                    break;
                case "constant":
                    parser = new ConstantParser(Reader);
                    right = parser.Result;
                    ReadEndTag("constant");
                    break;
                case "operation":
                    // TODO OperationParser(Reader);
                    //right = parser.Result;
                    ReadEndTag("variable");
                    break;
                default:
                    throw Exception.Generate(reader, Exception.ErrorType.BadAssignRight);
            }
            
            ReadEndTag("assign");
            Result = new AssignNode(left, right);

        }
    }
}
