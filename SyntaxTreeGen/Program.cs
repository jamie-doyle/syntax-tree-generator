using System;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        { 
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