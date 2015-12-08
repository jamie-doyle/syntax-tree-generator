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

            var plus = new OperatorNode(OpKind.Add, five, ten);
            var gt = new OperatorNode(OpKind.GreaterThan, plus, five);

            // assign = " res = x "
            var assign = new OperatorNode(OpKind.Equals, new VarNode(typeof(string), "res"),
                new VarNode(typeof(string), "x"));

            var innerIf = new IfNode(new ConstantNode<bool>(true), assign);

            // ifNode = "if [GT] { res = x }" 
            var ifNode = new IfNode(gt, innerIf);

            var res = ifNode.ToString();

            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}
