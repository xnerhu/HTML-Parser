using System.Collections.Generic;

namespace HTMLParser {
    public class Node {
        public NodeType NodeType;
        public string NodeName;
        public string NodeValue;
        public Node ParentNode;
        public List<Node> ChildNodes;
        public List<Node> Attributes;
    }
}
