using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a method declaration
    /// </summary>
    public class MethodNode : Node
    {
        /// <summary>
        /// The access level of this method
        /// </summary>
        public AccessLevel MethodAccessLevel { get; set; }
        
        /// <summary>
        /// Is this method static?
        /// </summary>
        public bool IsStatic { get; set; }
        
        /// <summary>
        /// String representation of the method's return type
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MethodNode() : base(2)
        {
            SetUpNodes();
        }
        
        /// <summary>
        /// Creates a new method signature with parameters
        /// </summary>
        /// <param name="methodName">Name of this method</param>
        /// <param name="isStatic">Dermines if the method is static</param>
        /// <param name="methodAccess">Protection level</param>
        /// <param name="returnType">String indicating the return type of this method</param>
        /// <param name="parameters">Parameters of this method</param>
        public MethodNode(string methodName, bool isStatic, AccessLevel methodAccess, string returnType, params Node[] parameters) : base(2)
        {
            MethodAccessLevel = methodAccess;
            IsStatic = isStatic;
            ReturnType = returnType;
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
        /// <param name="methodAccess">Protection level</param>
        /// <param name="returnType">C# return type of this method</param>
        /// <param name="parameters">Parameters of this method</param>
        /// <param name="statements">Statements in the method</param>
        public MethodNode(string methodName, bool isStatic, AccessLevel methodAccess, string returnType, 
            IEnumerable<VarNode> parameters, IEnumerable<Node> statements) : base(2)
        {
            MethodAccessLevel = methodAccess;
            IsStatic = isStatic;
            ReturnType = returnType;
            Info = methodName;
            
            SetUpNodes();
            
            foreach (var parameter in parameters)
            {
                // Check "IsDeclaration" flag is set
                if (!parameter.IsDeclaration)
                    throw new ArgumentException("All parameters in a method signature should be marked as declarations ");

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
        public IEnumerable<Node> Parameters
        {
            get {
                // return empty list if no params set
                return Subnodes.First().Subnodes.Any() ? 
                    (IEnumerable<Node>) Subnodes.First().Subnodes : new Node[] {};
            }
            set { Subnodes[0] = new NodeListNode((Node[])value); }
        }

        /// <summary>
        /// Access the statements in this method
        /// </summary>
        public IEnumerable<Node> Statements
        {
            get
            {
                // Return an empty list if no statements set
                return Subnodes.Last().Subnodes.Any() ?
                 (IEnumerable<Node>)Subnodes.Last().Subnodes : new Node[] { };
            }
            set { Subnodes[1] = new NodeListNode((Node[])value); }
        }
        
        /// <summary>
        /// Return a string representation of the method
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // Build method signature
            sb.Append(Margin.Tab());
            sb.Append(MethodAccessLevel.ToString().ToLower() + " ");

            if (IsStatic)
                sb.Append("static" + " ");

            // Add return type, if appropriate (constructors, etc. don't have one)
            if (ReturnType != null)
            {
                // Stop full qualification of "void" (this doesn't work in C#)
                if (ReturnType.Contains("System.Void"))
                    sb.Append("void ");
                else
                    sb.Append(ReturnType + " ");
            }

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
            sb.Append(Margin.Tab() + "{");
            sb.AppendLine();
            
            // call ToString on each method statement
            Margin.Indent();
            foreach (var n in Subnodes.Last().Subnodes)
            {
                sb.Append(n);
                sb.AppendLine();
            }
            Margin.Outdent();

            // Close method body
            sb.Append(Margin.Tab() + "}");

            return sb.ToString();
        }
    }
}