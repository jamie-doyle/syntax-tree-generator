using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A node representing an assignment operation.
    /// </summary>
    class AssignNode : Node
    {
        /// <summary>
        /// The assignment charater to use
        /// </summary>
        private readonly string _assignChar = "=";

        /// <summary>
        /// Create a new assign node 
        /// </summary>
        /// <param name="left">Left side of the assignment</param>
        /// <param name="right">Right side of the assignment</param>
        public AssignNode(Node left, Node right) : base(2, left, right)
        {
            Info = _assignChar;
        }

        public override string ToString()
        {
            return Subnodes[0] + " " + Info + " " + Subnodes[1] + ";";
        }
    }
}
