using System;

namespace SyntaxTreeGen.Models
{
    internal class OperatorNode : Node
    {
        /// <summary>
        /// Defines the operators possible.
        /// </summary>
        public enum OpKind
        {
            LessThan,
            GreaterThan,
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            ToPowerOf,
            Equals
        }

        /// <summary>
        /// Gets the operator type for this operand
        /// </summary>
        public OpKind Op { get; }

        public OperatorNode(OpKind op, Node left, Node right) : base(left, right)
        {
            Op = op;
            Info = GetOperator(op);
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
                case OpKind.Add:
                    return "+";
                case OpKind.Subtract:
                    return "+";
                case OpKind.Multiply:
                    return "*";
                case OpKind.Divide:
                    return "/";
                case OpKind.Modulo:
                    return "mod";
                case OpKind.ToPowerOf:
                    return "^";
                case OpKind.Equals:
                    return "=";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, "The operator used does not exist.");
            }
        }

        public override string ToString()
        {
            // TODO: possible override needed here?
            return base.ToString();
        }
    }
}