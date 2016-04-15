using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    class MethodParser : Parser
    {
        /// <summary>
        /// Parses XML data to a ClassNode
        /// </summary>
        /// <param name="xmlreader">Reader pointed to a method element</param>
        internal MethodParser(XmlReader xmlreader) : base (xmlreader, "class")
        {
            var tmp = new MethodNode();

            // While not at a "</method>" tag
            while (!((Reader.NodeType == XmlNodeType.EndElement) && (Reader.Name.Equals("method"))))
            {
                // skip comments
                while (Reader.NodeType == XmlNodeType.Comment)
                {
                    Reader.Read();
                }

                // Get the attribute type
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "accesslevel":
                        // advance to data and read it
                        Reader.Read();
                        tmp.MethodAccessLevel = Node.GetAccessLevel(Reader.Value);
                        ReadEndTag(attribute);
                        break;
                    case "isstatic":
                        Reader.Read();
                        tmp.IsStatic = bool.Parse(Reader.Value);
                        ReadEndTag(attribute);
                        break;
                    case "name":
                        Reader.Read();
                        tmp.Info = Reader.Value;
                        ReadEndTag(attribute);
                        break;
                    case "parameter":
                        Reader.Read();

                        // Parse each <variable> in the <parameters> list
                        var paramList = new List<Node>();
                        while (Reader.Name.ToLower() == "variable")
                        {
                            try
                            {
                                var parser = new VarParser(Reader);
                                paramList.Add(parser.Result);     
                            }
                            catch (ArgumentException)
                            {
                                throw Exception.Generate(base.Reader, Exception.ErrorType.UnknownSubnode);
                            }
                        }
                        
                        // Add parameters to Method
                        tmp.Parameters = paramList;
                        ReadEndTag(attribute);
                        break;

                    case "returntype":
                        Reader.Read();
                        tmp.ReturnType = Reader.Value;
                        ReadEndTag(attribute);
                        break;

                    case "statements":
                        Reader.Read();
                        if (tmp.Statements.Any())
                            throw Exception.Generate(Reader, Exception.ErrorType.Statements);
                        // todo ParseStatements(Reader).Result
                        //var res = ParseStatements(reader);
                        //tmp.Statements = res.Subnodes;
                        break;

                    default:
                        throw Exception.Generate(base.Reader, Exception.ErrorType.UnknownSubnode);
                }
            }

            ReadEndTag("method");
            Result = tmp;
        }
    }
}