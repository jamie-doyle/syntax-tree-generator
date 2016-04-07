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
        public ProtectionLevelKind ProtectionLevel { get; set; }

        /// <summary>
        /// Parameterless contructor
        /// </summary>
        public ClassNode() : base(2)
        {

        }

        /// <summary>
        /// Creates a new method signature with parameters
        /// </summary>
        /// <param name="methodName">Name of this method</param>
        /// <param name="isStatic">Dermines if the method is static</param>
        /// <param name="protection">Protection level</param>
        /// <param name="parameters">Parameters of this method</param>
        public ClassNode(string methodName, bool isStatic, ProtectionLevelKind protection, params Node[] parameters) : base(int.MaxValue)
        {
            ProtectionLevel = protection;
            IsStatic = isStatic;

            Info = methodName;
            
            foreach (var parameter in parameters)
            {
                AddSubnode(parameter);
            }
        }

        /// <summary>
        /// Is the method static?
        /// </summary>
        [XmlElement("isStatic")]
        public bool IsStatic { get; set; }

        /// <summary>
        /// Get/set the class name
        /// </summary>
        [XmlElement("name")]
        public string Name
        {
            get { return Info; }
            set { Info = value; }
        }

        [XmlElement("methods", typeof(MethodNode))]
        public Node[] Methods
        {
            get
            {
                return Subnodes.ToArray();
            }
            set
            {
                if (Subnodes.Count>0)
                    Subnodes.Clear();
                
                foreach (var n in value ?? new Node[0])
                    Subnodes.Add(n);
            }
        }
        
        public enum ProtectionLevelKind
        {
            Public,
            Private
        }
        
        public ClassNode(ProtectionLevelKind protectionLevel, bool isStatic, string className, params MethodNode[] methods) 
            : base(int.MaxValue)
        {
            ProtectionLevel = protectionLevel;
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
            sb.Append(ProtectionLevel.ToString().ToLower() + " " );

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

            var formatted = FormatCSharp(sb.ToString());

            return sb.ToString();
        }
    }
    
}
