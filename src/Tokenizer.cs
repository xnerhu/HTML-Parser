using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Tokenizer {
        public static List<string> Tokenize(string source) {
            List<string> list = new List<string>();

            string text = "";
            bool capturingTag = false;

            for (int i = 0; i < source.Length; i++) {
                 if (source[i] == '<') {
                    if (text.Length > 0) {
                        list.Add(text);
                        text = "";
                    }

                    capturingTag = true;
                } else if (source[i] == '>' && capturingTag) {
                    list.Add(text + '>');
                    text = "";
                    capturingTag = false;
                    continue;
                }

                text += source[i];
            }

            return list;
        }
    }
}
