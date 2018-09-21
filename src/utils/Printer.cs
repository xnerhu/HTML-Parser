using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Printer {
        public static void Print(List<Node> tree, bool printClosing = true) {
            int lastLevel = 0;
            PrintChildren(tree, printClosing, ref lastLevel);
        }

        private static void PrintChildren(List<Node> tree, bool printClosing, ref int lastLevel, int level = 0) {
            lastLevel = level;

            foreach (Node node in tree) {
                string gap = new string(' ', 2 * level);

                if (node.nodeType == NodeType.TEXT_NODE) {
                    Console.WriteLine(gap + node.nodeValue);
                } else if (node.nodeType == NodeType.ELEMENT_NODE) {
                    Console.WriteLine(string.Format("{0}<{1}>", gap, node.nodeName));

                    if (node.childNodes.Count > 0) {
                        PrintChildren(node.childNodes, printClosing, ref lastLevel, level + 1);
                    }
                }

                if (printClosing && (level < lastLevel || level == 0)) {
                    Console.WriteLine(string.Format("{0}</{1}>", gap, node.nodeName));
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
