using System.Collections.Generic;

namespace HTMLParser {
    public class DOMElement {
        public string TagName;
        public string TagCode;
        public int TagStartIndex = -1;
        public int TagEndIndex = -1;
        public TagType Type;
        public string Content;
        public string HelperText;

        public List<DOMElement> Children = new List<DOMElement>();
        public List<Attribute> Attributes = new List<Attribute>();

        public DOMElement () {

        }
    }
}
