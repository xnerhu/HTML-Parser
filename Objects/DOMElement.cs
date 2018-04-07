using System.Collections.Generic;

namespace HTMLParser {
    public class DOMElement {
        public TagType Type;
        public string TagName;
        public string TagCode;
        public string Content;

        public int TagStartIndex = -1;
        public int TagEndIndex = -1;
        public string HelperText;

        public CList<DOMElement> Children = new CList<DOMElement>();
        public CList<DOMElementAttribute> Attributes = new CList<DOMElementAttribute>();
    }
}
