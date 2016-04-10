using System;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxTreeGen.Models;
using static SyntaxTreeGen.Models.OperatorNode;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path 
                = @"C:\Users\Jamie\Source\Repos\Syntax Tree\SyntaxTreeGen\Test.xml";
            var doc = new XmlSyntaxTree();
            Node docNode = doc.Import(path);
            
            Console.WriteLine(docNode.ToString());
            Console.ReadLine();

            // Class
            var testClass = new ClassNode(
                ClassNode.ProtectionLevelKind.Public, false, "FooClass",
                // First method
                new MethodNode("Capitalise", true, MethodNode.ProtectionLevelKind.Private, "void",
                    // Parameters
                    new Node[]
                    {
                        new VarNode(typeof(int), "x")
                    },
                    // Statements
                    new Node[]
                    {
                        // assign ten -> x
                        new AssignNode(new VarNode(typeof(int), "ten"), new ConstantNode<int>(10)),

                        // assing (x ^ 2) -> res
                        new AssignNode(
                            new VarNode(typeof(int), "res"), 
                            new OperatorNode(new VarNode(typeof(int), "res"), OpKind.ToPowerOf, new ConstantNode<int>(2)) 
                            )
                    }
                ) // END METHOD
            );

            //TODO: Generate tutorial programs from node representation (Deitel & Deitel)
            
            var assignResX = new AssignNode( new VarNode(typeof (int), "res"), new VarNode(typeof(string), "x"));

            var innerIf = new IfNode(new ConstantNode<bool>(true), assignResX);
            
            //var ifMethod = new MethodNode("IfStatement", false, MethodNode.ProtectionLevelKind.Public, )

            var res = testClass.ToString();

            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}