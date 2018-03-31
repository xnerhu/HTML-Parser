using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Utils {
        public static int SearchForClosestChar(string str, char endingChar, int startIndex) {
            for (int i = startIndex; i < str.Length; i++) {
                char character = str[i];

                if (character == endingChar) {
                    return i;
                }
            }

            return -1;
        }

        public static string ClearBreakingCharacters(string str) {
            return str.Replace(System.Environment.NewLine, "").Trim();
        }

        public static void Write(List<DOMElement> tags) {
            foreach (DOMElement element in tags) {
                Console.WriteLine("<" + element.TagCode + ">");

                if (element.Children.Count > 0 && element.Children[0].Type == TagType.Text) {
                    Console.WriteLine(element.Children[0].Content);
                }
            }
        }

        public static void WriteAttributes(List<DOMElementAttribute> attributesList) {
            foreach (DOMElementAttribute attribute in attributesList) {
                Utils.Log(attribute.Property + ": ", attribute.Value + "\n", ConsoleColor.Cyan, ConsoleColor.DarkCyan);
            }
        }

        public static void Log (string description, string value, ConsoleColor valueColor = ConsoleColor.Green, ConsoleColor descriptionColor = ConsoleColor.Cyan) {
            Console.ForegroundColor = descriptionColor;
            Console.Write(description);
            Console.ForegroundColor = valueColor;
            Console.Write(value);
        }

        public static List<int> GetIndexes(string parent, string str) {
            List<int> indexes = new List<int>();
            int i = -1;

            while ((i = parent.IndexOf(str, i + 1)) != -1) {
                indexes.Add(i);
            }

            return indexes;
        }

        public static void WriteDOMTree(List<DOMElement> elements, int level = 0, DOMElement parent = null) {
            for (int i = 0; i < elements.Count; i++) {
                string gap = "";
                for (int l = 0; l < level; l++) gap += "  ";

                DOMElement element = elements[i];

                if (element.Type == TagType.Text) {
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine(gap + element.Content);
                } else {
                    ConsoleColor tagColor = ConsoleColor.Green;
                    ConsoleColor attributeColor = ConsoleColor.Cyan;
                    ConsoleColor attributeValueColor = ConsoleColor.DarkCyan;

                    Console.ForegroundColor = tagColor;
                    Console.Write(gap + "<" + (element.Type == TagType.Closing ? "/" : "") + element.TagName);
                    
                    if (element.Type ==  TagType.Opening || element.Type == TagType.SelfClosing) {
                        Console.ForegroundColor = attributeColor;

                        for (int a = 0; a < element.Attributes.Count; a++) {
                            DOMElementAttribute attribute = element.Attributes[a];

                            Console.Write(" " + attribute.Property);

                            if (attribute.Value.Length != 0) {
                                Console.Write('=');
                                Console.ForegroundColor = attributeValueColor;
                                Console.Write('"' + attribute.Value + '"');
                                Console.ForegroundColor = attributeColor;
                            }
                        }
                    }

                    Console.ForegroundColor = tagColor;
                    Console.Write(">");
                    Console.WriteLine();
                }


                if (elements[i].Children.Count > 0) {
                    WriteDOMTree(elements[i].Children, level + 1, elements[i]);
                }
            }
        }
    }
}
