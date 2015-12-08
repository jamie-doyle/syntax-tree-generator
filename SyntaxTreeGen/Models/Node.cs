using System;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A base class for representing binary tree nodes
    /// </summary>
    public abstract class Node
    {
        private static int _depth;

        private Node _left;
        private Node _right;

        // Additional information about this node
        public string Info { get; set; }
        
        protected Node(Node left, Node right)
        {
            _left = left;
            _right = right;
        }
        
        public Node Left
        {
            get { return _left; }
            set
            {
                CanHaveChildNode();

                _left = value;
            }
        }

        public Node Right
        {
            get { return _right; }
            set
            {
                CanHaveChildNode();

                _right = value;
            }
        }

        /// <summary>
        /// Produces a string representation of this node and its children.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // NB: ToString is called implicitly on Left/Right nodes
            if (Left != null && Right != null)
                return Left + " " + Info + " " + Right;

            if (Left != null)
                return Left + " " + Info;
            
            if (Right != null)
                return Info +" " +  Right;

            return Info;
        }

        /// <summary>
        /// Checks this node may have child nodes. Attempting to add a child 
        /// to nodes representing variables, constants, etc. will produce an error.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to add a child node to a terminal node.
        /// </exception>
        private void CanHaveChildNode()
        {
            var gt = GetType();

            if (gt != typeof(ConstantNode<>) && gt != typeof(VarNode))
                return;
            
            throw new InvalidOperationException(
                "Constants and variables cannot have child nodes.");
        }
    }
}
