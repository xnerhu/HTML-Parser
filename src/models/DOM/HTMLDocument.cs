using System.Diagnostics;

namespace HTMLParser {
    public class HTMLDocument {
        public CList<DOMElement> DOMTree = new CList<DOMElement>();
        private CList<DOMElement> TagsList = new CList<DOMElement>();
        public CList<DOMElement> MetaTags = new CList<DOMElement>();
        public Statistics Stats = new Statistics();

        public bool IsDownloaded = false;
        public string Source;

        public HTMLDocument(string url, bool isURL = false) {
            if (!isURL && (url.StartsWith("http") || url.StartsWith("www."))) isURL = true;
            Source = isURL ? GetSourceCode(url) : url;

            IsDownloaded = isURL;

            // Parse
            DOMTree = HTML.Parse(Source, ref Stats);

            // Get meta tags
            MetaTags = GetElementsByName("meta");
        }

        #region Utils

        private string GetSourceCode(string url) {
            Stopwatch watch = Stopwatch.StartNew();

            string html = Utils.GetWebSiteContent(url);

            // Get time
            watch.Stop();
            Stats.DownloadingTime = (int)watch.ElapsedMilliseconds;

            return html;
        }

        #endregion

        #region Elements searching

        public DOMElement GetElementById(string id) {
            TagsList = TagUtils.ParseDOMTreeToList(DOMTree);

            for (int i = 0; i < TagsList.Count; i++) {
                DOMElement element = TagsList[i];
                string attributeValue = element.GetAttribute("id");

                //System.Console.WriteLine(element.TagCode);
                if (attributeValue != null && attributeValue == id) {
                    return element;
                }
            }

            return null;
        }

        public CList<DOMElement> GetElementsByName(string tagName) {
            TagsList = TagUtils.ParseDOMTreeToList(DOMTree);

            CList<DOMElement> list = new CList<DOMElement>();

            for (int i = 0; i < TagsList.Count; i++) {
                DOMElement element = TagsList[i];

                if (element.TagName == tagName) list.Add(element);
            }

            return list;
        }

        #endregion
    }
}
