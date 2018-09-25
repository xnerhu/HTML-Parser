using System;
using System.Collections.Generic;

namespace HTMLParser {
    public static class Tokenizer {
        /// <summary>
        /// Parses source code to tokens
        /// </summary>
        public static List<string> Tokenize(string source) {
            List<string> list = new List<string>();

            bool capturingTag = false;
            bool isScriptTag = false;
            string tagName = "";
            string text = "";

            for (int i = 0; i < source.Length; i++) {
                if (source[i] == '<' && !capturingTag) {
                    capturingTag = true;
                } else if (source[i] == '>' && capturingTag) {
                    if (tagName == "<script") {
                        isScriptTag = true;
                    } else if (tagName == "</script") {
                        isScriptTag = false;
                    }

                    if (!isScriptTag || tagName == "<script") {
                        text = text.Trim();

                        if (text.Length > 0) {
                            list.Add(text);
                            text = "";
                        }

                        list.Add(tagName + '>');
                    } else {
                        text += tagName + '>';
                    }

                    capturingTag = false;
                    tagName = "";
                    continue;
                } else if (i == source.Length - 1) {
                    text = text.Trim();

                    if (text.Length > 0) {
                        list.Add(text + source[i]);
                    }
                }

                if (capturingTag) {
                    tagName += source[i];
                } else {
                    text += source[i];
                }
            }

            return list;
        }
    }
}
