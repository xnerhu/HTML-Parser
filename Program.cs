using System;
using System.IO;
using System.Text;

namespace HTMLParser {
    class Program {
        static string AssetsPath = AppDomain.CurrentDomain.BaseDirectory + "assets/";
        static string IndexPath = AssetsPath + "index.html";

        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            string path = IndexPath;
            string sourceCode = SourceHandler.Request(new Uri(path));

            HTMLDocument document = new HTMLDocument(sourceCode);

            Printer.Print(document.Children);
            Printer.PrintDetails(document);

            Console.ReadLine();
        }
    }
}
