using System;
using System.Collections.Generic;

namespace MM2Randomizer.Extensions
{
    public static class StringExtensions
    {
        //
        // Public Methods
        //

        public static Byte[] AsIntroString(this String in_String)
        {
            if (null == in_String)
            {
                return null;
            }

            Byte[] convertedString = new Byte[in_String.Length];
            Int32 index = 0;

            foreach (Char c in in_String)
            {
                if (true == StringExtensions.IntroCharacterLookup.TryGetValue(c, out Byte convertedCharacter))
                {
                    convertedString[index] = convertedCharacter;
                }
                else
                {
                    convertedString[index] = StringExtensions.IntroCharacterLookup['?'];
                }

                index++;
            }

            return convertedString;
        }


        //
        // Private Constants
        //

        private static readonly Dictionary<Char, Byte> IntroCharacterLookup = new Dictionary<Char, Byte>()
        {
            { ' ', 0x00 },
            { '0', 0xA0 },
            { '8', 0xA1 },
            { '2', 0xA2 },
            { '©', 0xA3 },  // Copyright symbol
            { '™', 0xA4 },  // Trademark symbol
            { '9', 0xA5 },
            { '7', 0xA6 },
            { '1', 0xA7 },
            { '3', 0xA8 },
            { '4', 0xA9 },
            { '5', 0xAA },
            { '6', 0xAB },
            { '|', 0xC0 },  // Blank space character
            { 'a', 0xC1 },
            { 'A', 0xC1 },
            { 'b', 0xC2 },
            { 'B', 0xC2 },
            { 'c', 0xC3 },
            { 'C', 0xC3 },
            { 'd', 0xC4 },
            { 'D', 0xC4 },
            { 'e', 0xC5 },
            { 'E', 0xC5 },
            { 'f', 0xC6 },
            { 'F', 0xC6 },
            { 'g', 0xC7 },
            { 'G', 0xC7 },
            { 'h', 0xC8 },
            { 'H', 0xC8 },
            { 'i', 0xC9 },
            { 'I', 0xC9 },
            { 'j', 0xCA },
            { 'J', 0xCA },
            { 'k', 0xCB },
            { 'K', 0xCB },
            { 'l', 0xCC },
            { 'L', 0xCC },
            { 'm', 0xCD },
            { 'M', 0xCD },
            { 'n', 0xCE },
            { 'N', 0xCE },
            { 'o', 0xCF },
            { 'O', 0xCF },
            { 'p', 0xD0 },
            { 'P', 0xD0 },
            { 'q', 0xD1 },
            { 'Q', 0xD1 },
            { 'r', 0xD2 },
            { 'R', 0xD2 },
            { 's', 0xD3 },
            { 'S', 0xD3 },
            { 't', 0xD4 },
            { 'T', 0xD4 },
            { 'u', 0xD5 },
            { 'U', 0xD5 },
            { 'v', 0xD6 },
            { 'V', 0xD6 },
            { 'w', 0xD7 },
            { 'W', 0xD7 },
            { 'x', 0xD8 },
            { 'X', 0xD8 },
            { 'y', 0xD9 },
            { 'Y', 0xD9 },
            { 'z', 0xDA },
            { 'Z', 0xDA },
            { '?', 0xDB },
            { '.', 0xDC },
            { ',', 0xDD },
            { '\'', 0xDE },
            { '!', 0xDF },
        };

    }
}
