using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MarkdownSharp;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// A base class for representing binary tree nodes
    /// </summary>
    public abstract class Node
    {
        private readonly int _size;
        private readonly List<Node> _subnodes;

        /// <summary>
        /// A textual representation of the function of this node
        /// </summary>
        public string Info { get; set; }
        
        /// <summary>
        /// Adds a node to the next available branch of this node.
        /// </summary>
        /// <param name="n">Node to add</param>
        public void AddSubnode(Node n)
        {
           if (_subnodes.Count == _size)
                throw new InvalidOperationException("This node cannot have any more subnodes.");

           _subnodes.Add(n);
        }

        /// <summary>
        /// The subnodes of this node
        /// </summary>
        public List<Node> Subnodes => _subnodes;

        /// <summary>
        /// Basic constructor, describing the number of children allowed.
        /// </summary>
        /// <param name="maxChild">
        /// Maximum number of children allowed on this node. 
        /// May be omitted for nodes with no children.
        /// </param>
        protected Node(int maxChild = 0)
        {
            _size = maxChild;
            _subnodes = new List<Node>();
        }
        
        /// <summary>
        /// Base constructor for N parameters
        /// </summary>
        /// <param name="node">One or more nodes</param>
        /// <param name="size">Maximum number of subnodes allowed</param>
        protected Node(int size, params Node[] node)
        {
            _size = size;
            _subnodes = new List<Node>();
            
            foreach (var n in node)
            {
                AddSubnode(n);
            }
        }
        
        /// <summary>
        /// Produces a string representation of this node and its children. This generic ToString
        /// should be overriden by more specific representaions in inherited classes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // If a node has no subnodes, just return its value
            if (_subnodes.Count < 1)
                return Info;

            // Otherwise, build a string containing its nodes. 
            var sb = new StringBuilder();
            
            foreach (var n in _subnodes)
                sb.Append(n + " ");
            
            return sb.ToString();
        }

        /// <summary>
        /// Formats a string with C# line breaks and indentation, using MarkdownSharp
        /// </summary>
        /// <returns>Formatted code</returns>
        public static string FormatCSharp(string rawCode)
        {
            var md = new Markdown();
            return md.Transform(rawCode);
        }
        
        /// <summary>
        /// Defines available protection levels
        /// </summary>
        public enum AccessLevel
        {
            Public,
            Private,
            Internal,
            Protected
        }

        /// <summary>
        /// Get the access level associated with a string
        /// </summary>
        /// <param name="text">Access level to fetch</param>
        /// <returns></returns>
        internal static Node.AccessLevel GetAccessLevel(string text)
        {
            AccessLevel res;

            if (Enum.TryParse(text, true, out res))
                return res;
            
            // If not found, throw error
            throw new InvalidDataException("\"" + text + "\" is not a valid access level");
        }
    }
}