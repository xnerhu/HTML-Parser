using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Printer {
        public static void PrintTree(List<Node> tree, int level = 0) {
            foreach (Node node in tree) {
                string gap = new string(' ', 2 * level);

                Console.Write('\n' + gap);

                if (node.nodeType == NodeType.ELEMENT_NODE) {
                    Console.Write('<' + node.nodeName + '>');
                    PrintTree(node.childNodes, level + 1);
                } else {
                    Console.Write(node.nodeValue);
                }
            }
        }
    }
}
