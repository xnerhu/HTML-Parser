using System.Collections.Generic;

namespace HTMLParser {
    public static class DOMBuilder {
        public static List<string> SelfClosingTags = new List<string>() {
            "area", "base", "br", "col", "command", "embed", "hr", "img", "input",
            "keygen", "link", "menuitem", "meta", "param", "source", "track", "wbr"
        };

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

                // If node is a text, then it's name is #text and node value is content
                if (node.NodeType == NodeType.TEXT_NODE) {
                    node.NodeName = "#text";
                    node.NodeValue = token;
                } else if (node.NodeType == NodeType.COMMENT_NODE) { // Same as a text node, but name is #comment instead of #text
                    node.NodeName = "#comment";
                    // Extract text from a comment token
                    node.NodeValue = NodeUtils.ExtractCommentContent(token);
                } else if (node.NodeType == NodeType.ELEMENT_NODE) {
                    // Extract tag name and it's type from token
                    string tagName = TagUtils.GetTagName(token).ToLower();
                    TagType tagType = TagUtils.GetTagType(token);

                    // If tag is opening
                    if (tagType == TagType.Opening) {
                        node.NodeName = tagName;
                        node.ChildNodes = new List<Node>();
                        // Extract attributes e.g class or id
                        node.Attributes = TagUtils.GetAttributes(token, tagName);
                        // Check if tag is self-closing
                        bool isSelfClosing = SelfClosingTags.Exists(e => e == node.NodeName);

                        // If tag isn't self-closing then
                        // update current parent and add tag to the list of opened tags for later validation
                        if (!isSelfClosing) {
                            parentNode = node;
                            openedTags.Add(tagName);
                        }
                    } else { // If tag is closing
                        int index = openedTags.LastIndexOf(tagName);

                        // Validation for not opened tags
                        if (index != -1) {
                            // If name of closing tag is the same as name of current parent,
                            // then everything is correct
                            if (parentNode.NodeName == tagName) {
                                parentNode = parentNode.ParentNode;
                            } else {
                                // If something went wrong, then search for opening tag,
                                // that has the same name as closing tag 
                                Node parent = TagUtils.GetParentTag(tagName, parentNode.ParentNode);

                                if (parent != null && parent.ParentNode != null) {
                                    parentNode = parent.ParentNode;
                                }
                            }

                            // Remove tag from the list
                            openedTags.RemoveAt(index);
                        }

                        continue;
                    }
                }

                // Add node
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
