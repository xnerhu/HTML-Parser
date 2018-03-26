using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class HTML {
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

            bool addNewElements = true;
            int startParsingIndex = -1;

            for (int i = 0; i < source.Length; i++) {
                char character = source[i];

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

                    // Tags that are disabling parsing for until they are closed
                    if (element.TagName == "script" || element.TagName == "style") {
                        if (element.Type == TagType.Opening && addNewElements) {
                            addNewElements = false;
                            tags.Add(element);
                            
                            startParsingIndex = GetIndexOfLastTag(source, element.TagEndIndex + 1, element.TagName);
                        } else if (element.TagEndIndex == startParsingIndex && !addNewElements) {
                            addNewElements = true;
                            startParsingIndex = -1;
                        }
                    }

                    // Get text between tags
                    if (tags.Count > 0 && addNewElements) {
                        DOMElement latest = tags[tags.Count - 1];

                        int startIndex = latest.TagEndIndex + 1;
                        int endIndex = element.TagStartIndex - latest.TagEndIndex - 1;

                        string content = source.Substring(startIndex, endIndex).Trim();

                        if (content.Length > 0) {
                            DOMElement text = new DOMElement() {
                                Type = TagType.Text,
                                Content = content
                            };

                            latest.Children.Add(text);
                        }
                    }

                    // Add it to the list
                    if (addNewElements) tags.Add(element);
                }
            }

            return tags;
        }

        public static List<DOMElement> GetDOMTree(List<DOMElement> tagsList) {
            List<DOMElement> tree = new List<DOMElement>();
            List<DOMElement> parentsList = new List<DOMElement>();

            int openedTags = 0;
            bool autoClosed = false;

            for (int i = 0; i < tagsList.Count; i++) {
                DOMElement element = tagsList[i];
                DOMElement lastParent = parentsList.Count > 0 ? parentsList[parentsList.Count - 1] : null;

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
                    // add it as last parent's child
                    // and select it as a new parent
                    if (element.Type == TagType.Opening) {
                        // Add the child to last parent
                        lastParent.Children.Add(element);
                        // Add new parent
                        parentsList.Add(element);
                        openedTags++;
                    }
                    // For every closing tag
                    // remove latest parent from parentsList
                    // add closing tag as DOMElement to tree
                    else if (element.Type == TagType.Closing) {
                        if (lastParent.TagName == element.TagName) {
                            openedTags--;
                            // Remove last parent
                            if (!autoClosed) {
                                parentsList.Remove(lastParent);
                            } 
                            // If there is any parent left
                            if (parentsList.Count > 0 && !autoClosed) {
                                parentsList[parentsList.Count - 1].Children.Add(element);
                            }

                            autoClosed = false;
                        } else {
                            lastParent.HelperText = i.ToString();
                            element.HelperText = i.ToString();

                            DOMElement closingTag = new DOMElement() {
                                Type = TagType.Closing,
                                TagCode = "/" + lastParent.TagName,
                                TagName = lastParent.TagName,
                                HelperText = "x"
                            };

                            parentsList[parentsList.Count - 2].Children.Add(closingTag);
                            parentsList.Remove(parentsList[parentsList.Count - 1]);

                            i--;
                            autoClosed = true;
                        }
                    }
                }
            }

            // Test output
            Console.WriteLine();
            Utils.WriteDOMTree(tree);

            Console.ForegroundColor = openedTags > 0 ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen;
            Console.WriteLine("\n\nDocument is " + (openedTags > 0 ? "invalid" : "valid") + " (" + openedTags + ")");

            return tree;
        }

        private static int GetIndexOfLastTag (string source, int startIndex, string tagName) {
            List<int> indexes = new List<int>();

            for (int i = startIndex; i < source.Length; i++) {   
                if (source[i] == '<') {
                    int index = Utils.SearchForClosestChar(source, '>', i);
                    string code = TagUtils.GetCode(source, i, index);
                    string name = TagUtils.GetName(code);                    

                    if (name == tagName && code.StartsWith("/")) {
                        indexes.Add(index);
                    }
                }
            }

            return indexes.Count > 0 ? indexes[indexes.Count - 1] : -1;
        }
    }
}
