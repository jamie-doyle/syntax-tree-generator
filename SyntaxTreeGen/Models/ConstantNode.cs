using System;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A typed, directly-referenced constant (e.g., "a string", 'c', 10, SomeObject). 
    /// For immutable variables (e.g., const int months = 12), use VarNode 
    /// </summary>
    class ConstantNode : Node
    {
        public object ConstVal { get; internal set; }
        
        /// <summary>
        /// Creates a node with no children
        /// </summary>
        /// <param name="constVal"></param>
        public ConstantNode(object constVal) : base(0)
        {
            ConstVal = constVal;
            Info = constVal.ToString();
        }

        public ConstantNode() : base(0)
        {
            
        }
        
    }
}
