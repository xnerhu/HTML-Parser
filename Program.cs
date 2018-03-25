using System;
using System.Text;

namespace HTMLParser {
    class Program {
        private static string HTMLPageContent = "";
        private static HTMLDocument Document;

        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            // Get source code
            HTMLPageContent = FileManager.ReadFile(Paths.MainHTMLDocument);
            // Get DOM tree
            Document = new HTMLDocument(HTMLPageContent);

            Console.ReadKey();
        }
    }
}
