using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HTMLParser {
    class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            string indexPath = "./assets/index.html";
            string sourceCode = File.ReadAllText(indexPath, Encoding.UTF8);

            List<string> tokens = Tokenizer.Tokenize(sourceCode);
            List<Node> tree = DOMBuilder.Build(tokens);

            Printer.PrintTree(tree);

            Console.ReadLine();
        }
    }
}
