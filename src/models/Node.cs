using System.Collections.Generic;

namespace HTMLParser {
    public class Node {
        public NodeType NodeType { get; set; }
        public string NodeName { get; set; }
        public string NodeValue { get; set; }
        public Node ParentNode { get; set; }
        public List<Node> ChildNodes { get; set; }
    }
}
