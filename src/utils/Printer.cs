using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Printer {
        public static void Print(List<Node> tree, bool printClosing = true) {
            int lastLevel = 0;
            PrintChildren(tree, printClosing, ref lastLevel);

            Console.ResetColor();
        }

        private static void PrintChildren(List<Node> tree, bool printClosing, ref int lastLevel, int level = 0) {
            lastLevel = level;

            foreach (Node node in tree) {
                string gap = new string(' ', 2 * level);

                if (node.NodeType == NodeType.TEXT_NODE) {
                    PrintColored(gap + node.NodeValue, DefaultColors.Text);
                } else if (node.NodeType == NodeType.COMMENT_NODE) {
                    PrintColored(gap + string.Format("<!--{0}-->", node.NodeValue), DefaultColors.Comment);
                } else if (node.NodeType == NodeType.ELEMENT_NODE) {
                    PrintColored(string.Format("{0}<{1}", gap, node.NodeName), DefaultColors.Tag, true);                

                    if (node.Attributes.Count > 0) {
                        foreach (Node attr in node.Attributes) {
                            PrintColored(' ' + attr.NodeName, DefaultColors.Property, true);

                            if (attr.NodeValue != null) {
                                PrintColored("=\"", DefaultColors.Tag, true);
                                PrintColored(attr.NodeValue, DefaultColors.Value, true);
                                PrintColored("\"", DefaultColors.Tag, true);
                            }
                        }
                    }

                    PrintColored(">\n", DefaultColors.Tag, true);

                    if (node.ChildNodes.Count > 0) {
                        PrintChildren(node.ChildNodes, printClosing, ref lastLevel, level + 1);
                    }

                    if (printClosing && (level < lastLevel || level == 0 || node.ParentNode.ChildNodes.Count - 1 == 0)) {
                        PrintColored(string.Format("{0}</{1}>", gap, node.NodeName), DefaultColors.Tag);
                    }
                }
            }
        }

        public static void PrintTokens(List<string> tokens) {             
            foreach (string token in tokens) {
                Console.WriteLine(token);
            }
        }

        private static void PrintColored(string text, ConsoleColor color, bool inline = false) {
            Console.ForegroundColor = color;
            if (!inline) Console.WriteLine(text);
            else Console.Write(text);
        }
    }
}
