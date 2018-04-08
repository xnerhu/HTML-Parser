using System.Diagnostics;

namespace HTMLParser {
    public static class HTML {
        /// <summary>
        /// Parses source code
        /// </summary>
        public static CList<DOMElement> Parse(string source, ref Statistics stats) {
            source = source.Trim();

            Stopwatch watch = Stopwatch.StartNew();

            // Parse source code to tags list
            CList<DOMElement> tagsList = GetTagsList(source);
            // Get time
            watch.Stop();
            stats.SourceCodeParsingTime = (int)watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();

            // Parse tags list to DOM tree
            CList<DOMElement> tree = DOMTree.Get(tagsList, source);

            // Get time
            watch.Stop();
            stats.DOMTreeParsingTime = (int)watch.ElapsedMilliseconds;

            return tree;
        }

        /// <summary>
        /// Parses source code to objects
        /// </summary>
        public static CList<DOMElement> GetTagsList(string source) {
            CList<DOMElement> tags = new CList<DOMElement>();

            bool ignoreNewElements = false;

            for (int i = 0; i < source.Length; i++) {
                if (source[i] == '<') {
                    int tagEndIndex = Utils.SearchForClosestChar(source, '>', i + 1);
                    string tagCode = TagUtils.GetCode(source, i, tagEndIndex);

                    // Create new element
                    DOMElement element = new DOMElement() {
                        TagCode = tagCode,
                        TagName = TagUtils.GetName(tagCode),
                        Type = TagUtils.GetType(tagCode),
                        TagStartIndex = i,
                        TagEndIndex = tagEndIndex
                    };

                    if (element.Type == TagType.SelfClosing) {
                        element.TagCode = element.TagCode.Substring(0, element.TagCode.Length - 1).Trim();
                    }

                    // Check if the tag is on tags ignored list.
                    // For example <!DOCTYPE html>
                    bool isIgnored = TagUtils.IsTagIgnored(element.TagName);

                    if (!isIgnored) {
                        // If the tag is a comment
                        if (element.TagName.StartsWith("!")) {
                            element.Type = TagType.Comment;

                            int startIndex = element.TagStartIndex + 2;
                            int endIndex = -1;
                            int hyphensCount = GetHyphensCount(source, startIndex);

                            endIndex = hyphensCount >= 2
                                ? Utils.SearchForClosestString(source, "-->", i)
                                : Utils.SearchForClosestChar(source, '>', i);

                            if (endIndex == -1) endIndex = source.Length;

                            element.Content = source.Substring(startIndex + hyphensCount, endIndex - startIndex - hyphensCount).Trim();
                            element.Content = "<!-- " + element.Content + " -->";

                            i += endIndex - startIndex;
                        } else {
                            // If the tag is opening or self-closing, then get attributes
                            if (element.Type == TagType.Opening || element.Type == TagType.SelfClosing) {
                                element.Attributes = DOMAttributes.Get(element);
                            }

                            if (TagUtils.IsTagSelfClosing(element)) {
                                element.Type = TagType.SelfClosing;
                            }

                            // Get a text between two tags 
                            if (tags.Count > 0 && !ignoreNewElements) {
                                DOMElement last = tags.GetLast();

                                int startIndex = tags.GetLast().TagEndIndex + 1;
                                int endIndex = element.TagStartIndex - last.TagEndIndex - 1;

                                if (last.Type == TagType.SelfClosing) last = tags[tags.Count - 2];

                                if (endIndex > 0) {
                                    string content = source.Substring(startIndex, endIndex).Trim();

                                    if (content.Length > 0) {
                                        DOMElement text = new DOMElement() {
                                            Type = TagType.Text,
                                            Content = content
                                        };

                                        tags.Add(text);
                                    }
                                }
                            }

                            if (!ignoreNewElements) tags.Add(element);

                            string lowerCaseTagName = element.TagName.ToLower();

                            if (element.Type == TagType.Opening && (lowerCaseTagName == "script" || lowerCaseTagName == "style")) {
                                string searchedTagName = lowerCaseTagName == "script" ? "script" : "style";
                                int searchedTagIndex = TagUtils.GetClosestClosingTagIndex(source, element.TagEndIndex + 1, searchedTagName);

                                element.Content = source.Substring(element.TagEndIndex + 1, searchedTagIndex - element.TagEndIndex - 1);

                                i = element.TagEndIndex + element.Content.Length;
                            }
                        }
                    }
                }
            }

            return tags;
        }

        /// <summary>
        /// Gets hyphens count (-)
        /// </summary>
        private static int GetHyphensCount(string str, int startIndex) {
            int count = 0;

            for (int i = startIndex; i < str.Length; i++) {
                if (str[i] == '-') count++;
                else return count;
            }

            return 0;
        }
    }
}
