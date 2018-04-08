namespace HTMLParser {
    public class DOMTree {
        /// <summary>
        /// Creates DOM tree from tags list
        /// </summary>
        public static CList<DOMElement> Get(CList<DOMElement> tagsList, string source) {
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
                    }
                    // Auto closing, ignoring not opened tags
                    else if (openingTag.Count >= 0) {
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
    }
}
