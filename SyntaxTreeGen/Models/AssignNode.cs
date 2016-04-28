using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A node representing an assignment operation. 
    /// </summary>
    public class AssignNode : Node
    {
        /// <summary>
        /// The assignment charater to use
        /// </summary>
        private const string AssignChar = "=";
        public bool HasBeenAssigned { get; private set; }
         
        /// <summary>
        /// Create a new assign node 
        /// </summary>
        /// <param name="left">Left side of the assignment - the VarNode or ConstantNode to assign to</param>
        /// <param name="right">Right side of the assignment</param>
        public AssignNode(Node left, Node right) : base(2, left, right)
        {
            // Verify assignment is possible with the given parameters
            var leftType = left.GetType();
            var rightType = right.GetType();
            
            if (leftType != typeof(VarNode))
                throw new ArgumentException("Assignment must be to a VarNode.");

            if (rightType != typeof(VarNode) && rightType != typeof(ConstantNode) && rightType != typeof(OperationNode))
                throw new ArgumentException("Cannot assign from a "+rightType);

            Info = AssignChar;
        }

        public AssignNode()
        {
        }

        /// <summary>
        /// Returns a string representation of the assignment operation. The variable assigned by 
        /// this AssignNode is considered declared upon the first call of this method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            HasBeenAssigned = true;
            
            return Margin.Tab() + Subnodes[0] + " " + Info + " " + Subnodes[1] + ";";
        }
    }
}