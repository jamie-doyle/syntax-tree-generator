using System;
using System.ComponentModel;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a variable
    /// </summary>
    public class VarNode : Node
    {
        public string NodeType { get; set; }

        /// <summary>
        /// Constructor for VarNode
        /// </summary>
        /// <param name="type"></param>
        /// <param name="varName"></param>
        public VarNode(string type, string varName) : base(0)
        {
            NodeType = type;
            Info = varName;
        }

        public VarNode()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Instantiate()
        {
            
        }

    }
}
