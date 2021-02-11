using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Utilities
{
    public static class SeedConvert
    {
        public static String ConvertBase10To26(Int32 n)
        {
            String hexavigesimal = IntToString(n,
            Enumerable.Range('A', 26).Select(x => (Char)x).ToArray());
            return hexavigesimal;
        }

        public static Int32 ConvertBase26To10(String n)
        {
            Int32 base10 = StringToInt(n,
                Enumerable.Range('A', 26).Select(x => (Char)x).ToArray());
            return base10;
        }

        public static String IntToString(Int32 value, Char[] baseChars)
        {
            String result = String.Empty;
            Int32 targetBase = baseChars.Length;

            do
            {
                result = baseChars[value % targetBase] + result;
                value = value / targetBase;
            }
            while (value > 0);

            return result;
        }

        public static Int32 StringToInt(String value, Char[] baseChars)
        {
            Int32 result = 0;
            Int32 targetBase = baseChars.Length;
            List<Char> baseCharsList = new List<Char>(baseChars);
            Char[] valueChars = value.ToCharArray();

            for (Int32 i = 0; i < valueChars.Length; i++)
            {
                // Starting from the right of the String, get the index of the character
                Int32 index = baseCharsList.IndexOf(valueChars[valueChars.Length - 1 - i]);

                // Add the product of each digit with its place value
                result = result + index * (Int32)Math.Pow(targetBase, i);
            }
            return result;
        }
    }
}
