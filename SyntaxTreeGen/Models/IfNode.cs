using System;
using System.Text;

namespace SyntaxTreeGen.Models
{
    class IfNode: Node
    {
        /// <summary>
        /// Creates an if node
        /// </summary>
        /// <param name="con">Condition to be satisfied</param>
        /// <param name="body">nodes to execute where con evaluates to true</param>
        public IfNode(Node con, Node body) : base(2, con, body)
        {
            if (con == null)
                throw new InvalidOperationException(
                    "An \"if\" node requires a termination condition.");

            Info = "if";
        }

        /// <summary>
        /// Create a new if/else node
        /// </summary>
        /// <param name="con">Condition to be satisfied</param>
        /// <param name="body">Body to run if condition satisfied</param>
        /// <param name="elsepart">Runs where con is not satisfied</param>
        public IfNode (Node con, Node body, Node elsepart) : base(3, con, body, elsepart)
        {
            if (con == null)
                throw new InvalidOperationException(
                    "An \"if\" node requires a termination condition.");

            Info = "if";
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Margin.Tab());
            sb.Append("if (" + Subnodes[0] + ")");
            sb.AppendLine();

            sb.Append(Margin.Tab());
            sb.Append("{");
            sb.AppendLine();
            Margin.Indent();
            
            sb.Append(Subnodes[1]);
            
            sb.AppendLine();
            Margin.Outdent();
            sb.Append(Margin.Tab());
            sb.Append("}");

            // if three subnodes, add else part
            if (Subnodes.Count > 2)
            {
                sb.AppendLine();
                sb.Append(Margin.Tab());
                sb.Append("else");
                sb.AppendLine();
                sb.Append(Margin.Tab());
                sb.Append("{");
                sb.AppendLine();

                Margin.Indent();
                sb.Append(Subnodes[2]);

                sb.AppendLine();

                Margin.Outdent();
                sb.Append(Margin.Tab());
                sb.Append("}");
            }

            return sb.ToString();
        }

    }
}
