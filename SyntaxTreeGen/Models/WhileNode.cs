using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    class WhileNode : Node
    {
        public WhileNode(Node con, Node body) : base(2, con, body)
        {
            if (con == null)
                throw new InvalidOperationException(
                    "A \"while\" node requires a termination condition.");

            Info = "while";
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Margin.Tab());
            sb.Append("while (" + Subnodes[0] + ")");
            sb.AppendLine();

            sb.Append(Margin.Tab());
            sb.Append("{");
            sb.AppendLine();
            
            Margin.Indent();
            sb.Append(Subnodes[1]);
            Margin.Outdent();
            
            sb.AppendLine();
            sb.Append(Margin.Tab() + "}");

            return sb.ToString();
        }
    }
}
