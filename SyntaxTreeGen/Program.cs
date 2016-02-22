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
            //var nodes = new List<Node>();

            //// gt = "5 + 10 > 5"
            //var five = new ConstantNode<int>(5);
            //var ten = new ConstantNode<int>(10);
            
            //var fiveplusten = new OperatorNode(OpKind.Add, five, ten);
            //var gt = new OperatorNode(OpKind.GreaterThan, fiveplusten, five);

            //// assign = " res = x "
            //var assignResX = new AssignNode( new VarNode(typeof (int), "res"), new VarNode(typeof(string), "x"));

            //var innerIf = new IfNode(new ConstantNode<bool>(true), assignResX);

            //// Build "ELSE res = 10"
            //var elsePart = new AssignNode(new VarNode(typeof(int), "res"), ten);

            //// ifNode = "if [GT] { res = x } else { res = 10 } " 
            //var ifNode = new IfNode(gt, innerIf, elsePart);

            //var ifNode2 = new IfNode(gt, new IfNode(new ConstantNode<bool>(true),
            //                             new AssignNode(new VarNode(typeof(int), "res"), new VarNode(typeof(string), "x"))),
            //                             new AssignNode(new VarNode(typeof(int), "res"), ten));


            //var res = ifNode.ToString();
            //var res2 = ifNode2.ToString();

            //Console.WriteLine(res);
            //Console.WriteLine(res2);
            
            // Class
            var testClass = new ClassNode(
                ClassNode.ProtectionLevelKind.Public, false, "FooClass",
                // First method
                new MethodNode("Capitalise", true, MethodNode.ProtectionLevelKind.Private, typeof(void),
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
            
            var res = testClass.ToString();

            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}