using System;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxTreeGen.XML
{
    /// <summary>
    /// Builds exceptions from a set of predefined messages
    /// </summary>
    internal static class Exception
    {
        /// <summary>
        /// Builds detailed exceptions about parsing failures. Do not use for errors not directly caused by bad XML
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="error"></param>
        internal static XmlException Generate(XmlReader reader, ErrorType error)
        {
            var info = ((IXmlLineInfo) reader);
            
            var lineNum = info.LineNumber;
            var linePos = info.LinePosition;

            string msg;

            // Error messages should fit the format:
            // "XML could not be parsed because {MESSAGE}."
            switch (error)
            {
                case ErrorType.Statements:
                    msg = "the method contains more than one set of statements";
                    break;
                case ErrorType.UnclosedTag:
                    msg = "the tag <" + reader.Name + "> was not closed";
                    break;
                case ErrorType.NoValue:
                    msg = "a constant node must have a value";
                    break;
                case ErrorType.BadAssignLeft:
                    msg = "assignment must be to a variable";
                    break;
                case ErrorType.BadAssignRight:
                    msg = "assignment must be of a constant or variable";
                    break;
                case ErrorType.Conditions:
                    msg = "the if contains more than one condition declaration";
                    break;
                case ErrorType.TooManyOperands:
                    msg = "the operation contains more than two operands";
                    break;
                case ErrorType.NoOperator:
                    msg = "the expression does not contain an operator";
                    break;
                case ErrorType.UnknownSubnode:
                    msg = "an unexpected sunode was found";
                    break;
                case ErrorType.InvalidIf:
                    msg = "an \"if\" must contain a condition";
                    break;
                default:
                    msg = "an unknown error occurred";
                    break;
            }

            return new XmlException(msg, null, lineNum, linePos);
        }

        internal enum ErrorType
        {
            NoClass,

            UnknownSubnode,

            Statements,  // Multiple <statements> blocks
            UnclosedTag, // Any unclosed tag
            BadAssignLeft, 
            BadAssignRight,
            NoValue,
            Conditions,

            TooManyOperands,
            NoOperator,

            InvalidIf
        }
    }
}
