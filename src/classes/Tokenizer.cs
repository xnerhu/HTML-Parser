using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Tokenizer {
        /// <summary>
        /// Parses source code to tokens
        /// </summary>
        public static List<string> Tokenize(string source) {
            List<string> list = new List<string>();
            string text = "";
            bool capturingTag = false;
            bool capturingScriptContent = false;

            for (int i = 0; i < source.Length; i++) {
                 if (source[i] == '<') {
                    text = text.Trim();

                    if (!capturingScriptContent) {
                        if (text.Length > 0) list.Add(text);
                        text = "";
                    }

                    capturingTag = true;
                } else if (source[i] == '>' && capturingTag) {
                    list.Add(text + '>');
                    capturingScriptContent = text == "<script";
                    capturingTag = false;
                    text = "";

                    continue;
                }

                text += source[i];
            }

            return list;
        }
    }
}
