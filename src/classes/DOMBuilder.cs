using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HTMLParser {
    public static class DOMBuilder {
        public static List<Node> Build(List<string> tokens) {
            List<Node> tree = new List<Node>();
            Node parentNode = null;

            foreach (string token in tokens) {
                Node node = new Node() {
                    nodeType = GetNodeType(token),
                    parentNode = parentNode
                };

                if (node.nodeType == NodeType.ELEMENT_NODE) {
                    TagType tagType = GetTagType(token);

                    if (tagType == TagType.Closing) {
                        if (parentNode != null && parentNode.parentNode != null) {
                            parentNode = parentNode.parentNode;
                        }

                        continue;
                    } else {
                        node.nodeName = ExtractTagName(token);

                        if (tagType == TagType.Opening) {
                            parentNode = node;
                        }
                    }
                } else {
                    node.nodeName = "#text";
                    node.nodeValue = token;
                }

                if (node.parentNode == null) {
                    tree.Add(node);
                } else {
                    node.parentNode.childNodes.Add(node);
                }
            }

            return tree;
        }

        private static string ExtractTagName(string token) {
            string tagName = "";

            for (int i = 0; i < token.Length; i++) {
                if (token[i] == '>' || token[i] == ' ') {
                    return tagName;
                } else if (token[i] != '<') {
                    tagName += token[i];
                }
            }

            return null;
        }

        private static NodeType GetNodeType(string token) {
            if (token.Length >= 3 && token.StartsWith('<') && token.EndsWith('>')) {
                return NodeType.ELEMENT_NODE;
            }

            return NodeType.TEXT_NODE;
        }
        
        private static TagType GetTagType(string token) {
            if (token.Length >= 4 && token.StartsWith("</")) {
                return TagType.Closing;
            } else if (token.Length >= 4 && token.EndsWith("/>")) {
                return TagType.SelfClosing;
            }

            return TagType.Opening;
        }
    }
}
