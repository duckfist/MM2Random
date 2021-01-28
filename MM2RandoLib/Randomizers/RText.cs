using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Utilities;

namespace MM2Randomizer.Randomizers
{
    public class RText : IRandomizer
    {
        public static readonly int MAX_CHARS = 12;
        public static readonly int INTRO_LINE1_MAXCHARS = 19;
        public static readonly int INTRO_LINE2_MAXCHARS = 31;
        public static readonly int INTRO_LINE3_MAXCHARS = 11;
        public static readonly int INTRO_LINE4_MAXCHARS = 25;

        public static readonly int offsetWpnGetLetters   = 0x037E22;
        public static readonly int offsetAtomicFire      = 0x037E2E;
        public static readonly int offsetCutscenePage1L1 = 0x036D56;
        public static readonly int offsetIntroLine1      = 0x036EA8;
        public static readonly int offsetIntroLine2      = 0x036EBE;
        public static readonly int offsetIntroLine3      = 0x036EE0;
        public static readonly int offsetIntroLine4      = 0x036EEE;

        private readonly List<string> countryNames = new List<string>();
        private readonly List<string> companyNames = new List<string>();
        private readonly string[] newWeaponNames = new string[8];
        private readonly char[] newWeaponLetters = new char[9]; // Original order: P H A W B Q F M C

        public RText() { }

        public void Randomize(Patch p, Random r)
        {
            IntroStorySet introStorySet = Properties.Resources.IntroStoryConfig.Deserialize<IntroStorySet>();
            IEnumerable<IntroStory> introStories = introStorySet.Where(x => true == x.Enabled);
            IntroStory introStory = introStories.ElementAt(r.Next(introStories.Count()));

            // Write in splash screen intro text
            //Intro Screen Line 1: 0x036EA8 - 0x036EBA(20 chars)
            //Intro Screen Line 2: 0x036EBE - 0x036EDC(31 chars)
            //Intro Screen Line 3: 0x036EE0 - 0x036EEA(11 chars)
            //Intro Screen Line 4: 0x036EEE - 0x036F06(25 chars)
            //
            //       ©1988 CAPCOM CO.LTD
            // TM AND ©1989 CAPCOM U.S.A.,INC.
            //   MEGA MAN 2 RANDOMIZER 0.3.2
            //           LICENSED BY
            //    NINTENDO OF AMERICA. INC.

            // Line 1: ©2017 <company name> (13 chars for company, 19 total)
            string[] lines;
            int startChar;
            string companyStr;
            char[] company;

            CompanyNameSet companyNameSet = Properties.Resources.CompanyNameConfig.Deserialize<CompanyNameSet>();

            foreach (CompanyName companyName in companyNameSet)
            {
                if (true == companyName.Enabled)
                {
                    companyNames.Add(companyName.Name);
                }
            }

            companyStr = companyNames[r.Next(companyNames.Count)];
            company = ($"©{DateTime.Now.Year} {companyStr}").ToCharArray();
            Char[] companyPadded = Enumerable.Repeat(' ', INTRO_LINE1_MAXCHARS).ToArray();
            startChar = (INTRO_LINE1_MAXCHARS - company.Length) / 2;


            for (int i = 0; i < company.Length; i++)
            {
                companyPadded[startChar + i] = company[i];
            }

            for (int i = 0; i < INTRO_LINE1_MAXCHARS; i++)
            {
                byte charByte = IntroCipher[companyPadded[i]];
                p.Add(
                    offsetIntroLine1 + i,
                    charByte,
                    $"Splash Text: {companyPadded[i]}");
            }

            // Line 2: Version
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string version = assembly.GetName().Version.ToString().Substring(0, 5);
            char[] line2 = $"  MEGA MAN 2 RANDOMIZER {version}  ".ToCharArray();
            for (int i = 0; i < INTRO_LINE2_MAXCHARS; i++)
            {
                byte charByte = IntroCipher[line2[i]];
                p.Add(
                    offsetIntroLine2 + i,
                    charByte,
                    $"Splash Text: {line2[i]}");
            }

            // Line 3: FOR USE IN
            char[] forUseIn = "FOR USE IN ".ToCharArray();
            for (int i = 0; i < INTRO_LINE3_MAXCHARS; i++)
            {
                byte charByte = IntroCipher[forUseIn[i]];
                p.Add(
                    offsetIntroLine3 + i,
                    charByte,
                    $"Splash Text: {forUseIn[i]}");
            }

            // Line 4: <Country>
            CountryNameSet countryNameSet = Properties.Resources.CountryNameConfig.Deserialize<CountryNameSet>();

            foreach (CountryName countryName in countryNameSet)
            {
                if (true == countryName.Enabled)
                {
                    countryNames.Add(countryName.Name);
                }
            }

            char[] country = countryNames[r.Next(countryNames.Count)].ToCharArray();
            char[] countryPadded = Enumerable.Repeat(' ', INTRO_LINE4_MAXCHARS).ToArray();
            startChar = (INTRO_LINE4_MAXCHARS - country.Length) / 2;
            for (int i = 0; i < country.Length; i++)
            {
                countryPadded[startChar + i] = country[i];
            }
            for (int i = 0; i < INTRO_LINE4_MAXCHARS; i++)
            {
                byte charByte = IntroCipher[countryPadded[i]];
                p.Add(
                    offsetIntroLine4 + i,
                    charByte,
                    $"Splash Text: {countryPadded[i]}");
            }

            // Write in cutscene intro text
            Int32 introTextIndex = 0;
            foreach (Byte character in introStory.GetFormattedString())
            {
                p.Add(offsetCutscenePage1L1 + introTextIndex++, character, $"Intro Text: {character}");
            }

            // Write in new weapon names
            for (int i = 0; i < 8; i++)
            {
                int offset = offsetAtomicFire + i * 0x10;

                string name = GetRandomName(r);
                newWeaponNames[i] = name;
                char[] chars = name.ToCharArray();

                for (int j = 0; j < MAX_CHARS; j++)
                {
                    if (j < chars.Length)
                    {
                        byte b = Convert.ToByte(chars[j]);
                        p.Add(offset + j, b, String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)i).Name, j, chars[j].ToString()));

                    }
                    else
                    {
                        p.Add(offset + j, Convert.ToByte('@'), $"Weapon Name {((EDmgVsBoss.Offset)i).Name} Char #{j}: @");
                    }
                }
            }

            // Erase "Boomerang" for now
            for (int i = 0; i < 10; i++)
            {
                p.Add(0x037f5e + i, Convert.ToByte('@'), $"Quick Boomerang Name Erase Char #{i}: @");
            }

            // Create new weapon letters
            {
                // Keep local copy of the alphabet to remove letters from
                List<char> alphabet = new List<char>(Alphabet.ToCharArray());

                // First pick a letter for buster, 1/26
                int rLetterIndex = r.Next(alphabet.Count);
                newWeaponLetters[0] = alphabet[rLetterIndex];
                alphabet.RemoveAt(rLetterIndex);

                // For each special weapon...
                for (int i = 0; i < 8; i++)
                {
                    // Try to use the first letter of the weapon name if it hasn't been used yet
                    char tryLetter = newWeaponNames[i][0];
                    if (alphabet.Contains(tryLetter))
                    {
                        newWeaponLetters[i + 1] = tryLetter;
                        alphabet.Remove(tryLetter);
                    }
                    // Otherwise use a random letter from the remaining letters
                    else
                    {
                        rLetterIndex = r.Next(alphabet.Count);
                        newWeaponLetters[i + 1] = alphabet[rLetterIndex];
                        alphabet.RemoveAt(rLetterIndex);
                    }
                }
            }

            // Write in new weapon letters
            for (int i = 0; i < 9; i++)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                int newLetter = 0x41 + Alphabet.IndexOf(newWeaponLetters[i]); // unicode
                p.Add(offsetWpnGetLetters + i, (byte)newLetter, $"Weapon Get {((EDmgVsBoss.Offset)i).Name} Letter: {newWeaponLetters[i]}");

                // Write to pause menu
                int[] pauseLetterBytes = PauseScreenCipher[newWeaponLetters[i]];
                int wpnLetterAddress = PauseScreenWpnAddressByBossIndex[i];
                for (int j = 0; j < pauseLetterBytes.Length; j++)
                {
                    p.Add(wpnLetterAddress + j, (byte)pauseLetterBytes[j], $"Pause menu weapon letter GFX for \'{newWeaponLetters[i]}\', byte #{j}");
                }
            }

            // Credits: Text content and line lengths (Starting with "Special Thanks")
            CreditTextSet creditTextSet = Properties.Resources.CreditTextConfig.Deserialize<CreditTextSet>();

            StringBuilder creditsSb = new StringBuilder();

            Int32 k = 0;
            foreach (CreditText creditText in creditTextSet)
            {
                if (true == creditText.Enabled)
                {
                    p.Add(0x024C78 + k, (Byte)creditText.Text.Length, $"Credits Line {k} Length");
                    Byte value = Convert.ToByte(creditText.Value, 16);
                    p.Add(0x024C3C + k, value, $"Credits Line {k} X-Pos");

                    k++;

                    // Content of line of text
                    creditsSb.Append(creditText.Text);
                }
            }

            startChar = 0x024D36; // First byte of credits text
            for (int i = 0; i < creditsSb.Length; i++)
            {
                p.Add(startChar, CreditsCipher[creditsSb[i]], $"Credits char #{i}");
                startChar++;
            }

            // Last line "Capcom Co.Ltd."
            for (int i = 0; i < companyStr.Length; i++)
            {
                p.Add(startChar, CreditsCipher[companyStr[i]], $"Credits company char #{i}");
                startChar++;
            }
            p.Add(0x024CA4, (byte)companyStr.Length, "Credits Company Line Length");

            int[] txtRobos = new int[8] {
                0x024D6B, // Heat
                0x024D83, // Air
                0x024D9C, // Wood
                0x024DB7, // Bubble
                0x024DD1, // Quick
                0x024DEB, // Flash
                0x024E05, // Metal
                0x024E1F, // Clash
            };

            int[] txtWilys = new int[6] {
                0x024E54, // Dragon
                0x024E6C, // Picopico
                0x024E80, // Guts
                0x024E97, // Boobeam
                0x024EAE, // Machine
                0x024EC3, // Alien
            };

            // Write Robot Master damage table
            StringBuilder sb;
            for (int i = 0; i < txtRobos.Length; i++)
            {
                sb = new StringBuilder();

                // Since weaknesses are for the "Room", and the room bosses were shuffled,
                // obtain the weakness for the boss at this room
                // TODO: Optimize this mess; when the bossroom is shuffled it should save
                // a mapping that could be reused here.
                int newIndex = 0;
                for (int m = 0; m < RandomMM2.randomBossInBossRoom.Components.Count; m++)
                {
                    var room = RandomMM2.randomBossInBossRoom.Components[m];
                    if (room.OriginalBossIndex == i)
                    {
                        newIndex = m;
                        break;
                    }
                }

                for (int j = 0; j < 9; j++)
                {
                    int dmg = RWeaknesses.BotWeaknesses[newIndex, j];
                    sb.Append($"{GetBossWeaknessDamageChar(dmg)} ");
                }

                string rowString = sb.ToString().Trim();
                for (int j = 0; j < rowString.Length; j++)
                {
                    p.Add(txtRobos[i] + j,
                        CreditsCipher[rowString[j]],
                        $"Credits robo weakness table char #{j + i * rowString.Length}");
                }
            }

            // Write Wily Boss damage table
            for (int i = 0; i < txtWilys.Length; i++)
            {
                sb = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    int dmg = RWeaknesses.WilyWeaknesses[i, j];
                    sb.Append($"{GetBossWeaknessDamageChar(dmg)} ");
                }

                sb.Remove(sb.Length - 1, 1);
                string rowString = sb.ToString();

                for (int j = 0; j < rowString.Length; j++)
                {
                    p.Add(txtWilys[i] + j,
                        CreditsCipher[rowString[j]],
                        $"Credits wily weakness table char #{j + i * rowString.Length}");
                }
            }
        }

        public void FixWeaponLetters(Patch p, int[] permutation)
        {
            // Re-order the letters array to match the ordering of the shuffled weapons
            char[] newLettersPermutation = new char[9];
            newLettersPermutation[0] = newWeaponLetters[0];
            for (int i = 0; i < 8; i++)
            {
                newLettersPermutation[i + 1] = newWeaponLetters[permutation[i] + 1];
            }

            // Write new weapon letters to weapon get screen
            for (int i = 1; i < 9; i++)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                int newLetter = 0x41 + Alphabet.IndexOf(newLettersPermutation[i]); // unicode
                p.Add(offsetWpnGetLetters + i - 1, (byte)newLetter, $"Weapon Get {((EDmgVsBoss.Offset)i).Name} Letter: {newWeaponLetters[i]}");
            }

            //// Write new weapon letters to pause menu
            //for (int i = 0; i < 9; i++)
            //{
                //int[] pauseLetterBytes = PauseScreenCipher[newWeaponLetters[i + 1]];
                //int wpnLetterAddress = PauseScreenWpnAddressByBossIndex[permutedIndex + 1];
                //for (int j = 0; j < pauseLetterBytes.Length; j++)
                //{
                //    p.Add(wpnLetterAddress + j, (byte)pauseLetterBytes[j], $"Pause menu weapon letter GFX for \'{newWeaponLetters[permutedIndex]}\', byte #{j}");
                //}
            //}
        }

        static char GetBossWeaknessDamageChar(int dmg)
        {
            char c;
            if (dmg == 0 || dmg == 255)
            {
                c = ' ';
            }
            else if (dmg < 10)
            {
                c = dmg.ToString()[0];
            }
            else if (dmg >= 10 && dmg < 20)
            {
                c = 'A';
            }
            else
            {
                c = 'B';
            }
            return c;
        }

        public static string Alphabet { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// These arrays represent the raw graphical data for each letter, A-Z, to be rendered on the pause menu. To 
        /// replace a weapon letter's graphic on the pause screen, use <see cref="PauseScreenWpnAddressByBossIndex"/>
        /// to get the address of the desired weapon letter to replace, and then write in the 16 bytes for the desired
        /// new letter from this dictionary.
        /// </summary>
        public static Dictionary<char, int[]> PauseScreenCipher = new Dictionary<char, int[]>()
        {
            // Kept as int, to prevent a thousand (byte) casts clogging this up. Cast to byte in a loop later.
            { 'A', new int[] { 0x02, 0x39, 0x21, 0x21, 0x01, 0x39, 0x21, 0xE7, 0x7C, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0x00 } },
            { 'B', new int[] { 0x02, 0x39, 0x21, 0x02, 0x39, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xFC, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'C', new int[] { 0x00, 0x38, 0x26, 0x20, 0x20, 0x20, 0x82, 0x7C, 0x7C, 0xC6, 0xC0, 0xC0, 0xC0, 0xC6, 0x7C, 0x00 } },
            { 'D', new int[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'E', new int[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x00, 0xFE, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xFE, 0x00 } },
            { 'F', new int[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x20, 0xE0, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xC0, 0x00 } },
            { 'G', new int[] { 0x02, 0x39, 0x27, 0x21, 0x39, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0xDE, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'H', new int[] { 0x21, 0x21, 0x21, 0x01, 0x39, 0x21, 0x21, 0xE7, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'I', new int[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x02, 0xFE, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0xFC, 0x00 } },
            { 'J', new int[] { 0x02, 0x02, 0x02, 0x02, 0x12, 0x12, 0x46, 0x3C, 0x0C, 0x0C, 0x0C, 0x0C, 0x6C, 0x6C, 0x38, 0x00 } },
            { 'K', new int[] { 0x22, 0x26, 0x0C, 0x18, 0x08, 0x24, 0x32, 0xEE, 0xCC, 0xD8, 0xF0, 0xE0, 0xF0, 0xD8, 0xCC, 0x00 } },
            { 'L', new int[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0xFE, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xFC, 0x00 } },
            { 'M', new int[] { 0x01, 0x01, 0x01, 0x29, 0x31, 0x21, 0x21, 0xE7, 0xC6, 0xEE, 0xFE, 0xD6, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'N', new int[] { 0x21, 0x01, 0x01, 0x21, 0x31, 0x29, 0x21, 0xE7, 0xC6, 0xE6, 0xF6, 0xDE, 0xCE, 0xC6, 0xC6, 0x00 } },
            { 'O', new int[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'P', new int[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x3C, 0x20, 0xE0, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xC0, 0xC0, 0x00 } },
            { 'Q', new int[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x31, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xDE, 0xCE, 0x7C, 0x00 } },
            { 'R', new int[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x32, 0x09, 0xC7, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xCC, 0xC6, 0x00 } },
            { 'S', new int[] { 0x02, 0x39, 0x27, 0x82, 0x79, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0x7C, 0x06, 0xC6, 0x7C, 0x00 } },
            { 'T', new int[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x00 } },
            { 'U', new int[] { 0x21, 0x21, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'V', new int[] { 0x21, 0x21, 0x21, 0x21, 0x93, 0x46, 0x2C, 0x18, 0xC6, 0xC6, 0xC6, 0xC6, 0x6C, 0x38, 0x10, 0x00 } },
            { 'W', new int[] { 0x21, 0x21, 0x21, 0x21, 0x01, 0x11, 0x21, 0xC7, 0xC6, 0xC6, 0xC6, 0xD6, 0xFE, 0xEE, 0xC6, 0x00 } },
            { 'X', new int[] { 0x22, 0x22, 0x84, 0x48, 0x00, 0x32, 0x22, 0xCE, 0xCC, 0xCC, 0x78, 0x30, 0x78, 0xCC, 0xCC, 0x00 } },
            { 'Y', new int[] { 0x22, 0x22, 0x22, 0x84, 0x48, 0x08, 0x08, 0x38, 0xCC, 0xCC, 0xCC, 0x78, 0x30, 0x30, 0x30, 0x00 } },
            { 'Z', new int[] { 0x02, 0x72, 0x84, 0x08, 0x10, 0x22, 0x02, 0xFE, 0xFC, 0x8C, 0x18, 0x30, 0x60, 0xC4, 0xFC, 0x00 } },
        };

        /// <summary>
        /// These ROM addresses point to the graphical data of the sprites in the pause menu, namely the weapon 
        /// letters. Use the data at <see cref="PauseScreenCipher"/> to write new values at these locations to
        /// change the weapon letter graphics.
        /// </summary>
        public static int[] PauseScreenWpnAddressByBossIndex = new int[]
        {
            0x001B00, // "P"
            0x001A00, // "H"
            0x0019C0, // "A"
            0x0019A0, // "W"
            0x0019E0, // "B"
            0x0019D0, // "Q"
            0x0019B0, // "F"
            0x0019F0, // "M"
            0x001A10, // "C"
        };

        public static Dictionary<char, byte> CreditsCipher = new Dictionary<char, byte>()
        {
            { ' ', 0x00},
            { 'A', 0x01},
            { 'B', 0x02},
            { 'C', 0x03},
            { 'D', 0x04},
            { 'E', 0x05}, 
            { 'F', 0x06},
            { 'G', 0x07},
            { 'H', 0x08},
            { 'I', 0x09}, 
            { 'J', 0x0A}, 
            { 'K', 0x0B}, 
            { 'L', 0x0C}, 
            { 'M', 0x0D}, 
            { 'N', 0x0E},
            { 'O', 0x0F},
            { 'P', 0x10},
            { 'Q', 0x11},
            { 'R', 0x12},
            { 'S', 0x13},
            { 'T', 0x14},
            { 'U', 0x15},
            { 'V', 0x16},
            { 'W', 0x17},
            { 'X', 0x18},
            { 'Y', 0x19},
            { 'Z', 0x1A},
            { '.', 0x1C},
            { ',', 0x1D},
            {'\'', 0x1E},
            { '!', 0x1F},
            { 'f', 0x20},
            { '0', 0x30},
            { '1', 0x31},
            { '2', 0x32},
            { '3', 0x33},
            { '4', 0x34},
            { '5', 0x35},
            { '6', 0x36},
            { '7', 0x37},
            { '8', 0x38}, 
            { '9', 0x39},
            { '=', 0x23},
        };

        public static Dictionary<char, byte> IntroCipher = new Dictionary<char, byte>()
        {
            { ' ', 0x00 },
            { '0', 0xA0 },
            { '8', 0xA1 },
            { '2', 0xA2 },
            { '©', 0xA3 },
            { 't', 0xA4 }, // tm
            { '9', 0xA5 },
            { '7', 0xA6 },
            { '1', 0xA7 },
            { '3', 0xA8 }, // check
            { '4', 0xA9 }, // check
            { '5', 0xAA }, // check
            { '6', 0xAB }, // check
            { '|', 0xC0 }, // Blank space, change later? also, there may be a blank row (B) to use...
            { 'A', 0xC1 },
            { 'B', 0xC2 },
            { 'C', 0xC3 },
            { 'D', 0xC4 },
            { 'E', 0xC5 },
            { 'F', 0xC6 },
            { 'G', 0xC7 },
            { 'H', 0xC8 },
            { 'I', 0xC9 },
            { 'J', 0xCA },
            { 'K', 0xCB },
            { 'L', 0xCC },
            { 'M', 0xCD },
            { 'N', 0xCE },
            { 'O', 0xCF },
            { 'P', 0xD0 },
            { 'Q', 0xD1 },
            { 'R', 0xD2 },
            { 'S', 0xD3 },
            { 'T', 0xD4 },
            { 'U', 0xD5 },
            { 'V', 0xD6 },
            { 'W', 0xD7 },
            { 'X', 0xD8 },
            { 'Y', 0xD9 },
            { 'Z', 0xDA },
            { '?', 0xDB }, // "r." change to ?
            { '.', 0xDC },
            { ',', 0xDD },
            { '\'', 0xDE },
            { '!', 0xDF },
        };
        // STAFF == D3 D4 C1 C6 C6

        private string GetRandomName(Random r)
        {
            // Start with random list
            int l = r.Next(1);
            string name0, name1;
            if (l == 0)
            {
                // Get random name from first list
                int random = r.Next(Names0.Length);
                name0 = Names0[random];

                // From second list, get subset of names with valid character count
                int charsLeft = MAX_CHARS - name0.Length - 1; // 1 space
                List<string> names1Left = new List<string>();
                for (int j = 0; j < Names1.Length; j++)
                {
                    if (Names1[j].Length <= charsLeft)
                    {
                        names1Left.Add(Names1[j]);
                    }
                }

                // Get random name from modified second list
                if (names1Left.Count > 0)
                {
                    random = r.Next(names1Left.Count);
                    name1 = names1Left[random];
                }
                else
                {
                    name1 = "";
                }
            }
            else
            {
                // Get random name from second list
                int random = r.Next(Names1.Length);
                name1 = Names1[random];

                // From first list, get subset of names with valid character count
                int charsLeft = MAX_CHARS - name1.Length - 1; // 1 space
                List<string> names0Left = new List<string>();
                for (int j = 0; j < Names0.Length; j++)
                {
                    if (Names0[j].Length <= charsLeft)
                    {
                        names0Left.Add(Names0[j]);
                    }
                }

                // Get random name from modified first list
                if (names0Left.Count > 0)
                {
                    random = r.Next(names0Left.Count);
                    name0 = names0Left[random];
                }
                else
                {
                    name0 = "";
                }
            }

            // Handle cases for only one name
            string finalName;
            if (name0.Length == 0)
            {
                finalName = name1;
            }
            else if (name1.Length == 0)
            {
                finalName = name0;
            }
            // Concatenate final name
            else
            {
                finalName = name0 + "@" + name1;
            }

            return finalName;
        }

        static string[] Names0 = new string[]
            {
                "TIME",
                "MEGA",
                "SUPER",
                "METAL",
                "ATOMIC",
                "AIR",
                "WILY",
                "ROLL",
                "RUSH",
                "FRANKERZ",
                "WATER",
                "GUTS",
                "ELEC",
                "GEMINI",
                "COSSACK",
                "TOAD",
                "HYPER",
                "TIME",
                "CRASH",
                "LEAF",
                "QUICK",
                "DRILL",
                "FLAME",
                "PLANT",
                "KNIGHT",
                "SILVER",
                "JUNK",
                "THUNDER",
                "WILD",
                "NOISE",
                "SLASH",
                "TORNADO",
                "ASTRO",
                "CLOWN",
                "SOLAR",
                "CHILL",
                "TRIPLE",
                "REBOUND",
                "NUDUA",
                "JOKA",
                "ELLO",
                "COOLKID",
                "CYGHFER",
                "ZODA",
                "SHOKA",
                "PROTO",
                "PLUG",
                "RTA",
                "MASH",
                "TURBO",
                "TAS",
                "BIG",
                "CUT",
                "URN",
                "TWITCH",
                "PRO",
                "ION",
                "AUTO",
                "BEAT",
                "LAG",
            };

        static string[] Names1 = new string[]
        {
            "BLAST",
            "BLASTER",
            "FIRE",
            "CUTTER",
            "BLADE",
            "STOPPER",
            "GUN",
            "CANNON",
            "HIT",
            "SHOT",
            "COIL",
            "SHOOTER",
            "BOMB",
            "BOMBER",
            "LASER",
            "FLUSH",
            "BEAM",
            "DASH",
            "MISSILE",
            "STORM",
            "CUTTER",
            "SHIELD",
            "KNUCKLE",
            "SNAKE",
            "SHOCK",
            "SPIN",
            "CRUSHER",
            "HOLD",
            "EYE",
            "ATTACK",
            "KICK",
            "STONE",
            "WAVE",
            "SPEAR",
            "CLAW",
            "BALL",
            "TRIDENT",
            "WOOL",
            "SPIKE",
            "BLAZE",
            "STRIKER",
            "WALL",
            "BALLOON",
            "MARINE",
            "WIRE",
            "BURNER",
            "BUSTER",
            "ZIP",
            "GLITCH",
            "ADAPTER",
            "RAID",
            "DEVICE",
            "BOX",
            "AXE",
            "ARC",
            "JAB",
            "RESET",
            "STRAT",
        };
    }
}
