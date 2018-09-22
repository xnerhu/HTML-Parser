namespace HTMLParser {
    public static class NodeUtils {
        public static NodeType GetNodeType(string token) {
            if (token.Length >= 3 && token.StartsWith('<') && token.EndsWith('>')) {
                return NodeType.ELEMENT_NODE;
            }

            return NodeType.TEXT_NODE;
        }
    }
}
