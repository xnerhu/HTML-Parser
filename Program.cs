using System;
using System.Text;

namespace HTMLParser {
    class Program {
        private static string HTMLPageContent = "";
        private static HTMLDocument Document;

        private static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            // Get a source code
            HTMLPageContent = FileManager.ReadFile(Paths.MainHTMLDocument);
            // Get a DOM tree
            Document = new HTMLDocument(HTMLPageContent);

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
        }
    }
}
