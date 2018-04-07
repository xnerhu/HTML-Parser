using System;
using System.Text;

namespace HTMLParser {
    class Program {
        private static HTMLDocument Document;
        private static string MainHTMLContent;

        private static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            MainHTMLContent = FileManager.ReadFile(Paths.MainHTMLDocument);

            // Get a DOM tree
            // To parse a website instead of html file in assets folder use
            // new HTMLDocument("url")
            Document = new HTMLDocument(MainHTMLContent);

            // Write the DOM tree and the statistics
            DOMPrinter.WriteDOMTree(Document.DOMTree);
            WriteStatistics();

            Console.ReadLine();
        }

        private static void WriteStatistics() {
            Utils.Log("\n\nTime of parsing source code to tags list",
                Document.Stats.SourceCodeParsingTime + "ms");
            Utils.Log("Time of parsing to a DOM tree",
                Document.Stats.DOMTreeParsingTime + "ms");

            if (Document.IsDownloaded) {
                Utils.Log("Downloading time",
                    Document.Stats.DownloadingTime + "ms");
            }
        }
    }
}
