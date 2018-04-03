﻿using System;

namespace HTMLParser {
    public static class Paths {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string Assets = BaseDirectory + @"Assets\";
        public static string MainHTMLDocument = Assets + "index.html";
    }
}
