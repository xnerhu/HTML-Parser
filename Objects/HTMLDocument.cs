using System.Collections.Generic;

namespace HTMLParser {
    public class HTMLDocument {
        public List<DOMElement> Elements = new List<DOMElement>();

        public HTMLDocument (string source) {
            Elements = HTML.Parse(source);
        }
    }
}
