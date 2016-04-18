using System;
using System.Text;

namespace SyntaxTreeGen.Models
{
    class IfNode: Node
    {
        private static int _depth;
        
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
            _depth++;
            var sb = new StringBuilder();
            
            sb.Append("if (" + Subnodes[0] + ")");
            sb.AppendLine();

            sb.Append(Tab(_depth - 1));
            sb.Append("{");
            sb.AppendLine();

            sb.Append(Tab(_depth));
            sb.Append(Subnodes[1]);

            sb.AppendLine();
            sb.Append(Tab(_depth - 1));
            sb.Append("}");

            // if three subnodes, add else part
            if (Subnodes.Count > 2)
            {
                sb.AppendLine();
                sb.Append(Tab(_depth - 1));
                sb.Append("else");
                sb.AppendLine();
                sb.Append("{");
                sb.AppendLine();

                sb.Append(Tab(_depth));
                sb.Append(Subnodes[2]);

                sb.AppendLine();
                sb.Append(Tab(_depth - 1));
                sb.Append("}");
            }

            _depth--;

            return sb.ToString();
        }

        private string Tab(int i)
        {
            var x = "";

            while (i > 0)
            {
                x += "  ";
                i--;
            }

            return x;
        }
    }
}
