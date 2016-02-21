using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// An XML document
    /// </summary>
    class ImportedDocument
    {
        private string _directory;
        private XmlDocument _doc;
        public Node Result;

        public ImportedDocument (string dir)
        {
            _directory = Path.GetFullPath(dir);
            
            _doc = new XmlDocument();

            // load doc
            _doc.Load(_directory);


            // Get nodes from XML
            Node head;

            using (var stream = new StringReader(_doc.InnerXml))
            {
                // TODO: refactor object structure so this actually works
                var reader = new XmlSerializer(typeof(Node));

                head = (Node) reader.Deserialize(stream);
            }

            Result = head;
        }
    }
}
