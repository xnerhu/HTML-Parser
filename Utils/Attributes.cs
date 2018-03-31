using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Attributes {
        /// <summary>
        /// Parses tag's code into attributes
        /// </summary>
        public static List<DOMElementAttribute> Get(DOMElement element) {
            string tagCode = element.TagCode.Substring(element.TagName.Length, element.TagCode.Length - element.TagName.Length).Trim();

            List<DOMElementAttribute> attributesList = new List<DOMElementAttribute>();

            if (tagCode.Length > 0) {
                bool searchForAttribute = true;
                bool searchForValue = false;
                int openedQuotes = 0;

                char quoteChar = '"';

                for (int i = 0; i < tagCode.Length; i++) {
                    DOMElementAttribute lastAttribute = attributesList.Count > 0 ? attributesList[attributesList.Count - 1] : null;

                    if (tagCode[i] != ' ') {
                        if (searchForAttribute) {
                            DOMElementAttribute attribute = new DOMElementAttribute();
                            // Set attribute's property start index as the loop index
                            attribute.PropertyStartIndex = i;

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
                            if (hasValue) {
                                searchForValue = true;
                            } else {
                                // Skip the loop index
                                i = attribute.PropertyEndIndex;
                            }

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
    }
}
