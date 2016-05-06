using System;
using System.Linq;
using System.Text;

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

            if (rightType != typeof(VarNode) && rightType != typeof(ConstantNode) && rightType != typeof(OperationNode) && rightType != typeof(ExternalCallNode)) 
                throw new ArgumentException("Cannot assign from a "+rightType);
            
            Info = AssignChar;
        }
        
        /// <summary>
        /// Returns a string representation of the assignment operation. The variable assigned by 
        /// this AssignNode is considered declared upon the first call of this method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var left = Subnodes.First();
            var right = Subnodes.Last();
            
            var sb = new StringBuilder();

            sb.Append(Margin.Tab());
            sb.Append(left);
            sb.Append(" " + AssignChar + " ");
            
            // assigning an external class?
            if (right.GetType() == typeof (ExternalCallNode))
            {
                // Trim ";"
                var rightText = right.ToString().Trim().Replace(";", "");

                // Append "new" if instantiation is needed
                // TODO Determine when "new" is needed
                //    sb.Append("new ");
                
                sb.Append(rightText);
            }
            else
            {
                // Add right-side and end line
                sb.Append(right.ToString().Trim());
            }

            sb.Append(";");
            return sb.ToString();
        }
    }
}