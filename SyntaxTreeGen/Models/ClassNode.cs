using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents classes
    /// </summary>
    public class ClassNode : Node
    {
        public bool IsStatic { get; private set; }
        public ProtectionLevelKind ProtectionLevel { get; private set; }

        public enum ProtectionLevelKind
        {
            Public,
            Private
        }
        
        public ClassNode(ProtectionLevelKind protectionLevel, bool isStatic, string className, params MethodNode[] methods) 
            : base(int.MaxValue)
        {
            ProtectionLevel = protectionLevel;
            IsStatic = isStatic;
            Info = className;

            foreach (var method in methods)
            {
                AddSubnode(method);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ProtectionLevel.ToString().ToLower() + " " );

            if (IsStatic)
                sb.Append("static ");

            sb.Append("class " + Info);
            sb.AppendLine();

            sb.Append("{");

            // Add methods
            foreach (var n in Subnodes)
                sb.Append(n.ToString());

            sb.AppendLine();
            sb.Append("}");

            return sb.ToString();
        }
    }
    
}
