using System;
using System.ComponentModel;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a variable
    /// </summary>
    public class VarNode : Node
    {
        //public T NodeValue { get; set; }

        public bool HasBeenDeclared { get; private set; }
        public Type NodeType { get; private set; }

        /// <summary>
        /// Constructor for VarNode
        /// </summary>
        /// <param name="varName"></param>
        public VarNode(Type type, string varName) : base(0)
        {
            NodeType = type;
            Info = varName;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Instantiate()
        {
            
        }

    }
}
