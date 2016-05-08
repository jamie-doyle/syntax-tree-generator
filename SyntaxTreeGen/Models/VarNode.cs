using System;
using System.ComponentModel;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a variable
    /// </summary>
    public class VarNode : Node
    {
        /// <summary>
        /// String representation of the variable type
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Does this node declare the variable?
        /// </summary>
        public bool IsDeclaration { get; set; }

        /// <summary>
        /// Constructs a VarNode with all fields
        /// </summary>
        /// <param name="type">String corresponding to variable's C# type</param>
        /// <param name="varName">Name of the variable. Should be a valid C# identifier</param>
        /// <param name="isDeclaration">Is this variable a declaration? Optional - defaults to false</param>
        public VarNode(string type, string varName, bool isDeclaration = false) : base(0)
        {
            NodeType = type;
            Info = varName;
        }

        /// <summary>
        /// Default VarNode constructor
        /// </summary>
        public VarNode()
        {
        }

        /// <summary>
        /// Returns string representation of the var
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // If this variable is a declaration, append a tab, type and end the line with ";"
            return !IsDeclaration ? base.ToString() : $"{Margin.Tab()}{NodeType} {Info};";
        }
    }
}
