using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a call to an external class (for instantiation of variables) or a call to a method
    /// </summary>
    class ExternalCallNode : Node
    {
        public List<string> Qualifiers { get; }

        public List<Node> Parameters { get; } 
        
        /// <summary>
        /// Represents a reference to an external class.
        /// </summary>
        /// <param name="qualifiers">
        ///     Strings detailing, in order, the fully qualified name of the class definition or static method. 
        ///     Each string represents a namespace or class.
        /// </param>
        /// <param name="parameters">
        ///     Parameters (if any) to pass to a static method call. Leave null if no params required.
        /// </param>
        public ExternalCallNode(IEnumerable<string> qualifiers, IEnumerable<Node> parameters = null) : base(0)
        {
            Qualifiers = new List<string>();
            Parameters = new List<Node>();

            foreach (var s in qualifiers)
                Qualifiers.Add(s);
            
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var pType = p.GetType();
                    if (pType != typeof(VarNode) && pType != typeof(ConstantNode) && pType != typeof(ExternalCallNode))
                        throw new ArgumentException("Parameters must be a variable, constant, or call");

                    Parameters.Add(p);
                }
            }
        }
        
        /// <summary>
        /// Build code for an ExternalCall
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Margin.Tab());

            // Add all qualifiers before the last
            for (var i = 0; i < Qualifiers.Count - 1; i++)
            {
                sb.Append(Qualifiers[i].Trim() + ".");
            }

            // Add last
            sb.Append(Qualifiers.Last().Trim());
            
            // add params
            sb.Append("(");

            if (Parameters.Any())
            {
                // Add parameters
                for (var i = 0; i < Parameters.Count - 1; i++)
                    sb.Append(Parameters[i].ToString().Trim() + ",");

                sb.Append(Parameters.Last().ToString().Trim());
            }

            sb.Append(")");
            sb.Append(";");

            return sb.ToString();
        }
    }
}