using System;
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
        public OpKind Op { get; private set; }
        
        public OperatorNode(Node left, OpKind op, Node right) : base(2, left, right)
        {
            Op = op;
            Info = GetOperator(op);
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

        public override string ToString()
        {
            return "(" +  Subnodes[0] + " " + GetOperator(Op) + " " + Subnodes[1] + ")";
        }
        
    }
}