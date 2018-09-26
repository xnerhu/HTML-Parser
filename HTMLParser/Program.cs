using System;
using System.Text;

namespace HTMLParser {
    class Program {
        // Useful paths
        static string AssetsPath = AppDomain.CurrentDomain.BaseDirectory + "assets/";
        static string IndexPath = AssetsPath + "index.html";

        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            string path = IndexPath;
            // Get source code
            string sourceCode = SourceHandler.Request(new Uri(path));

            HTMLDocument document = new HTMLDocument(sourceCode);

            // Print out DOM tree
            Printer.Print(document.Children);
            // Print out document details such as links and scripts count
            Printer.PrintDetails(document);

            Console.ReadLine();
        }
    }
}
