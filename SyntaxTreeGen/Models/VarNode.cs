using System;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a variable
    /// </summary>
    class VarNode : Node
    {
        public Type NodeType { get; }
        
        public VarNode(Type varType, string varName) : base(0)
        {
            NodeType = varType;
            Info = varName;
        }
    }
}
