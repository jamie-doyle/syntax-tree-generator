using System;
using System.Text;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        { 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var left  = new VarNode("StringBuilder", "sb");
            //var right = new ExternalCallNode(false, new [] {"System", "Text", "StringBuilder"});

            //var assign = new AssignNode(left, right);
           // Console.Write(assign);
            Console.ReadLine();

            /* Example - Construct a class with two fields, and no methods, then print to console */
            var classNode = new ClassNode
            {
                Name = "TestClass",
                ClassAccessLevel = Node.AccessLevel.Public,
                Fields = new[] {new VarNode("int", "TestInt"), new VarNode("string", "TestString")},
                IsStatic = true
            };
            
            Console.WriteLine(classNode);
            Console.ReadLine();

            //TODO: Generate tutorial programs from node representation (Deitel & Deitel)
        }
    }
}