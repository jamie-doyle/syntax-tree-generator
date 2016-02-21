using System;
using System.Linq;
using System.Text;

namespace SyntaxTreeGen.Models
{
    public class MethodNode : Node
    {
        public ProtectionLevelKind ProtectionLevel { get; set; }
        public bool IsStatic { get; set; }
        
        public Type MethodType { get; }
        
        public enum ProtectionLevelKind
        {
            Public,
            Private
        }
        
        /// <summary>
        /// Creates a new method signature with parameters
        /// </summary>
        /// <param name="methodName">Name of this method</param>
        /// <param name="protection">Protection level</param>
        /// <param name="methodType">C# return type of this method</param>
        /// <param name="parameters">Parameters of this method</param>
        public MethodNode(string methodName, bool isStatic, ProtectionLevelKind protection, Type methodType, params Node[] parameters) : base(2)
        {
            ProtectionLevel = protection;
            IsStatic = isStatic;
            MethodType = methodType;
            Info = methodName;
            
            SetUpNodes();

            foreach (var parameter in parameters)
            {
                Subnodes.First().AddSubnode(parameter);
            }
        }

        /// <summary>
        /// Adds a NodeListNode for parameters as Subnode[0], and for statements at Subnode[1]
        /// </summary>
        private void SetUpNodes()
        {
            AddSubnode(new NodeListNode());
            AddSubnode(new NodeListNode());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // Build method signature
            sb.Append(ProtectionLevel.ToString().ToLower() + " ");

            if (IsStatic)
                sb.Append("static" + " ");

            // Handle 'void' cases - fully qualified type for void is invalid
            if (MethodType == typeof (void))
                sb.Append("void" + " ");
            else
                sb.Append(MethodType + " ");

            sb.Append(Info + " ");

            // parameters
            sb.Append("(");

            if (Subnodes.First().Subnodes.Count > 0)
            {
                VarNode param;

                for (var i = 0; i < Subnodes[0].Subnodes.Count - 1; i++)
                {
                    param = Subnodes.First().Subnodes[i] as VarNode;

                    sb.Append(param?.NodeType + " " + param?.Info +  ", ");
                }

                // Append the last parameter without a trailing ','
                param = Subnodes.First().Subnodes.Last() as VarNode;

                sb.Append(param?.NodeType + " " + param?.Info);
            }

            sb.Append(")");
            sb.AppendLine(); // END method sig

            // Open method body
            sb.Append("{");
            sb.AppendLine();

            // call ToString on each method statement
            foreach (var n in Subnodes.Last().Subnodes)
            {
                sb.Append(n);
                sb.AppendLine();
            }

            // Close method body
            sb.Append("}");

            return sb.ToString();
        }
    }
}
