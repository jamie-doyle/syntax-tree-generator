using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    [Serializable]
    [XmlRoot("operator")]
    internal class OperatorNode : Node
    {
        /// <summary>
        /// Defines the collection of usable operators for this node
        /// </summary>
        public enum OpKind
        {
            LessThan,
            GreaterThan,
            LessThanOrEqual,
            GreaterThanOrEqual,
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            ToPowerOf,
            Equals,
            Not,
            And,
            Or
        }

        /// <summary>
        /// Gets the operator type for this operand
        /// </summary>
        public OpKind Op { get; internal set; }
        
        public OperatorNode(Node left, OpKind op, Node right) : base(2, left, right)
        {
            Op = op;
            Info = GetOperator(op);
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        internal OperatorNode()
        {
        }

        internal Node Left
        {
            get { return Subnodes[0]; }
            set { Subnodes[0] = value; }
        }

        internal Node Right
        {
            get { return Subnodes[1]; }
            set { Subnodes[1] = value; }
        }

        /// <summary>
        /// Allows setting of operator kind (an OpKind enum) using a string
        /// </summary>
        [XmlElement("kind")]
        public string StringOp
        {
            get { return Op.ToString(); }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                var parsedOp = (OpKind) Enum.Parse(typeof (OpKind), value);

                if (Enum.IsDefined(typeof (OpKind), parsedOp))
                    Op = parsedOp;
                else
                    throw new ArgumentException(
                        "\"" + value + "\" is not a valid operator. ");
            }   
        }

        /// <summary>
        /// Determine the string representation of an operator.
        /// </summary>
        /// <param name="op">Operator to determine</param>
        /// <returns></returns>
        private string GetOperator(OpKind op) 
        {
            switch (op)
            {
                case OpKind.LessThan:
                    return "<";
                case OpKind.GreaterThan:
                    return ">";
                case OpKind.GreaterThanOrEqual:
                    return "<=";
                case OpKind.LessThanOrEqual:
                    return ">=";
                case OpKind.Add:
                    return "+";
                case OpKind.Subtract:
                    return "-";
                case OpKind.Multiply:
                    return "*";
                case OpKind.Divide:
                    return "/";
                case OpKind.Modulo:
                    return "mod";
                case OpKind.ToPowerOf:
                    return "^";
                case OpKind.Equals:
                    return "==";
                case OpKind.Not:
                    return "!=";
                case OpKind.And:
                    return "&&";
                case OpKind.Or:
                    return "||";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, "The operator used does not exist.");
            }
        }

        /// <summary>
        /// Get the operator type associated with a string
        /// </summary>
        /// <param name="text">Access level to fetch</param>
        /// <returns></returns>
        internal static OpKind GetOperator(string text)
        {
            OpKind res;

            if (Enum.TryParse(text, true, out res))
                return res;

            // If not found, throw error
            throw new InvalidDataException("\"" + text + "\" is not a valid access level");
        }

        public override string ToString()
        {
            return "(" +  Subnodes[0] + " " + GetOperator(Op) + " " + Subnodes[1] + ")";
        }
        
    }
}