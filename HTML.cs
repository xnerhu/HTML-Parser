using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HTMLParser {
    public static class HTML {
        /// <summary>
        /// Parses source code
        /// </summary>
        public static CList<DOMElement> Parse(string source, ref Statistics stats, bool textInsideOneLine = false) {
            source = source.Trim();
            if (textInsideOneLine) source = source.Replace(System.Environment.NewLine, "");

            Stopwatch watch = Stopwatch.StartNew();

            // Parse source code to tags list
            CList<DOMElement> tagsList = GetTagsList(source);

            // Get time
            watch.Stop();
            stats.SourceCodeParsingTime = (int)watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();

            // Parse tags list to DOM tree
            CList<DOMElement> tree = GetDOMTree(tagsList, source);

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
                                element.Attributes = GetAttributes(element);
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
        /// Creates DOM tree from tags list
        /// </summary>
        public static CList<DOMElement> GetDOMTree(CList<DOMElement> tagsList, string source) {
            CList<DOMElement> tree = new CList<DOMElement>();
            CList<DOMElement> parentsList = new CList<DOMElement>();
            CList<OpeningTag> openingTagsList = new CList<OpeningTag>();

            for (int i = 0; i < tagsList.Count; i++) {
                DOMElement element = tagsList[i];
                DOMElement lastParent = parentsList.GetLast();
                OpeningTag openingTag = null;

                if (element.Type == TagType.Closing || element.Type == TagType.Opening) {
                    int index = TagUtils.GetOpeningTagIndex(openingTagsList, element.TagName);

                    if (index == -1) {
                        openingTagsList.Add(new OpeningTag() {
                            Count = 0,
                            TagName = element.TagName
                        });

                        openingTag = openingTagsList.GetLast();
                    } else {
                        openingTag = openingTagsList[index];
                    }

                    if (element.Type == TagType.Opening) openingTag.Count++;
                    else openingTag.Count--;
                }

                // Add tag that isn't any tag's child
                // For example <html> is first tag in a document so it isn't any tag's child
                if (parentsList.Count == 0 || (element.Type == TagType.Closing && parentsList.Count == 1)) {
                    tree.Add(element);

                    // Add tag as a parent
                    // For example <html>
                    if (element.Type == TagType.Opening) {
                        parentsList.Add(element);
                    }
                }
                // For every closing tag
                // remove last parent from parentsList
                // add closing tag as DOMElement to tree
                else if (element.Type == TagType.Closing) {
                    int parentOpeningTagIndex = TagUtils.GetOpeningTagIndex(openingTagsList, lastParent.TagName);
                    OpeningTag parentOpeningTag = openingTagsList[parentOpeningTagIndex];

                    if (element.TagName == lastParent.TagName) {
                        parentsList.Remove(lastParent);
                        parentsList.GetLast().Children.Add(element);
                    } else if (openingTag.Count >= 0) {
                        DOMElement closingTag = new DOMElement() {
                            Type = TagType.Closing,
                            TagCode = "/" + lastParent.TagName,
                            TagName = lastParent.TagName,
                            HelperText = "auto-closed"
                        };

                        parentsList.Remove(lastParent);
                        parentsList.GetLast().Children.Add(closingTag);

                        openingTag.Count--;
                        parentOpeningTag.Count--;
                        i--;
                    }
                }
                // For every opening tag
                // add it as last parent's child
                // and select it as a new parent
                else if (element.Type == TagType.Opening) {
                    // Add the child to last parent
                    lastParent.Children.Add(element);
                    // Add new parent
                    parentsList.Add(element);
                } else {
                    lastParent.Children.Add(element);
                }
            }

            return tree;
        }

        /// <summary>
        /// Parses tag's code to attributes
        /// </summary>
        public static CList<DOMElementAttribute> GetAttributes(DOMElement element) {
            string tagCode = element.TagCode.Substring(element.TagName.Length, element.TagCode.Length - element.TagName.Length).Trim();

            CList<DOMElementAttribute> attributesList = new CList<DOMElementAttribute>();

            if (tagCode.Length > 0) {
                bool searchForAttribute = true;
                bool searchForValue = false;
                int openedQuotes = 0;

                char quoteChar = '"';

                for (int i = 0; i < tagCode.Length; i++) {
                    DOMElementAttribute lastAttribute = attributesList.GetLast();

                    if (tagCode[i] != ' ') {
                        if (searchForAttribute) {
                            DOMElementAttribute attribute = new DOMElementAttribute {
                                // Set attribute's property start index as the loop index
                                PropertyStartIndex = i
                            };

                            // Get closest equals char index for getting attribute property
                            int closestEqualsChar = Utils.SearchForClosestChar(tagCode, '=', i);
                            // If there isn't any equals char then use tag's content length as index
                            if (closestEqualsChar == -1) closestEqualsChar = tagCode.Length;
                            // Get content betweeen tag's property start index and closest equals char
                            string content = tagCode.Substring(i, closestEqualsChar - i);
                            // Check if in content is any space
                            int spaceIndexInContent = Utils.SearchForClosestChar(content, ' ', 0);
                            // If attribute has value, there isn't any spaces in content
                            bool hasValue = spaceIndexInContent == -1;

                            // Get attribute's property end index
                            attribute.PropertyEndIndex = spaceIndexInContent == -1 ? closestEqualsChar : (spaceIndexInContent + attribute.PropertyStartIndex);
                            // Get property by cutting
                            attribute.Property = tagCode.Substring(attribute.PropertyStartIndex, attribute.PropertyEndIndex - attribute.PropertyStartIndex);
                            // Add attribute to the list
                            attributesList.Add(attribute);

                            // If attribute has value, then its searching for value
                            if (hasValue) searchForValue = true;
                            else i = attribute.PropertyEndIndex;

                            // Search for another attribute
                            searchForAttribute = !hasValue;
                        }
                        // Search for value
                        // Count opened quotes (" and ')
                        else if (searchForValue) {
                            // If quote char is an apostrophe
                            if (openedQuotes == 0 && tagCode[i] == '\'') quoteChar = '\'';

                            if (tagCode[i] == quoteChar) {
                                // Open value
                                if (openedQuotes == 0) {
                                    lastAttribute.ValueStartIndex = i + 1;
                                    openedQuotes++;
                                }
                                // Close value
                                else if (openedQuotes == 1) {
                                    // Get attribute's value
                                    lastAttribute.ValueEndIndex = i;
                                    lastAttribute.Value = tagCode.Substring(lastAttribute.ValueStartIndex, lastAttribute.ValueEndIndex - lastAttribute.ValueStartIndex);

                                    // Reset parser
                                    openedQuotes = 0;
                                    searchForValue = false;
                                    searchForAttribute = true;
                                    quoteChar = '"';
                                }
                            }
                        }
                    }
                }
            }

            return attributesList;
        }

        private static int GetHyphensCount(string str, int startIndex) {
            int count = 0;

            for (int i = startIndex; i < str.Length; i++) {
                if (str[i] == '-') count++;
                else break;
            }

            return count;
        }
    }
}
