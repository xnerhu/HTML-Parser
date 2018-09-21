using System.Collections.Generic;

namespace HTMLParser {
    public class Node {
        public NodeType nodeType;
        public string nodeName;
        public string nodeValue;
        public Node parentNode;
        public List<Node> childNodes = new List<Node>();
    }
}
