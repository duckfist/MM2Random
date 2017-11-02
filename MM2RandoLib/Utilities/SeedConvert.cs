using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Utilities
{
    public static class SeedConvert
    {
        public static string ConvertBase10To26(int n)
        {
            string hexavigesimal = IntToString(n,
            Enumerable.Range('A', 26).Select(x => (char)x).ToArray());
            return hexavigesimal;
        }

        public static int ConvertBase26To10(string n)
        {
            int base10 = StringToInt(n,
                Enumerable.Range('A', 26).Select(x => (char)x).ToArray());
            return base10;
        }

        public static string IntToString(int value, char[] baseChars)
        {
            string result = string.Empty;
            int targetBase = baseChars.Length;

            do
            {
                result = baseChars[value % targetBase] + result;
                value = value / targetBase;
            }
            while (value > 0);

            return result;
        }

        public static int StringToInt(string value, char[] baseChars)
        {
            int result = 0;
            int targetBase = baseChars.Length;
            List<char> baseCharsList = new List<char>(baseChars);
            char[] valueChars = value.ToCharArray();

            for (int i = 0; i < valueChars.Length; i++)
            {
                // Starting from the right of the string, get the index of the character
                int index = baseCharsList.IndexOf(valueChars[valueChars.Length - 1 - i]);

                // Add the product of each digit with its place value
                result = result + index * (int)Math.Pow(targetBase, i);
            }
            return result;
        }
    }
}
