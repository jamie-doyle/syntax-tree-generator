using System;
using System.Text;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A typed, directly-referenced constant (e.g., "a string", 'c', 10 ). 
    /// For immutable variables (e.g., const int months = 12), use VarNode 
    /// </summary>
    class ConstantNode : Node
    {
        public object ConstVal { get; internal set; }
        private readonly Type _varType;

        
        /// <summary>
        /// Creates a node with no children
        /// </summary>
        /// <param name="constVal"></param>
        public ConstantNode(object constVal) : base(0)
        {
            ConstVal = constVal;
            Info = constVal.ToString();

            _varType = constVal.GetType();
        }
        
        /// <summary>
        /// Construct a constant node with no parameters
        /// </summary>
        public ConstantNode() : base(0)
        {
            
        }

        public override string ToString()
        {
            // Format string
            if (_varType == typeof (string))
                return $"\"{Info}\"";

            // format character
            if (_varType == typeof (char))
                return $"'{Info}'";

            // if not a string or char, return 
            return ConstVal.ToString();
        }
    }
}
