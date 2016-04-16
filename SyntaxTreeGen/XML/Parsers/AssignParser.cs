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
            
            switch (Reader.Name.ToLower().Trim())
            {
                case "variable":
                    right = new VarParser(Reader).Result;
                    break;
                case "constant":
                    right = new ConstantParser(Reader).Result;
                    break;
                case "operation":
                    right = new OperationParser(Reader).Result;
                    break;
                default:
                    throw Exception.Generate(reader, Exception.ErrorType.BadAssignRight);
            }
            
            // close node and set result
            Reader.Read();
            ReadEndTag("assign");
            Result = new AssignNode(left, right);
        }
    }
}