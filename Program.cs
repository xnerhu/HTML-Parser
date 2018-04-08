using System;
using System.Text;

namespace HTMLParser {
    class Program {
        private static HTMLDocument Document;
        private static string Content;

        private static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            Content = FileManager.ReadFile(Paths.MainHTMLDocument);

            // Get a DOM tree
            // To parse a website instead of a HTML file in assets folder use
            // new HTMLDocument("url")
            Document = new HTMLDocument(Content);

            DOMElement testElement = Document.GetElementById("test");
            testElement.SetInnerHTML("<form><input type='text' placeHolder='Login'><button id='submit'>A button</button></form>");

            DOMElement submitButtonElement = Document.GetElementById("submit");
            submitButtonElement.SetInnerHTML("<span>Submit</span>");
            
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
