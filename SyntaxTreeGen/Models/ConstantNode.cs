using System;

namespace SyntaxTreeGen.Models
{
    class ConstantNode<T> : Node
    {
        /// <summary>
        /// The type of value represented by this node
        /// </summary>
        public Type NodeType { get; }

        private readonly T _constVal;
        
        /// <summary>
        /// Creates a node with no children
        /// </summary>
        /// <param name="constVal"></param>
        public ConstantNode(T constVal) : base(0)
        {
            _constVal = constVal;
            Info = constVal.ToString();
            NodeType = typeof(T);
        }
        
    }
}
