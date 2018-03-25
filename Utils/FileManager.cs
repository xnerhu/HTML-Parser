using System;
using System.IO;
using System.Text;

namespace HTMLParser {
    public static class FileManager {
        public static string ReadFile(string path) {
            return File.ReadAllText(path, Encoding.UTF8);
        }
    }
}