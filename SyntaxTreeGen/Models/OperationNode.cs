using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    internal class OperationNode : Node
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
            Xor,
            Not,
            NotEqual,
            And,
            Or,
            Equal
        }

        /// <summary>
        /// Gets the operator type for this operand
        /// </summary>
        public OpKind Op { get; internal set; }
        
        public OperationNode(Node left, OpKind op, Node right) : base(2, left, right)
        {
            Op = op;
            Info = GetOperator(op);
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        internal OperationNode()
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
                case OpKind.Xor:
                    return "^";
                case OpKind.Equal:
                    return "==";
                case OpKind.Not:
                    return "!";
                case OpKind.NotEqual:
                    return "!=";
                case OpKind.And:
                    return "&&";
                case OpKind.Or:
                    return "||";
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
        
        /// <summary>
        /// Get the operator type associated with a string
        /// </summary>
        /// <param name="text">C# style symbol representaion of an operator</param>
        /// <returns></returns>
        internal static OpKind GetOperator(string text)
        {
            switch (text)
            {
                case "<":
                    return OpKind.LessThan;
                case ">":
                    return OpKind.GreaterThan;
                case "<=":
                    return OpKind.GreaterThanOrEqual;
                case ">=":
                    return OpKind.LessThanOrEqual;
                case "+":
                    return OpKind.Add;
                case "-":
                    return OpKind.Subtract;
                case "*":
                    return OpKind.Multiply;
                case "/":
                    return OpKind.Divide;
                case "mod":
                    return OpKind.Modulo;
                case "^":
                    return OpKind.Xor;
                case "==":
                    return OpKind.Equal;
                case "!":
                    return OpKind.Not;
                case "!=":
                    return OpKind.NotEqual;
                case "&&":
                    return OpKind.And;
                case "||":
                    return OpKind.Or;
                default:
                    throw new ArgumentException($"\"{text}\" is not a valid operator");
            }
        }

        public override string ToString()
        {
            return "(" +  Subnodes[0] + " " + GetOperator(Op) + " " + Subnodes[1] + ")";
        }
        
    }
}