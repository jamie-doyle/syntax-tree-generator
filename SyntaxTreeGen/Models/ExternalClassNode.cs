using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a call to an external class (for instantiation or typification of variables), 
    /// or a direct call to a static external method.
    /// </summary>
    class ExternalClassNode : Node
    {
        /// <summary>
        /// Is this a call to a static method?
        /// </summary>
        public readonly bool IsCallStatic;

        public List<string> Qualifiers { get; }

        public List<Node> Parameters { get; } 
        
        /// <summary>
        /// Represents a reference to an external class.
        /// </summary>
        /// <param name="isStatic">
        ///     Is this ExternalClass used to call a static method?
        /// </param>
        /// <param name="qualifiers">
        ///     Strings detailing, in order, the fully qualified name of the class definition or static method. 
        ///     Each string represents a namespace or class.
        /// </param>
        /// <param name="parameters">
        ///     Parameters (if any) to pass to a static method call. Leave null if no params required.
        /// </param>
        public ExternalClassNode(bool isStatic, IEnumerable<string> qualifiers, IEnumerable<Node> parameters = null) : base(0)
        {
            IsCallStatic = isStatic;
            Qualifiers = new List<string>();
            Parameters = new List<Node>();

            foreach (var s in qualifiers)
                Qualifiers.Add(s);

            if (parameters != null && !isStatic)
                throw new ArgumentException("Cannot pass parameters to a class");
            
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var pType = p.GetType();
                    if (pType != typeof(VarNode) && pType != typeof(ConstantNode))
                        throw new ArgumentException("Parameters must be a variable or constant");

                    Parameters.Add(p);
                }
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            // Add all qualifiers before the last
            for (var i = 0; i < Qualifiers.Count - 1; i++)
            {
                sb.Append(Qualifiers[i].Trim() + ".");
            }

            // Add last
            sb.Append(Qualifiers.Last().Trim());
            
            // If a static method call, add params
            if (IsCallStatic)
            {
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
            }
            
            return sb.ToString();
        }
    }
}