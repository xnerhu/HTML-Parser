using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class HTMLx {
        public static List<DOMElement> Parse(string source) {
            source = Utils.ClearBreakingCharacters(source);

            List<DOMElement> tree = GetDOMTree(GetTagsList(source));

            return tree;
        }

        /// <summary>
        /// Parses source code into objects
        /// </summary>
        public static List<DOMElement> GetTagsList(string source) {
            List<DOMElement> tags = new List<DOMElement>();

            bool isReadingTagCode = false;
            bool isReadingText = false;
            int textStartIndex = -1;

            string tagNameStoppingBreak = null;

            for (int i = 0; i < source.Length; i++) {
                char character = source[i];

                // Check if the text reading is finished
                if ((character == '<' || i == source.Length - 1) && isReadingText) {
                    isReadingText = false;

                    // Cut to get the text
                    string content = source.Substring(textStartIndex, i - textStartIndex);

                    if (content.Length > 0) {
                        DOMElement text = new DOMElement() {
                            Type = TagType.Text,
                            Content = content
                        };

                        tags[tags.Count - 1].Children.Add(text);
                    }
                }

                if (character == '<') {
                    // Get tag's end index (>)
                    int tagEndIndex = Utils.SearchForClosestChar(source, '>', i + 1);
                    // Get tag's code
                    string tagCode = TagUtils.GetCode(source, i, tagEndIndex);
                    // Get tag's name
                    string tagName = TagUtils.GetName(tagCode);
                    // Get tag's type
                    TagType tagType = TagUtils.GetType(tagCode);
                    // Create DOMElement
                    DOMElement element = new DOMElement() {
                        TagCode = tagCode,
                        TagName = tagName,
                        Type = tagType,
                        TagStartIndex = i,
                        TagEndIndex = tagEndIndex
                    };
                    // Add it to the list
                    if (tagNameStoppingBreak == null) {
                        tags.Add(element);
                        isReadingTagCode = true;
                    } else if (tagNameStoppingBreak == element.TagName && element.Type == TagType.Closing) {
                        tagNameStoppingBreak = null;
                        isReadingText = false;
                        Console.WriteLine("XDDD" + element.TagCode);
                    }

                    if (element.TagName == "script") {
                        if (element.Type == TagType.Opening) {
                            tagNameStoppingBreak = element.TagName;
                            isReadingText = true;
                            isReadingTagCode = false;
                        }
                    }
                } else if (!isReadingTagCode && !isReadingText && character != ' ') {
                    textStartIndex = i;
                    isReadingText = true;
                } else if (character == '>') {
                    isReadingTagCode = false;
                }
            }

            return tags;
        }

        public static List<DOMElement> GetDOMTree(List<DOMElement> tagsList) {
            List<DOMElement> tree = new List<DOMElement>();
            List<DOMElement> parentsList = new List<DOMElement>();

            for (int i = 0; i < tagsList.Count; i++) {
                DOMElement element = tagsList[i];

                // Add tag that isn't any tag's child
                // For example <html> is first tag in a document so it isn't any tag's child
                if (parentsList.Count == 0 || (element.Type == TagType.Closing && parentsList.Count == 1)) {
                    tree.Add(element);
                    // Add tag as a parent
                    // For example <html>
                    if (element.Type == TagType.Opening) {
                        parentsList.Add(element);
                    }
                } else {
                    // For every opening tag
                    // add it as latest parent's child
                    // and select it as a new parent
                    if (element.Type == TagType.Opening) {
                        // Add the child to latest parent
                        parentsList[parentsList.Count - 1].Children.Add(element);
                        // Add new parent
                        parentsList.Add(element);
                    }
                    // For every closing tag
                    // remove latest parent from parentsList
                    // add closing tag as DOMElement to tree
                    else if (element.Type == TagType.Closing) {
                        // Remove latest parent
                        parentsList.Remove(parentsList[parentsList.Count - 1]);
                        // If there is any parent left
                        if (parentsList.Count > 0) {
                            parentsList[parentsList.Count - 1].Children.Add(element);
                        }
                    }
                }
            }

            // Test output
            Console.WriteLine();
            Utils.WriteDOMTree(tree);

            return tree;
        }
    }
}
