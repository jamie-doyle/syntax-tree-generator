using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Represents classes
    /// </summary>
    [XmlRoot("class", Namespace = "qub.ac.uk/jdoyle7/SyntaxTree/Class")]
    public class ClassNode : Node
    {
        [XmlAttribute("protectionLevel", Namespace = "class.protectionLevel")]
        public AccessLevel ClassAccessLevel { get; set; }

        /// <summary>
        /// Parameterless contructor
        /// </summary>
        public ClassNode() : base(2)
        {

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
        
        public ClassNode(AccessLevel classAccessLevel, bool isStatic, string className, params MethodNode[] methods) 
            : base(int.MaxValue)
        {
            ClassAccessLevel = classAccessLevel;
            IsStatic = isStatic;
            Info = className;

            foreach (var method in methods)
            {
                AddSubnode(method);
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ClassAccessLevel.ToString().ToLower() + " " );

            if (IsStatic)
                sb.Append("static ");

            sb.Append("class " + Info);
            sb.AppendLine();

            sb.Append("{");
            sb.AppendLine();

            // Add methods
            foreach (var n in Subnodes)
                sb.Append(n);

            sb.AppendLine();
            sb.Append("}");
            
            return sb.ToString();
        }
    }   
}