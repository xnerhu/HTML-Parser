using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class DOMPrinter {
        public static void WriteElements(List<DOMElement> elements) {
            for (int i = 0; i < elements.Count; i++) {
                DOMElement element = elements[i];

                Console.WriteLine("<" + element.TagCode + ">");

                if (element.Children.Count > 0 && element.Children[0].Type == TagType.Text) {
                    Console.WriteLine(element.Children[0].Content);
                }
            }
        }

        public static void WriteDOMTree(List<DOMElement> elements, int level = 0, DOMElement parent = null) {
            for (int i = 0; i < elements.Count; i++) {
                DOMElement element = elements[i];

                ConsoleColor tagColor = ConsoleColor.Green;
                ConsoleColor textColor = ConsoleColor.White;

                Console.Write("\n" + GetWhiteSpaces(level));

                if (element.Type != TagType.Text) {
                    if (element.TagName == "script") tagColor = ConsoleColor.DarkYellow;
                    else if (element.TagName == "style") tagColor = ConsoleColor.DarkBlue;

                    Console.ForegroundColor = tagColor;

                    // Write tag name
                    Console.Write("<" + element.TagName);

                    // Write attributes
                    if (element.Attributes.Count > 0) {
                        WriteAttributes(element.Attributes, true, true);
                    }

                    Console.Write((element.Type == TagType.Closing ? " /" : "") + ">");

                    // Write children
                    if (elements[i].Children.Count > 0) {
                        WriteDOMTree(elements[i].Children, level + 1, elements[i]);
                    }
                } else {
                    if (parent != null) {
                        if (parent.TagName == "script") textColor = ConsoleColor.Yellow;
                        else if (parent.TagName == "style") textColor = ConsoleColor.Blue;
                    }

                    Console.ForegroundColor = textColor;
                    Console.Write(element.Content);
                }
            }
        }

        public static void WriteAttributes(List<DOMElementAttribute> list,
            bool writeInLine = false,
            bool firstItemSpace = false,
            ConsoleColor attrColor = ConsoleColor.Cyan,
            ConsoleColor valueColor = ConsoleColor.DarkCyan
          ) {
            for (int i = 0; i < list.Count; i++) {
                DOMElementAttribute attribute = list[i];

                if (!writeInLine) Console.WriteLine();
                ConsoleColor defaultColor = Console.ForegroundColor;

                // Write property
                Console.ForegroundColor = attrColor;
                Console.Write(((i > 0 && writeInLine) || firstItemSpace ? " " : "") + attribute.Property);

                // Write value
                if (attribute.Value.Length != 0) {
                    Console.Write('=');
                    Console.ForegroundColor = valueColor;
                    Console.Write('"' + attribute.Value + '"');
                }

                Console.ForegroundColor = defaultColor;
            }
        }

        private static string GetWhiteSpaces (int count) {
            string str = "";
            for (int i = 0; i < count; i++) str += "  ";
            return str;
        }
    }
}
