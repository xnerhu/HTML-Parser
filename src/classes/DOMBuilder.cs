using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class DOMBuilder {
        public static List<Node> Build(List<string> tokens) {
            List<Node> tree = new List<Node>();
            List<string> openedTags = new List<string>();
            Node parentNode = null;

            for (int i = 0; i < tokens.Count; i++) {
                string token = tokens[i];

                Node node = new Node() {
                    NodeType = NodeUtils.GetNodeType(token),
                    ParentNode = parentNode
                };

                if (node.NodeType == NodeType.ELEMENT_NODE) {
                    TagType tagType = TagUtils.GetTagType(token);

                    if (tagType == TagType.Closing) {
                        string tokenTagName = TagUtils.GetTagName(token);
                        int index = openedTags.LastIndexOf(tokenTagName);

                        if (index != -1) {
                            if (parentNode.NodeName == tokenTagName) {
                                parentNode = parentNode.ParentNode;
                            } else {
                                Node parent = TagUtils.GetParentTag(tokenTagName, parentNode.ParentNode);

                                if (parent != null && parent.ParentNode != null) {
                                    parentNode = parent.ParentNode;
                                }
                            }

                            openedTags.RemoveAt(index);
                        }

                        continue;
                    } else {
                        node.ChildNodes = new List<Node>();
                        node.NodeName = TagUtils.GetTagName(token);

                        parentNode = node;
                        openedTags.Add(node.NodeName);
                    }
                } else {
                    node.NodeName = "#text";
                    node.NodeValue = token;
                }

                if (node.ParentNode == null) {
                    tree.Add(node);
                } else {
                    node.ParentNode.ChildNodes.Add(node);
                }
            }

            return tree;
        }
    }
}
