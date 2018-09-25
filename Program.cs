using System;
using System.IO;
using System.Text;

namespace HTMLParser {
    class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            string indexPath = "./assets/index.html";
            string sourceCode = File.ReadAllText(indexPath, Encoding.UTF8);

            HTMLDocument document = new HTMLDocument(sourceCode);

            Printer.Print(document.Children);
            Printer.PrintDetails(document);

            Console.ReadLine();
        }
    }
}
