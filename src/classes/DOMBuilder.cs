using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class DOMBuilder {
        /// <summary>
        /// Builds a DOM tree from given tokens
        /// </summary>
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

                if (node.NodeType == NodeType.TEXT_NODE) {
                    node.NodeName = "#text";
                    node.NodeValue = token;
                } else if (node.NodeType == NodeType.COMMENT_NODE) {
                    node.NodeName = "#comment";
                    node.NodeValue = NodeUtils.ExtractCommentContent(token);
                } else {
                    string tagName = TagUtils.GetTagName(token);
                    TagType tagType = TagUtils.GetTagType(token);

                    if (tagType == TagType.Opening) {
                        node.NodeName = tagName;
                        node.ChildNodes = new List<Node>();

                        parentNode = node;
                        openedTags.Add(tagName);
                    } else {
                        int index = openedTags.LastIndexOf(tagName);

                        if (index != -1) {
                            if (parentNode.NodeName == tagName) {
                                parentNode = parentNode.ParentNode;
                            } else {
                                Node parent = TagUtils.GetParentTag(tagName, parentNode.ParentNode);

                                if (parent != null && parent.ParentNode != null) {
                                    parentNode = parent.ParentNode;
                                }
                            }

                            openedTags.RemoveAt(index);
                        }

                        continue;
                    }
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
