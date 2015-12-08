using System;
using System.Text;

namespace SyntaxTreeGen.Models
{
    class IfNode: Node
    {
        private static int _depth;

        // TODO: How will ELSE be represented? Can this node have a third branch?
        public IfNode (Node con, Node body) : base(con, body)
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
            
            sb.Append("if (" + Left + ")");
            sb.AppendLine();

            sb.Append(Tab(_depth - 1));
            sb.Append("{");
            sb.AppendLine();

            sb.Append(Tab(_depth));
            sb.Append(Right);

            sb.AppendLine();
            sb.Append(Tab(_depth - 1));
            sb.Append("}");

            _depth--;

            return sb.ToString();
        }

        private string Tab(int i)
        {
            var x = "";

            while (i > 0)
            {
                x += "\t";
                i--;
            }

            return x;
        }
    }
}
