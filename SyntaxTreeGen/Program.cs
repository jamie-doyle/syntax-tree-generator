using System;
using SyntaxTreeGen.XML;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            const string testPath = @"C:\Users\Jamie\Source\Repos\Syntax Tree\SyntaxTreeGen\XML\Examples\";
            const string helloWorld = @"Test.xml";
           
            var doc = new Document(testPath + helloWorld);

            var docnodes = doc.GetNodes();

            Console.WriteLine(docnodes);
            Console.ReadLine();

            //TODO: Generate tutorial programs from node representation (Deitel & Deitel)
        }
    }
}