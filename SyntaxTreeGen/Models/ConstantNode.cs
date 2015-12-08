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
        
        public ConstantNode(T constVal) : base(null, null)
        {
            _constVal = constVal;
            Info = constVal.ToString();
            NodeType = typeof(T);
        }

    }
}
