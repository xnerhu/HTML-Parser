using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class TagUtils {
        public static TagType GetType(string str) {
            if (str[0] == '/') {
                return TagType.Closing;
            } else if (str[str.Length - 1] == '/') {
                return TagType.SelfClosing;
            }

            return TagType.Opening;
        }

        public static string GetCode(string source, int startIndex, int endIndex) {
            return source.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        public static string GetName(string code) {
            if (code.Length == 0) return null;

            char firstChar = code[0];
            int startIndex = 0;
            int endIndex = 0;

            // TODO: Comments support
            if (firstChar == '/') startIndex++;

            for (int i = startIndex; i < code.Length; i++) {
                bool isCharSpace = code[i] == ' ';

                if (isCharSpace || i + 1 == code.Length) {
                    endIndex = isCharSpace ? i - 1 : i;
                    break;
                }
            }

            return code.Substring(startIndex, endIndex - startIndex + 1).ToLower();
        }

        public static List<DOMElement> GetTagsListFromDOMTree (List<DOMElement> tree) {
            List<DOMElement> list = new List<DOMElement>();

            for (int i = 0; i < tree.Count; i++) {
                DOMElement element = tree[i];

                list.Add(element);

                if (element.Children.Count > 0) {
                    List<DOMElement> children = GetTagsListFromDOMTree(element.Children);

                    for (int c = 0; c < children.Count; c++) {
                        if (children[c].TagCode != null) {
                            list.Add(children[c]);
                        }
                    }
                }
            }

            return list;
        }

        public static List<DOMElement> GetElementsFromList(List<DOMElement> list, int startIndex, int endIndex) {
            List<DOMElement> elements = new List<DOMElement>();

            if (startIndex > endIndex) {
                throw new Exception("Start index is greater than end index");
            } else if (list.Count < endIndex) {
                throw new Exception("End index is greather than list size");
            }

            for (int i = startIndex; i < endIndex; i++) {
                elements.Add(list[i]);
            }

            return elements;
        }
    }
}
