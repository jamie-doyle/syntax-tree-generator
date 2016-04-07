using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    [XmlRoot("method", Namespace = "qub.ac.uk/jdoyle7/SyntaxTree/Method")]
    public class MethodNode : Node
    {
        [XmlAttribute("protectionLevel")]
        public ProtectionLevelKind ProtectionLevel { get; set; }

        [XmlAttribute("isStatic")]
        public bool IsStatic { get; set; }
       
        [XmlAttribute("type")]
        public Type MethodType { get; set; }
        
        public enum ProtectionLevelKind
        {
            Public,
            Private
        }

        public MethodNode() : base(2)
        {
            SetUpNodes();
        }

        /// <summary>
        /// Creates a new method signature with parameters
        /// </summary>
        /// <param name="methodName">Name of this method</param>
        /// <param name="isStatic">Dermines if the method is static</param>
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
        /// Creates a new method signature with parameters and statements
        /// </summary>
        /// <param name="methodName">Name of this method</param>
        /// <param name="isStatic">Dermines if the method is static</param>
        /// <param name="protection">Protection level</param>
        /// <param name="methodType">C# return type of this method</param>
        /// <param name="parameters">Parameters of this method</param>
        /// <param name="statements">Statements in the method</param>
        public MethodNode(string methodName, bool isStatic, ProtectionLevelKind protection, Type methodType, 
            IEnumerable<Node> parameters, IEnumerable<Node> statements) : base(2)
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

            foreach (var statement in statements)
            {
                Subnodes.Last().AddSubnode(statement);
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
        /// Access the parameters of this method
        /// </summary>
        /// <returns>Parameters</returns>
        public IEnumerable<Node> Parameters()
        {
            return Subnodes.First().Subnodes;
        }

        /// <summary>
        /// Access the statements in this method
        /// </summary>
        /// <returns>Statements</returns>
        public IEnumerable<Node> Statements()
        {
            return Subnodes.Last().Subnodes;
        } 
        
        /// <summary>
        /// Return a string representation of the method
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            // TODO: a LOT going on here - refactor?
            var sb = new StringBuilder();

            // Build method signature
            sb.Append(ProtectionLevel.ToString().ToLower() + " ");

            if (IsStatic)
                sb.Append("static" + " ");

            // Tidy up - remove "System.*" from classes, built-in types don't need full qualification
            var methodString = MethodType.ToString();

            if (methodString.Contains("System.Void"))
                methodString = "void";

            sb.Append(methodString + " ");
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
