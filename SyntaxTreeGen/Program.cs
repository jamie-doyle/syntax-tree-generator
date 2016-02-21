using System;
using System.Collections.Generic;
using System.Net.Mime;
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
                new MethodNode("FooMethod", true, MethodNode.ProtectionLevelKind.Private, typeof(void),
                    // Parameter
                    new VarNode(typeof(int), "x")
                )
           );

            //var method = new MethodNode("Test", true, MethodNode.ProtectionLevelKind.Private, typeof(void) );
            Console.WriteLine(testClass.ToString());

            Console.ReadLine();
        }
    }
}
