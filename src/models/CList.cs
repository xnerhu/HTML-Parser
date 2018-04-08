using System.Collections.Generic;

namespace HTMLParser {
    public class CList<T> : List<T> {
        public T GetLast () {
            return this.Count > 0 ? this[this.Count - 1] : default(T);
        }
    }
}
