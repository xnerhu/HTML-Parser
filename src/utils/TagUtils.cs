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
    }
}
