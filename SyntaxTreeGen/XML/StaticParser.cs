using System;
using System.IO;
using System.Linq;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML
{
    internal static class StaticParser
    {
        /// <summary>
        /// Parses XML data to a ClassNode
        /// </summary>
        /// <param name="reader">Reader pointed to a "<class>...</class>" or its initial descendant</param>
        /// <returns></returns>
        internal static ClassNode ParseClass(XmlReader reader)
        {
            // Expecting a <class> node
            if (reader.Name.ToLower().Trim() != "class")
                throw new ArgumentException("Reader isn't pointed to a <class> node ");

            var tmp = new ClassNode();

            // Keep parsing until we reach a "</class>" tag
            while (reader.Read())
            {
                // Get the attribute type
                var attribute = reader.Name.ToLower();

                switch (attribute)
                {
                    case "accesslevel":
                        reader.Read();
                        tmp.ClassAccessLevel = Node.GetAccessLevel(reader.Value);
                        break;
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "isstatic":
                        reader.Read();
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "method":
                        // Method handles entire <method> block, so go to next subnode after parsing 
                        var method = ParseMethod(reader);
                        tmp.AddSubnode(method);
                        continue;

                    // todo extend to support internal classes (only supports one class per file now)
                    case "class":
                        return tmp;

                    default:
                        throw new InvalidDataException();
                }

                // Get end tag of this element
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }

            return tmp;
        }

        /// <summary>
        /// Parses methods and their subnodes
        /// </summary>
        /// <param name="reader">Reader pointed to a "<method></method>" or its initial descendant</param>
        /// <returns></returns>
        internal static MethodNode ParseMethod(XmlReader reader)
        {
            // if we're on a <method> tag, advance the reader
            if (reader.Name.ToLower().Trim() != "method")
                throw new ArgumentException("<method> tag expected");

            var tmp = new MethodNode();

            // While not at a "</method>" tag
            while (reader.Read())
            {
                // skip comments
                if (reader.NodeType == XmlNodeType.Comment)
                    continue;

                // Get the attribute type
                var attribute = reader.Name.ToLower();

                switch (attribute)
                {
                    case "accesslevel":
                        // advance to data and read it
                        reader.Read();
                        tmp.MethodAccessLevel = Node.GetAccessLevel(reader.Value);
                        break;
                    case "isstatic":
                        reader.Read();
                        tmp.IsStatic = bool.Parse(reader.Value);
                        break;
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "parameter":
                        var param = ParseVar(reader);
                        // Add new parameter to existing 
                        tmp.Parameters = tmp.Parameters.Concat(new[] { param });
                        continue;

                    case "returntype":
                        reader.Read();
                        tmp.ReturnType = reader.Value;
                        break;

                    case "statements":
                        if (tmp.Statements.Any())
                            throw Exception.Generate(reader, Exception.ErrorType.Statements);

                        var res = ParseStatements(reader);
                        tmp.Statements = res.Subnodes;
                        break;

                    // Exit when close method reached
                    case "method":
                        return tmp;
                }

                // Get end tag of this element
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw new XmlException("A tag of type <" + attribute + "> was not closed");
            }

            return tmp;
        }

        internal static Node ParseStatements(XmlReader reader)
        {
            if (reader.Name.ToLower().Trim() != "statements")
                throw new ArgumentException("Reader should point to <statements> node");

            var tmp = new NodeListNode();

            while (reader.Read())
            {
                // get node type
                var kind = reader.Name.ToLower().Trim();
                switch (kind)
                {
                    case "assign":
                        tmp.AddSubnode(ParseAssign(reader));
                        continue;
                    case "call":
                        // todo ParseCall(reader);
                        continue;
                    case "if":
                        // todo ParseIf(reader);
                        continue;
                    case "variable":
                        var res = ParseVar(reader);
                        tmp.AddSubnode(res);
                        continue;
                    case "statements":
                        return tmp;
                        // todo a loop node (e.g. while)
                }
            }

            throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);
        }

        /// <summary>
        /// Parses variables or parameters
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static VarNode ParseVar(XmlReader reader)
        {
            // Expecting a reader pointed to a <variable> or <parameter>
            if (reader.Name.ToLower().Trim() != "variable" && reader.Name.ToLower().Trim() != "parameter")
                throw new XmlException("Reader should point to a root <variable> or <parameter> node");

            var tmp = new VarNode();

            while (reader.Read())
            {
                // Get the attribute type, then advance to the data
                var attribute = reader.Name.ToLower();

                switch (attribute)
                {
                    case "name":
                        reader.Read();
                        tmp.Info = reader.Value;
                        break;
                    case "type":
                        reader.Read();
                        tmp.NodeType = reader.Value;
                        break;
                    case "parameter":
                    case "variable":
                        return tmp;
                    default:
                        throw new XmlException("Invalid subnode");
                }

                // advance reader to end tag
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);
            }

            throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);
        }

        /// <summary>
        /// Parses constant values
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static ConstantNode ParseConstant(XmlReader reader)
        {
            // Expecting a reader pointed to a <constant>
            if (reader.Name.ToLower().Trim() != "constant")
                throw new ArgumentException("Reader should point to <constant>");

            var tmp = new ConstantNode();
            while (reader.Read())
            {
                switch (reader.Name.ToLower().Trim())
                {
                    case "value":
                        reader.Read();
                        tmp.ConstVal = reader.Read();
                        break;
                    case "constant":
                        if (tmp.ConstVal != null)
                            return tmp;
                        throw Exception.Generate(reader, Exception.ErrorType.NoValue);
                }
            }
            throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);
        }

        internal static AssignNode ParseAssign(XmlReader reader)
        {
            // Expecting a reader pointed to <assign>
            if (reader.Name.ToLower().Trim() != "assign")
                throw new ArgumentException("Reader should point to a root <assign> node");

            // Elements in AssignNode must have left first; no read loop needed
            // Get left (assignment target)
            reader.Read();
            if (reader.Name.ToLower().Trim() != "variable")
                throw Exception.Generate(reader, Exception.ErrorType.BadAssignLeft);
            var left = ParseVar(reader);

            // Get right (assignment value)
            reader.Read();
            Node right;

            switch (reader.Name.ToLower().Trim())
            {
                case "variable":
                    right = ParseVar(reader);
                    break;
                case "constant":
                    right = ParseConstant(reader);
                    break;
                default:
                    throw Exception.Generate(reader, Exception.ErrorType.BadAssignRight);
            }

            return new AssignNode(left, right);
        }


        internal static OperatorNode ParseOperator(XmlReader reader)
        {
            if (reader.Name.ToLower().Trim() != "operation")
                throw new ArgumentException("Reader should point to <operation> node");

            Node left = null, right = null;
            OperatorNode.OpKind opkind = 0;

            OperatorNode res = new OperatorNode();

            while (reader.Read())
            {
                Node tmp;
                // get node type
                var kind = reader.Name.ToLower().Trim();
                switch (kind)
                {
                    case "variable":
                        tmp = ParseVar(reader);
                        break;
                    case "constant":
                        tmp = ParseConstant(reader);
                        break;
                    case "operation":
                        tmp = ParseOperator(reader);
                        break;
                    case "operator":
                        if (res.Op > 0)
                            throw Exception.Generate(reader, Exception.ErrorType.NoOperator);
                        reader.Read();
                        opkind = OperatorNode.GetOperator(reader.Value);

                        // Close tag
                        reader.Read();
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() != "operator")
                            throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);

                        continue;


                    default:
                        throw Exception.Generate(reader, Exception.ErrorType.UnknownSubnode);
                }

                if (left == null)
                    left = res;
                else if (right == null)
                    right = res;
                else
                    throw Exception.Generate(reader, Exception.ErrorType.NoOperator);
            }

            throw Exception.Generate(reader, Exception.ErrorType.UnclosedTag);
        }
    }
}