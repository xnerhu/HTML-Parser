namespace HTMLParser {
    public static class TagUtils {
        public static TagType GetType(string source) {
            if (source[0] == '/') return TagType.Closing;
            else if (source[source.Length - 1] == '/') return TagType.SelfClosing;

            return TagType.Opening;
        }

        public static string GetCode(string source, int startIndex, int endIndex) {
            return source.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        public static string GetName(string source) {
            if (source.Length == 0) return null;

            int startIndex = 0;
            int endIndex = 0;

            if (source[0] == '/') startIndex++;

            for (int i = startIndex; i < source.Length; i++) {
                bool isCharSpace = source[i] == ' ';

                if (isCharSpace || i + 1 == source.Length) {
                    endIndex = isCharSpace ? i - 1 : i;
                    break;
                }
            }

            return source.Substring(startIndex, endIndex - startIndex + 1).ToLower();
        }

        public static int GetOpeningTagIndex(CList<OpeningTag> collection, string tagName) {
            for (int i = 0; i < collection.Count; i++) {
                if (collection[i].TagName == tagName) return i;
            }

            return -1;
        }

        public static CList<DOMElement> ParseDOMTreeToList(CList<DOMElement> tree) {
            CList<DOMElement> list = new CList<DOMElement>();

            for (int i = 0; i < tree.Count; i++) {
                DOMElement element = tree[i];

                list.Add(element);

                if (element.Children.Count > 0) {
                    CList<DOMElement> children = ParseDOMTreeToList(element.Children);

                    for (int c = 0; c < children.Count; c++) {
                        if (children[c].TagCode != null) {
                            list.Add(children[c]);
                        }
                    }
                }
            }

            return list;
        }

        public static bool IsTagIgnored (string tagName) {
            tagName = tagName.ToLower();

            for (int it = 0; it < HTMLSpecialList.ignoredTags.Count; it++) {
                if (tagName == HTMLSpecialList.ignoredTags[it]) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsTagSelfClosing(DOMElement element) {
            if (element.Type == TagType.Opening) {
                for (int l = 0; l < HTMLSpecialList.selfClosingTags.Count; l++) {
                    if (element.TagName == HTMLSpecialList.selfClosingTags[l]) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static int GetClosestClosingTagIndex(string source, int startIndex, string searchedTagName) {
            int tagStartIndex = -1;

            for (int i = startIndex; i < source.Length; i++) {
                if (source[i] == '<') {
                    int tagEndIndex = Utils.SearchForClosestChar(source, '>', i + 1);
                    string tagCode = TagUtils.GetCode(source, i, tagEndIndex);
                    string tagName = TagUtils.GetName(tagCode);
                    TagType tagType = TagUtils.GetType(tagCode);

                    if (tagType == TagType.Closing && tagName == searchedTagName) {
                        tagStartIndex = i;
                        break;
                    }
                }
            }

            return tagStartIndex == -1 ? source.Length : tagStartIndex;
        }
    }
}
