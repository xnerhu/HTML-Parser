using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class TagUtils {
        public static TagType GetTagType(string token) {
            if (token.Length >= 4 && token.StartsWith("</")) {
                return TagType.Closing;
            } else if (token.Length >= 4 && token.EndsWith("/>")) {
                return TagType.SelfClosing;
            }

            return TagType.Opening;
        }

        public static Node GetParentTag(string tagName, Node node) {
            if (node != null) {
                if (node.NodeName == tagName) {
                    return node;
                } else {
                    return GetParentTag(tagName, node.ParentNode);
                }
            }

            return null;
        }

        public static string GetTagName(string token) {
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

        public static List<Node> GetAttributes(string token, string tagName = null) {
            if (tagName == null) {
                tagName = GetTagName(token);
            }

            List<Node> attributes = new List<Node>();
            bool capturingValue = false;

            Node attribute = new Node() {
                NodeType = NodeType.ATTRIBUTE_NODE
            };

            bool foundQuote = false;

            for (int i = tagName.Length + 2; i < token.Length; i++) {
                if (!capturingValue) {
                    if (token[i] == '=') {
                        capturingValue = true;

                        foundQuote = token[i + 1] == '"';
                        if (foundQuote) i++;
                    } else if (token[i] == ' ' || i == token.Length - 1) {
                        attributes.Add(attribute);

                        attribute = new Node() {
                            NodeType = NodeType.ATTRIBUTE_NODE
                        };
                    } else {
                        attribute.NodeName += token[i];
                    }
                } else {
                    if (token[i] == '"' || i == token.Length - 1 || !foundQuote && token[i] == ' ') {
                        capturingValue = false;
                        attributes.Add(attribute);

                        attribute = new Node() {
                            NodeType = NodeType.ATTRIBUTE_NODE
                        };

                        i++;
                    } else {
                        attribute.NodeValue += token[i];
                    }
                }
            }

            foreach (Node attr in attributes) {
                Console.WriteLine(string.Format("Property: {0}\nValue: {1}\n", attr.NodeName, attr.NodeValue));
            }

            return attributes;
        } 
    }
}
