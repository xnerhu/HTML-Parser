using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Printer {
        public static void Print(List<Node> tree, bool printClosing = true) {
            ConsoleColor defaultColor = Console.ForegroundColor;

            int lastLevel = 0;
            PrintChildren(tree, printClosing, ref lastLevel);

            Console.ForegroundColor = defaultColor;
        }

        private static void PrintChildren(List<Node> tree, bool printClosing, ref int lastLevel, int level = 0) {
            lastLevel = level;

            foreach (Node node in tree) {
                string gap = new string(' ', 2 * level);

                if (node.NodeType == NodeType.TEXT_NODE) {
                    Console.ForegroundColor = DefaultColors.Text;
                    Console.WriteLine(gap + node.NodeValue);
                } else if (node.NodeType == NodeType.ELEMENT_NODE) {
                    Console.ForegroundColor = DefaultColors.Tag;
                    Console.WriteLine(string.Format("{0}<{1}>", gap, node.NodeName));

                    if (node.ChildNodes.Count > 0) {
                        PrintChildren(node.ChildNodes, printClosing, ref lastLevel, level + 1);
                    }
                }

                if (printClosing && node.NodeType == NodeType.ELEMENT_NODE && (level < lastLevel || level == 0 || node.ParentNode.ChildNodes.Count - 1 == 0)) {
                    Console.ForegroundColor = DefaultColors.Tag;
                    Console.WriteLine(string.Format("{0}</{1}>", gap, node.NodeName));
                }
            }
        }

        public static void PrintTokens(List<string> tokens) {             
            foreach (string token in tokens) {
                Console.WriteLine(token);
            }
        }
    }
}
