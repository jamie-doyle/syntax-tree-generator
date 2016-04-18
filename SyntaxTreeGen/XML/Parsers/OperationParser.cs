using System;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class OperationParser : Parser
    {
        /// <summary>
        /// Parses an <Operation>...</Operation> node to an OperatorNode object
        /// </summary>
        /// <param name="xmlreader"></param>
        internal OperationParser(XmlReader xmlreader) : base(xmlreader, "operation")
        {
            // Get left operand
            var left = ParseOperand();

            // Check an operator is defined, build an exception if not
            if (!(Reader.Name == "operator" && Reader.NodeType == XmlNodeType.Element))
                throw Exception.Generate(Reader, Exception.ErrorType.NoOperator); 
            
            // Get the operator
            Reader.Read();
            var parsedOp = OperatorNode.GetOperator(Reader.Value);
            Reader.Read();
            ReadEndTag("operator");

            // Get right operand
            var right = ParseOperand();

            // Close "Operation" 
            ReadEndTag("operation");

            // Set result
            Result = new OperatorNode(left, parsedOp, right);
        }

        private Node ParseOperand()
        {
            Node tmp;
            var kind = Reader.Name.ToLower();

            switch (kind)
            {
                case "variable":
                    tmp = new VarParser(Reader).Result;
                    break;
                case "constant":
                    tmp = new ConstantParser(Reader).Result;
                    break;
                case "operation":
                    tmp = new OperationParser(Reader).Result;
                    break;
                default:
                    throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
            }

            return tmp;
        }
    }
}
