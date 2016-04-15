using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.Parser;
using static SyntaxTreeGen.Models.OperatorNode;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Users\Jamie\Source\Repos\Syntax Tree\SyntaxTreeGen\Test.xml";
            
            try
            {
                var docRes = XmlSyntaxTree.Import(path);
                Console.WriteLine(docRes.ToString());
            Console.ReadLine();
            }
            catch (XmlException ex)
            {
                Console.WriteLine("This document could not be read as " + ex.Message + ".");
                if (ex.LineNumber > 0)
                    Console.WriteLine("The error ocurred on line "+ex.LineNumber+", position "+ex.LinePosition);
            }
           
            // Class
            var testClass = new ClassNode(
                ClassNode.AccessLevel.Public, false, "FooClass",
                // First method
                new MethodNode("Capitalise", true, MethodNode.AccessLevel.Private, "void",
                    // Parameters
                    new Node[]
                    {
                        new VarNode("int", "x")
                    },
                    // Statements
                    new Node[]
                    {
                        // assign ten -> x
                        new AssignNode(new VarNode("int", "ten"), new ConstantNode(10)),

                        // assing (x ^ 2) -> res
                        new AssignNode(
                            new VarNode("int", "res"), 
                            new OperatorNode(new VarNode("int", "res"), OpKind.ToPowerOf, new ConstantNode(2)) 
                            )
                    }
                ) // END METHOD
            );

            //TODO: Generate tutorial programs from node representation (Deitel & Deitel)
            
            var assignResX = new AssignNode( new VarNode("int", "res"), new VarNode("int", "x"));

            var innerIf = new IfNode(new ConstantNode(true), assignResX);
            
            //var ifMethod = new MethodNode("IfStatement", false, MethodNode.ProtectionLevelKind.Public, )

            var res = testClass.ToString();

            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}