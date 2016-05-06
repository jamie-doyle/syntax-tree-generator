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

        public bool IsDeclaration { get; set; }

        /// <summary>
        /// Constructor for VarNode
        /// </summary>
        /// <param name="type">String corresponding to variable's C# type</param>
        /// <param name="varName">Name of the variable. Should be a valid C# identifier</param>
        /// <param name="isDeclaration">Is this variable a declaration? Optional - defaults to false</param>
        public VarNode(string type, string varName, bool isDeclaration = false) : base(0)
        {
            NodeType = type;
            Info = varName;
        }

        public VarNode()
        {
            
        }

        /// <summary>
        /// Returns string representation of the var
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return !IsDeclaration ? base.ToString() : $"{NodeType} {Info}";
        }
    }
}
