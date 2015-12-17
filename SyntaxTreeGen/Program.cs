using System;
using System.Collections.Generic;
using SyntaxTreeGen.Models;
using static SyntaxTreeGen.Models.OperatorNode;

namespace SyntaxTreeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = new List<Node>();

            // gt = "5 + 10 > 5"
            var five = new ConstantNode<int>(5);
            var ten = new ConstantNode<int>(10);

            var fiveplusten = new OperatorNode(OpKind.Add, five, ten);
            var gt = new OperatorNode(OpKind.GreaterThan, fiveplusten, five);

            // assign = " res = x "
            var assignResX = new AssignNode( new VarNode(typeof (int), "res"), new VarNode(typeof(string), "x"));

            var innerIf = new IfNode(new ConstantNode<bool>(true), assignResX);

            // Build "ELSE res = 10"
            var elsePart = new AssignNode(new VarNode(typeof(int), "res"), ten);

            // ifNode = "if [GT] { res = x } else { res = 10 } " 
            var ifNode = new IfNode(gt, innerIf, elsePart);

            var res = ifNode.ToString();

            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}
