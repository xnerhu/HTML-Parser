using System.Collections.Generic;

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

        public static int GetOpeningTagIndex(List<OpeningTag> collection, string tagName) {
            for (int i = 0; i < collection.Count; i++) {
                if (collection[i].TagName == tagName) return i;
            }

            return -1;
        }
    }
}
