using System.Collections.Generic;
using System.Diagnostics;

namespace HTMLParser {
    public class HTMLDocument {
        public List<DOMElement> DOMTree = new List<DOMElement>();
        public List<DOMElement> MetaTags = new List<DOMElement>();

        public Statistics Stats = new Statistics();

        public bool IsDownloaded = false;

        /// <param name="url">Source code or url</param>
        public HTMLDocument (string url, bool isURL = true, bool textInsideOneLine = false) {
            string source = isURL ? GetSourceCode(url) : url;

            IsDownloaded = isURL;

            this.DOMTree = HTML.Parse(source, ref Stats, textInsideOneLine);
            this.MetaTags = GetElementsByName("meta");           
        }

        private string GetSourceCode(string url) {
            Stopwatch watch = Stopwatch.StartNew();

            string html = Utils.GetWebSiteContent(url);

            // Get time
            watch.Stop();
            Stats.DownloadingTime = (int)watch.ElapsedMilliseconds;

            return html;
        }

        public List<DOMElement> GetElementsByName (string name) {
            List<DOMElement> list = new List<DOMElement>();

            for (int i = 0; i < this.DOMTree.Count; i++) {
                DOMElement element = this.DOMTree[i];

                if (element.TagName == name) list.Add(element);
            }

            return list;
        }
    }
}
