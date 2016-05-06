using System.Collections.Generic;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class ExternalCallParser : Parser
    {
        internal ExternalCallParser(XmlReader xmlreader) : base(xmlreader, "externalcall")
        {
            var paramList = new List<Node>();
            var qualList = new List<string>();
            
            while (!(Reader.NodeType == XmlNodeType.EndElement && Reader.Name.ToLower() == ("externalcall")))
            {
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "qualifier":
                        Reader.Read();
                        qualList.Add(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    
                    case "parameters":
                        // Move in to params section
                        Reader.Read();
                        
                        // While not at </parameters>
                        while (!(Reader.NodeType == XmlNodeType.EndElement && Reader.Name.ToLower() == ("parameters")))
                        {
                            Parser parser;

                            // Add the variable/const/call to the param list
                            switch (Reader.Name.ToLower())
                            {
                                case "variable":
                                    parser = new VarParser(Reader);
                                    break;
                                case "constant":
                                    parser = new ConstantParser(Reader);
                                    break;
                                case "externalcall":
                                    parser = new ExternalCallParser(Reader);
                                    break;
                                default:
                                    throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                            }
                                
                            paramList.Add(parser.Result);
                        }
                        
                        // Close parameters section
                        //Reader.Read();
                        ReadEndTag("parameters");

                        break;

                    default:
                        throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                }
            }
            
            Result = new ExternalCallNode(qualList, paramList);
            ReadEndTag("externalcall");
        }
    }
}
