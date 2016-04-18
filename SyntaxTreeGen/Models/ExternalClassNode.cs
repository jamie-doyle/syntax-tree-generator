using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents a call to an external class. An example would be "Console.WriteLine()"
    /// </summary>
    class ExternalClassNode : Node
    {
        /// <summary>
        /// Is the call made 
        /// </summary>
        public readonly bool IsCallStatic;

        public ExternalClassNode(bool isCallStatic, string className, NodeListNode classes, Node method) : base(2)
        {
            IsCallStatic = isCallStatic;

            Subnodes[0] = classes;
        }

        public ExternalClassNode(bool isCallStatic, string className, NodeListNode classes) : base(2)
        {
            IsCallStatic = isCallStatic;

            Subnodes[0] = classes;
            Subnodes[1] = null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // System.Console.WriteLine()
            // CLASS 
            //        CLASS 
            //          
            
            //var system = new ExternalClassNode(true, "System", new NodeListNode(new));
                  
            return sb.ToString();
        }
    }
}