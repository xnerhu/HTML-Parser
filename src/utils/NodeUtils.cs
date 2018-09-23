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

        public static string ExtractCommentContent(string token) {
            string text = "";
            bool capturingText = false;
            int endIndex = token.Length;

            if (token.EndsWith("-->")) {
                endIndex -= 3;
            }

            for (int i = 2; i < endIndex; i++) {
                if (!capturingText && token[i] != '<' && token[i] != '-') {
                    capturingText = true;
                }

                if (capturingText) text += token[i];
            }

            return text;
        }
    }
}
