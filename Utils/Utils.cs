using System;
using System.IO;
using System.Net;

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

        public static void Log (string description, string value, ConsoleColor valueColor = ConsoleColor.Green, ConsoleColor descriptionColor = ConsoleColor.Cyan) {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = descriptionColor;
            Console.Write("\n" + description + ": ");
            Console.ForegroundColor = valueColor;
            Console.Write(value);

            Console.ForegroundColor = defaultColor;
        }

        public static string GetValidURL (string url) {
            bool isCorrect = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isCorrect) {
                if (!url.StartsWith("www")) url = "www." + url;
                url = "http://" + url;
            } 

            return url;
        }

        public static string GetWebSiteContent (string url) {
            url = GetValidURL(url);

            string content = "";

            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    content = reader.ReadToEnd();
                }
            } catch (WebException exception) {
                Console.WriteLine(exception.Message);
            }

            return content;
        }
    }
}
