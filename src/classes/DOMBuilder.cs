using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class DOMBuilder {
        public static List<Node> Build(List<string> tokens) {
            List<Node> tree = new List<Node>();
            Node parentNode = null;

            List<string> openedTags = new List<string>();

            for (int i = 0; i < tokens.Count; i++) {
                string token = tokens[i];

                Node node = new Node() {
                    nodeType = GetNodeType(token),
                    parentNode = parentNode
                };

                if (node.nodeType == NodeType.ELEMENT_NODE) {
                    TagType tagType = GetTagType(token);

                    if (tagType == TagType.Closing) {
                        string tokenTagName = GetTagName(token);
                        int index = openedTags.LastIndexOf(tokenTagName);

                        if (index != -1) {
                            if (parentNode.nodeName == tokenTagName) {
                                parentNode = parentNode.parentNode;
                            } else {
                                Node parent = GetParentTag(tokenTagName, parentNode.parentNode);

                                if (parent != null && parent.parentNode != null) {
                                    parentNode = parent.parentNode;
                                }
                            }

                            openedTags.RemoveAt(index);
                        }

                        continue;
                    } else {
                        node.nodeName = GetTagName(token);
                        parentNode = node;

                        openedTags.Add(node.nodeName);
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

        private static Node GetParentTag(string tagName, Node node) {
            if (node == null) return null;

            if (node.nodeName == tagName) {
                return node;
            } else {
                return GetParentTag(tagName, node.parentNode);
            }
        }

        private static string GetTagName(string token) {
            string tagName = "";

            for (int i = 0; i < token.Length; i++) {
                if (token[i] == '>' || token[i] == ' ') {
                    return tagName;
                } else if (token[i] != '<' && token[i] != '/') {
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
