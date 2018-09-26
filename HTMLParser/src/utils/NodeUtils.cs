using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class NodeUtils {
        public static NodeType GetNodeType(string token) {
            if (token.Length >= 3) {
                if (token.StartsWith("<!--") || token.EndsWith("-->")) {
                    return NodeType.COMMENT_NODE;
                } else if (token[1] == '!') {
                    return NodeType.DOCUMENT_TYPE_NODE;
                } else if (token[0] == '<' && token[token.Length - 1] == '>') {
                    return NodeType.ELEMENT_NODE;
                }
            }

            return NodeType.TEXT_NODE;
        }

        /// <summary>
        /// Extracts text from comment
        /// </summary>
        public static string ExtractCommentContent(string token) {
            bool capturingText = false;
            int endIndex = token.Length;
            string text = "";

            // Correct comment is ended with 2 hyphens and one bracket (greater)
            if (token.EndsWith("-->")) {
                endIndex -= 3;
            }

            // Ignore bracket and hyphens at start, then get text
            for (int i = 2; i < endIndex; i++) {
                if (!capturingText && token[i] != '<' && token[i] != '-') {
                    capturingText = true;
                }

                if (capturingText) {
                    text += token[i];
                }
            }

            return text;
        }

        /// <summary>
        /// Executes a callback for each node including it's children
        /// </summary>
        public static void Iterate(List<Node> nodes, Predicate<Node> predicate, Action<Node> action) {
            foreach (Node node in nodes) {
                if (predicate == null || predicate.Invoke(node)) {
                    action(node);
                }

                if (node.NodeType == NodeType.ELEMENT_NODE && node.ChildNodes.Count > 0) {
                    Iterate(node.ChildNodes, predicate, action);
                }
            }
        }
    }
}
