using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represent hard-coded, constant integer nodes.
    /// </summary>
    [Obsolete ("Use ConstantNode<int> instead")]
    class IntConNode : Node
    {
        public IntConNode(int value)
        {
            Info = value.ToString();
        }
    }
}
