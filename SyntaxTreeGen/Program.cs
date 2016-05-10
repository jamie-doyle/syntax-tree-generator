using System;
using System.Text;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML;

namespace SyntaxTreeGen
{
    class Program
    {
        /// <summary>
        /// Example of manual instantiation
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {   
            /* NB - The full program can be ran by building the Viewer project */

            // This will print as public static class TestClass { ... }
            var classNode = new ClassNode
            {
                Name = "TestClass",
                ClassAccessLevel = Node.AccessLevel.Public,
                Fields = new[] {new VarNode("int", "TestInt"), new VarNode("string", "TestString")},
                Methods = new []
                {
                    // Declares a method - "public static void Main(string[] args)
                    new MethodNode("Main", true, Node.AccessLevel.Public, "void", new []{new VarNode("string[]", "args", true) }),
                },
                IsStatic = true
            };
            
            Console.WriteLine(classNode);
            Console.ReadLine();
        }
    }
}