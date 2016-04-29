using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents classes
    /// </summary>
    public class ClassNode : Node
    {
        public AccessLevel ClassAccessLevel { get; set; }

        /// <summary>
        /// Parameterless contructor
        /// </summary>
        public ClassNode() : base(2)
        {
            SetupSubnodes();
        }
        
        /// <summary>
        /// Is the method static?
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Get/set the class name
        /// </summary>
        public string Name
        {
            get { return Info; }
            set { Info = value; }
        }
        
        /// <summary>
        /// Construct a class with all attributes, and with methods and fields
        /// </summary>
        /// <param name="classAccessLevel"></param>
        /// <param name="isStatic"></param>
        /// <param name="className"></param>
        /// <param name="methods"></param>
        /// <param name="fields">Array of variables declared in this class</param>
        public ClassNode(AccessLevel classAccessLevel, bool isStatic, string className, IEnumerable<MethodNode> methods, IEnumerable<VarNode> fields)
            : base(2)
        {
            ClassAccessLevel = classAccessLevel;
            IsStatic = isStatic;
            Info = className;
            
            SetupSubnodes();

            foreach (var field in fields)
                AddField(field);

            foreach (var method in methods)
                AddMethod(method);
        }

        private void SetupSubnodes()
        {
            AddSubnode(new NodeListNode());
            AddSubnode(new NodeListNode());
        }

        public VarNode[] Fields
        {
            get
            {
                // Get fields from subnode
                var fieldArray = Subnodes.First().Subnodes.ToArray();
                var len = fieldArray.Length;
                var res = new VarNode[len];
                
                // Cast each field as a VarNode
                for (var i=0; i<len; i++)
                {
                    res[i] = (VarNode)fieldArray[i];
                }

                return res;
            }
            set
            {
                Subnodes[0].Subnodes.Clear();
                foreach (var v in value )
                    Subnodes[0].AddSubnode(v);
            }
        }

        public MethodNode[] Methods
        {
            get
            {
                // Get method (in last subnode)
                var fieldArray = Subnodes.Last().Subnodes.ToArray();
                var len = fieldArray.Length;
                var res = new MethodNode[len];

                // Cast each field as a VarNode
                for (var i = 0; i < len; i++)
                {
                    res[i] = (MethodNode)fieldArray[i];
                }

                return res;
            }
            set
            {
                Subnodes[1].Subnodes.Clear();
                foreach (var v in value)
                    Subnodes[1].AddSubnode(v);
            }
        }

        /// <summary>
        /// Add a new method
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(MethodNode method)
        {
            Subnodes.Last().AddSubnode(method);
        }

        /// <summary>
        /// Add a new field
        /// </summary>
        /// <param name="field"></param>
        public void AddField(VarNode field)
        {
            Subnodes.First().AddSubnode(field);
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Margin.Tab());
            sb.Append(ClassAccessLevel.ToString().ToLower() + " " );

            if (IsStatic)
                sb.Append("static ");

            sb.Append("class " + Info);
            sb.AppendLine();
            sb.Append(Margin.Tab() + "{");
            sb.AppendLine();

            // Indent to build class body
            Margin.Indent();

            // Add fields
            foreach (var field in Fields)
            {
                sb.Append(Margin.Tab());
                sb.Append($"{field.NodeType} {field.Info};");
                sb.AppendLine();
            }

            // add Methods
            foreach (var method in Methods)
            {
                sb.Append(method);
            }

            Margin.Outdent();

            sb.AppendLine();
            sb.Append(Margin.Tab() + "}");
            
            return sb.ToString();
        }
    }   
}