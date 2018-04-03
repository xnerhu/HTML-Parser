using System;

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

        public static int GetTotalMiliseconds (DateTime dateTime) {
            return (int)(DateTime.Now - dateTime).TotalMilliseconds;
        }
    }
}
