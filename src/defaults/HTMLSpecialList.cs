using System.Collections.Generic;

namespace HTMLParser {
    public static class HTMLSpecialList {
        public static List<string> selfClosingTags = new List<string>() {
            "area",
            "img",
            "base",
            "br",
            "col",
            "input",
            "embed",
            "hr",
            "meta",
            "param",
            "source",
            "track",
            "link"
        };

        public static List<string> ignoredTags = new List<string>() {
            "?xml",
            "!doctype"
        };
    }
}
