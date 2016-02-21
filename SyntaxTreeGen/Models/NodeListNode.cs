using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    public class NodeListNode : Node
    {
        /// <summary>
        /// Creates an empty NodeList
        /// </summary>
        public NodeListNode() : base(int.MaxValue)
        {
            
        }

        /// <summary>
        /// Creates a NodeList with one or more nodes
        /// </summary>
        /// <param name="nodes"></param>
        public NodeListNode(params Node[] nodes) : base(int.MaxValue, nodes)
        {
            
        }
      
    }
}
