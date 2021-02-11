using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MM2Randomizer.Extensions
{
    public static class StringExtensions
    {
        //
        // Public Methods
        //
        public static T Deserialize<T>(this String in_Resource)
        {
            T returnValue;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(in_Resource);
                    streamWriter.Flush();
                    memoryStream.Position = 0;

                    returnValue = (T)xmlSerializer.Deserialize(memoryStream);

                    streamWriter.Close();
                }
            }

            return returnValue;
        }

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


        public static Byte AsIntroCharacter(this Char in_Char)
        {
            if (true == StringExtensions.IntroCharacterLookup.TryGetValue(in_Char, out Byte convertedCharacter))
            {
                return convertedCharacter;
            }
            else
            {
                return StringExtensions.IntroCharacterLookup['?'];
            }
        }


        public static Byte AsPrintCharacter(this Char in_Char)
        {
            if (true == StringExtensions.PrintCharacterLookup.TryGetValue(in_Char, out Byte retval))
            {
                return retval;
            }
            else
            {
                return StringExtensions.PrintCharacterLookup['?'];
            }
        }

        public static Byte[] AsPauseScreenString(this Char in_Char)
        {
            if (true == StringExtensions.PauseScreenLookup.TryGetValue(in_Char, out Byte[] retval))
            {
                return retval;
            }
            else
            {
                return StringExtensions.PauseScreenLookup['Z'];
            }
        }

        public static Byte[] AsCreditsString(this String in_String)
        {
            if (null == in_String)
            {
                return null;
            }

            Byte[] convertedString = new Byte[in_String.Length];
            Int32 index = 0;

            foreach (Char c in in_String)
            {
                if (true == StringExtensions.CreditsCharacterLookup.TryGetValue(c, out Byte convertedCharacter))
                {
                    convertedString[index] = convertedCharacter;
                }
                else
                {
                    convertedString[index] = StringExtensions.CreditsCharacterLookup['?'];
                }

                index++;
            }

            return convertedString;
        }


        public static Byte AsCreditsCharacter(this Char in_Char)
        {
            if (true == StringExtensions.CreditsCharacterLookup.TryGetValue(in_Char, out Byte convertedCharacter))
            {
                return convertedCharacter;
            }
            else
            {
                return StringExtensions.CreditsCharacterLookup['?'];
            }
        }


        public static String PadCenter(this String in_String, Int32 in_TotalWidth)
        {
            Int32 totalPadding = in_TotalWidth - in_String.Length;

            if (totalPadding > 1)
            {
                Int32 leftPadding = in_String.Length + (totalPadding / 2);

                return in_String.PadLeft(leftPadding).PadRight(in_TotalWidth);
            }
            else
            {
                return in_String.PadRight(in_TotalWidth);
            }
        }


        //
        // Private Constants
        //

        private static readonly Dictionary<Char, Byte> IntroCharacterLookup = new Dictionary<Char, Byte>()
        {
            { ' ',  0x00 },
            { '0',  0xA0 },
            { '8',  0xA1 },
            { '2',  0xA2 },
            { '©',  0xA3 },  // Copyright symbol
            { '™',  0xA4 },  // Trademark symbol
            { '9',  0xA5 },
            { '7',  0xA6 },
            { '1',  0xA7 },
            { '3',  0xA8 },
            { '4',  0xA9 },
            { '5',  0xAA },
            { '6',  0xAB },
            { '|',  0xC0 },  // Blank space character
            { 'a',  0xC1 },
            { 'A',  0xC1 },
            { 'b',  0xC2 },
            { 'B',  0xC2 },
            { 'c',  0xC3 },
            { 'C',  0xC3 },
            { 'd',  0xC4 },
            { 'D',  0xC4 },
            { 'e',  0xC5 },
            { 'E',  0xC5 },
            { 'f',  0xC6 },
            { 'F',  0xC6 },
            { 'g',  0xC7 },
            { 'G',  0xC7 },
            { 'h',  0xC8 },
            { 'H',  0xC8 },
            { 'i',  0xC9 },
            { 'I',  0xC9 },
            { 'j',  0xCA },
            { 'J',  0xCA },
            { 'k',  0xCB },
            { 'K',  0xCB },
            { 'l',  0xCC },
            { 'L',  0xCC },
            { 'm',  0xCD },
            { 'M',  0xCD },
            { 'n',  0xCE },
            { 'N',  0xCE },
            { 'o',  0xCF },
            { 'O',  0xCF },
            { 'p',  0xD0 },
            { 'P',  0xD0 },
            { 'q',  0xD1 },
            { 'Q',  0xD1 },
            { 'r',  0xD2 },
            { 'R',  0xD2 },
            { 's',  0xD3 },
            { 'S',  0xD3 },
            { 't',  0xD4 },
            { 'T',  0xD4 },
            { 'u',  0xD5 },
            { 'U',  0xD5 },
            { 'v',  0xD6 },
            { 'V',  0xD6 },
            { 'w',  0xD7 },
            { 'W',  0xD7 },
            { 'x',  0xD8 },
            { 'X',  0xD8 },
            { 'y',  0xD9 },
            { 'Y',  0xD9 },
            { 'z',  0xDA },
            { 'Z',  0xDA },
            { '?',  0xDB },
            { '.',  0xDC },
            { ',',  0xDD },
            { '\'', 0xDE },
            { '!',  0xDF },
        };


        public static Dictionary<Char, Byte> CreditsCharacterLookup = new Dictionary<Char, Byte>()
        {
            { ' ',  0x00},
            { 'a',  0x01},
            { 'A',  0x01},
            { 'b',  0x02},
            { 'B',  0x02},
            { 'c',  0x03},
            { 'C',  0x03},
            { 'd',  0x04},
            { 'D',  0x04},
            { 'e',  0x05},
            { 'E',  0x05},
            { 'f',  0x06},
            { 'F',  0x06},
            { 'g',  0x07},
            { 'G',  0x07},
            { 'h',  0x08},
            { 'H',  0x08},
            { 'i',  0x09},
            { 'I',  0x09},
            { 'j',  0x0A},
            { 'J',  0x0A},
            { 'k',  0x0B},
            { 'K',  0x0B},
            { 'l',  0x0C},
            { 'L',  0x0C},
            { 'm',  0x0D},
            { 'M',  0x0D},
            { 'n',  0x0E},
            { 'N',  0x0E},
            { 'o',  0x0F},
            { 'O',  0x0F},
            { 'p',  0x10},
            { 'P',  0x10},
            { 'q',  0x11},
            { 'Q',  0x11},
            { 'r',  0x12},
            { 'R',  0x12},
            { 's',  0x13},
            { 'S',  0x13},
            { 't',  0x14},
            { 'T',  0x14},
            { 'u',  0x15},
            { 'U',  0x15},
            { 'v',  0x16},
            { 'V',  0x16},
            { 'w',  0x17},
            { 'W',  0x17},
            { 'x',  0x18},
            { 'X',  0x18},
            { 'y',  0x19},
            { 'Y',  0x19},
            { 'z',  0x1A},
            { 'Z',  0x1A},
            { '.',  0x1C},
            { ',',  0x1D},
            { '\'', 0x1E},
            { '!',  0x1F},
            { '`',  0x20}, // Blank line
            { '0',  0x30},
            { '1',  0x31},
            { '2',  0x32},
            { '3',  0x33},
            { '4',  0x34},
            { '5',  0x35},
            { '6',  0x36},
            { '7',  0x37},
            { '8',  0x38},
            { '9',  0x39},
            { '=',  0x23},
        };

        private static readonly Dictionary<Char, Byte> PrintCharacterLookup = new Dictionary<Char, Byte>()
        {
            { ' ',  0x40 },
            { 'a',  0x41 },
            { 'A',  0x41 },
            { 'b',  0x42 },
            { 'B',  0x42 },
            { 'c',  0x43 },
            { 'C',  0x43 },
            { 'd',  0x44 },
            { 'D',  0x44 },
            { 'e',  0x45 },
            { 'E',  0x45 },
            { 'f',  0x46 },
            { 'F',  0x46 },
            { 'g',  0x47 },
            { 'G',  0x47 },
            { 'h',  0x48 },
            { 'H',  0x48 },
            { 'i',  0x49 },
            { 'I',  0x49 },
            { 'j',  0x4A },
            { 'J',  0x4A },
            { 'k',  0x4B },
            { 'K',  0x4B },
            { 'l',  0x4C },
            { 'L',  0x4C },
            { 'm',  0x4D },
            { 'M',  0x4D },
            { 'n',  0x4E },
            { 'N',  0x4E },
            { 'o',  0x4F },
            { 'O',  0x4F },
            { 'p',  0x50 },
            { 'P',  0x50 },
            { 'q',  0x51 },
            { 'Q',  0x51 },
            { 'r',  0x52 },
            { 'R',  0x52 },
            { 's',  0x53 },
            { 'S',  0x53 },
            { 't',  0x54 },
            { 'T',  0x54 },
            { 'u',  0x55 },
            { 'U',  0x55 },
            { 'v',  0x56 },
            { 'V',  0x56 },
            { 'w',  0x57 },
            { 'W',  0x57 },
            { 'x',  0x58 },
            { 'X',  0x58 },
            { 'y',  0x59 },
            { 'Y',  0x59 },
            { 'z',  0x5A },
            { 'Z',  0x5A },
            { '?',  0x5B },
            { '.',  0x5C },
            { ',',  0x5D },
            { '\'', 0x5E },
            { '!',  0x5F },
            { '0',  0xA0 },
            { '1',  0xA1 },
            { '2',  0xA2 },
            { '3',  0xA3 },
            { '4',  0xA4 },
            { '5',  0xA5 },
            { '6',  0xA6 },
            { '7',  0xA7 },
            { '8',  0xA8 },
            { '9',  0xA9 },
            { '-',  0x94 },

        };

        /// <summary>
        /// These arrays represent the raw graphical data for each letter, A-Z, to be rendered on the pause menu. To 
        /// replace a weapon letter's graphic on the pause screen, use <see cref="PauseScreenWpnAddressByBossIndex"/>
        /// to get the address of the desired weapon letter to replace, and then write in the 16 bytes for the desired
        /// new letter from this dictionary.
        /// </summary>
        public static Dictionary<Char, Byte[]> PauseScreenLookup = new Dictionary<Char, Byte[]>()
        {
            { 'A', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x01, 0x39, 0x21, 0xE7, 0x7C, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0x00 } },
            { 'B', new Byte[] { 0x02, 0x39, 0x21, 0x02, 0x39, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xFC, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'C', new Byte[] { 0x00, 0x38, 0x26, 0x20, 0x20, 0x20, 0x82, 0x7C, 0x7C, 0xC6, 0xC0, 0xC0, 0xC0, 0xC6, 0x7C, 0x00 } },
            { 'D', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'E', new Byte[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x00, 0xFE, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xFE, 0x00 } },
            { 'F', new Byte[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x20, 0xE0, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xC0, 0x00 } },
            { 'G', new Byte[] { 0x02, 0x39, 0x27, 0x21, 0x39, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0xDE, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'H', new Byte[] { 0x21, 0x21, 0x21, 0x01, 0x39, 0x21, 0x21, 0xE7, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'I', new Byte[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x02, 0xFE, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0xFC, 0x00 } },
            { 'J', new Byte[] { 0x02, 0x02, 0x02, 0x02, 0x12, 0x12, 0x46, 0x3C, 0x0C, 0x0C, 0x0C, 0x0C, 0x6C, 0x6C, 0x38, 0x00 } },
            { 'K', new Byte[] { 0x22, 0x26, 0x0C, 0x18, 0x08, 0x24, 0x32, 0xEE, 0xCC, 0xD8, 0xF0, 0xE0, 0xF0, 0xD8, 0xCC, 0x00 } },
            { 'L', new Byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0xFE, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xFC, 0x00 } },
            { 'M', new Byte[] { 0x01, 0x01, 0x01, 0x29, 0x31, 0x21, 0x21, 0xE7, 0xC6, 0xEE, 0xFE, 0xD6, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'N', new Byte[] { 0x21, 0x01, 0x01, 0x21, 0x31, 0x29, 0x21, 0xE7, 0xC6, 0xE6, 0xF6, 0xDE, 0xCE, 0xC6, 0xC6, 0x00 } },
            { 'O', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'P', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x3C, 0x20, 0xE0, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xC0, 0xC0, 0x00 } },
            { 'Q', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x31, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xDE, 0xCE, 0x7C, 0x00 } },
            { 'R', new Byte[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x32, 0x09, 0xC7, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xCC, 0xC6, 0x00 } },
            { 'S', new Byte[] { 0x02, 0x39, 0x27, 0x82, 0x79, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0x7C, 0x06, 0xC6, 0x7C, 0x00 } },
            { 'T', new Byte[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x00 } },
            { 'U', new Byte[] { 0x21, 0x21, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'V', new Byte[] { 0x21, 0x21, 0x21, 0x21, 0x93, 0x46, 0x2C, 0x18, 0xC6, 0xC6, 0xC6, 0xC6, 0x6C, 0x38, 0x10, 0x00 } },
            { 'W', new Byte[] { 0x21, 0x21, 0x21, 0x21, 0x01, 0x11, 0x21, 0xC7, 0xC6, 0xC6, 0xC6, 0xD6, 0xFE, 0xEE, 0xC6, 0x00 } },
            { 'X', new Byte[] { 0x22, 0x22, 0x84, 0x48, 0x00, 0x32, 0x22, 0xCE, 0xCC, 0xCC, 0x78, 0x30, 0x78, 0xCC, 0xCC, 0x00 } },
            { 'Y', new Byte[] { 0x22, 0x22, 0x22, 0x84, 0x48, 0x08, 0x08, 0x38, 0xCC, 0xCC, 0xCC, 0x78, 0x30, 0x30, 0x30, 0x00 } },
            { 'Z', new Byte[] { 0x02, 0x72, 0x84, 0x08, 0x10, 0x22, 0x02, 0xFE, 0xFC, 0x8C, 0x18, 0x30, 0x60, 0xC4, 0xFC, 0x00 } },
        };

    }
}
