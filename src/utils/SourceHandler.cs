using System;
using System.IO;
using System.Net;
using System.Text;

namespace HTMLParser {
    public static class SourceHandler {
        public const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)" +
            " AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

        /// <summary>
        /// Gets website content
        /// </summary>
        public static string Request(Uri uri, string userAgent = DefaultUserAgent) {
            // If path leads to a file
            if (uri.IsFile) {
                try {
                    return File.ReadAllText(uri.AbsolutePath, Encoding.UTF8);
                } catch (IOException e) {
                    Console.WriteLine(e.Message);
                }
            } else {
                try {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.UserAgent = userAgent;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream)) {
                        return reader.ReadToEnd();
                    }
                } catch (WebException e) {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

    }
}
