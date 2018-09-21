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

            List<Node> tree = new List<Node>() {
                new Node() {
                    nodeName = "html",
                    nodeType = NodeType.ELEMENT_NODE,
                    childNodes = new List<Node> () {
                        new Node() {
                            nodeName = "body",
                            nodeType = NodeType.ELEMENT_NODE,
                            childNodes = new List<Node> () {
                                new Node() {
                                    nodeName = "a",
                                    nodeType = NodeType.ELEMENT_NODE,
                                    childNodes = new List<Node> () {
                                        new Node() {
                                            nodeName = "#text",
                                            nodeType = NodeType.TEXT_NODE,
                                            nodeValue = "A text"
                                        }
                                    }
                                },
                                new Node() {
                                    nodeName = "b",
                                    nodeType = NodeType.ELEMENT_NODE,
                                }
                            }
                        }
                    }
                }
            };

            Printer.PrintTree(tree);

            Console.ReadLine();
        }
    }
}
