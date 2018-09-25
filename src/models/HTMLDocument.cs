using System.Collections.Generic;

namespace HTMLParser {
    public class HTMLDocument {
        public Node Body;
        public Node DocumentElement;
        public List<Node> Children = new List<Node>();
        public List<Node> Links = new List<Node>();
        public List<Node> Scripts = new List<Node>();
        public List<Node> StyleSheets = new List<Node>();

        public HTMLDocument(string source, bool logDetails = true) {
            List<string> tokens = Tokenizer.Tokenize(source);
            List<Node> nodes = DOMBuilder.Build(tokens);

            if (logDetails) {
                // Iterate every node to get info such as links count
                NodeUtils.Iterate(nodes, null, (Node node) => {
                    if (node.NodeName == "html") {
                        DocumentElement = node;
                    } else if (node.NodeName == "body") {
                        Body = node;
                    } else if (node.NodeName == "a") {
                        Links.Add(node);
                    } else if (node.NodeName == "script") {
                        Scripts.Add(node);
                    } else if (node.NodeName == "link" || node.NodeName == "style") {
                        StyleSheets.Add(node);
                    }
                });
            }

            Children = nodes;
        }
    }
}
