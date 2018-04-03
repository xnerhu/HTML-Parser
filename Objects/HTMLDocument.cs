using System.Collections.Generic;

namespace HTMLParser {
    public class HTMLDocument {
        public List<DOMElement> DOMTree = new List<DOMElement>();
        public List<DOMElement> Meta;

        public Statistics Stats = new Statistics();

        public HTMLDocument (string source) {
            this.DOMTree = HTML.Parse(source, ref Stats);
            this.Meta = GetElementsByName("meta");           
        }

        public List<DOMElement> GetElementsByName (string name) {
            List<DOMElement> list = new List<DOMElement>();

            for (int i = 0; i < this.DOMTree.Count; i++) {
                DOMElement element = this.DOMTree[i];

                if (element.TagName == name) list.Add(element);
            }

            return list;
        }
    }
}
