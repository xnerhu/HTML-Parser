using System;

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

        #region Attributes

        public int GetAttributeIndex(string property) {
            for (int i = 0; i < Attributes.Count; i++) {
                if (Attributes[i].Property == property.ToLower()) return i;
            }

            return -1;
        }

        public bool HasAttribute(string property) {
            return GetAttributeIndex(property) != -1;
        }

        public void SetAttribute(string property, string value) {
            int attributeIndex = GetAttributeIndex(property);

            if (attributeIndex != -1) {
                Attributes[attributeIndex].Value = value;
            } else {
                Attributes.Add(new DOMElementAttribute() {
                    Property = property,
                    Value = value
                });
            }
        }

        public string GetAttribute(string property) {
            int index = GetAttributeIndex(property);
            return index != -1 ? Attributes[index].Value : null;
        }

        #endregion

        #region InnerHTML
        
        public string GetInnerHTML () {
            return TagUtils.MinifyHTML(Children);
        }

        public string SetInnerHTML(string innerHTML) {
            Children = DOMTree.Get(HTML.GetTagsList(innerHTML), innerHTML);

            return innerHTML;
        }

        #endregion
    }
}
