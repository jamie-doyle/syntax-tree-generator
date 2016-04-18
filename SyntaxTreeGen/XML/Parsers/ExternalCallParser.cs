using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SyntaxTreeGen.Models;

namespace SyntaxTreeGen.XML.Parsers
{
    internal class ExternalCallParser : Parser
    {
        internal ExternalCallParser(XmlReader xmlreader) : base(xmlreader, "externalcall")
        {
            var isStatic = true;
            var paramList = new List<Node>();
            var qualList = new List<string>();
            
            while (!(Reader.NodeType == XmlNodeType.EndElement && Reader.Name.ToLower() == ("externalcall")))
            {
                var attribute = Reader.Name.ToLower();

                switch (attribute)
                {
                    case "isstatic":
                        Reader.Read();
                        isStatic = bool.Parse(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    
                    case "qualifier":
                        Reader.Read();
                        qualList.Add(Reader.Value);
                        Reader.Read();
                        ReadEndTag(attribute);
                        break;
                    
                    case "parameters":
                        // Move to var or const node
                        Reader.Read();

                        while (Reader.Name.ToLower() == "variable" || Reader.Name.ToLower() == "constant")
                        {
                            try
                            {
                                Parser parser;
                                
                                if (Reader.Name.ToLower() == "variable")
                                    parser = new VarParser(Reader);
                                else
                                    parser = new ConstantParser(Reader);

                                paramList.Add(parser.Result);     
                            }
                            catch (ArgumentException)
                            {
                                throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                            }
                        }
                        
                        break;

                    default:
                        throw Exception.Generate(Reader, Exception.ErrorType.UnknownSubnode);
                }
            }
            
            Result = new ExternalClassNode(isStatic, qualList, paramList);
            ReadEndTag("externalcall");
        }
    }
}
