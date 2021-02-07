using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RText : IRandomizer
    {
        //
        // Constructor
        //

        public RText()
        {
        }


        //
        // IRandomizer Methods
        //

        public void Randomize(Patch in_Patch, Random in_Random)
        {
            CompanyNameSet companyNameSet = Properties.Resources.CompanyNameConfig.Deserialize<CompanyNameSet>();
            IEnumerable<CompanyName> enabledCompanyNames = companyNameSet.Where(x => true == x.Enabled);
            CompanyName companyName = enabledCompanyNames.ElementAt(in_Random.Next(enabledCompanyNames.Count()));

            // Write the intro text

            //       ©1988 CAPCOM CO.LTD
            // TM AND ©1989 CAPCOM U.S.A.,INC.
            //   MEGA MAN 2 RANDOMIZER 0.3.2
            //           LICENSED BY
            //    NINTENDO OF AMERICA. INC.

            RText.PatchCompanyName(in_Patch, companyName);
            RText.PatchIntroVersion(in_Patch);
            RText.PatchForUse(in_Patch, in_Random);
            RText.PatchIntroStroy(in_Patch, in_Random);

            // Generate Weapon Names
            IEnumerable<WeaponName> weaponNames = WeaponName.GenerateUniqueWeaponNames(in_Random, 8);

            // Write the new weapons names
            RText.PatchWeaponNames(in_Patch, in_Random, weaponNames, out List<Char> newWeaponLetters);

            // This is a hack to get around the strange interdependency that
            // the randomizer interfaces have
            this.mNewWeaponLetters = newWeaponLetters;

            // Write the credits
            RText.PatchCredits(in_Patch, in_Random, companyName);
        }

        //
        // Private Static Methods
        //

        /// <summary>
        /// This method patches the company name in the intro screen.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 1: 0x036EA8 - 0x036EBA (19 chars)
        /// ©2017 <company name> (13 chars for company, 19 total)
        /// </remarks>
        public static void PatchCompanyName(Patch in_Patch, CompanyName in_CompanyName)
        {
            const Int32 MAX_LINE_LENGTH = 19;
            const Int32 INTRO_LINE1_ADDRESS = 0x036EA8;

            String line = $"©{DateTime.Now.Year} {in_CompanyName.GetCompanyName()}".PadCenter(MAX_LINE_LENGTH);

            in_Patch.Add(
                INTRO_LINE1_ADDRESS,
                line.AsIntroString(),
                $"Splash Text: {line}");
        }


        /// <summary>
        /// This method patches the second line in the intro text.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 2: 0x036EBE - 0x036EDC (31 chars)
        /// </remarks>
        public static void PatchIntroVersion(Patch in_Patch)
        {
            const String APP_NAME = "Mega Man 2 Randomizer v";
            const Int32 INTRO_LINE2_OFFSET = 0x036EBE;
            const Int32 INTRO_LINE2_MAXLENGTH = 31;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            Version appVersion = assembly.GetName().Version;
            String version = appVersion.ToString(2);

            String line = APP_NAME + version;
            line = line.PadCenter(INTRO_LINE2_MAXLENGTH);

            in_Patch.Add(INTRO_LINE2_OFFSET, line.AsIntroString(), $"Splash Text: {line}");
        }


        /// <summary>
        /// This method patches the third and fourth lines in the intro text.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 3: 0x036EE0 - 0x036EEA (11 chars)
        /// Intro Screen Line 4: 0x036EEE - 0x036F06 (25 chars)
        /// </remarks>
        public static void PatchForUse(Patch in_Patch, Random in_Random)
        {
            const String INTRO_LINE3_PREFIX = "FOR USE ";
            const Int32 INTRO_LINE3_ADDRESS = 0x036EE0;
            const Int32 INTRO_LINE4_ADDRESS = 0x036EEE;

            CountryNameSet countryNameSet = Properties.Resources.CountryNameConfig.Deserialize<CountryNameSet>();
            IEnumerable<CountryName> countryNames = countryNameSet.Where(x => true == x.Enabled);
            CountryName countryName = countryNames.ElementAt(in_Random.Next(countryNames.Count()));

            Int32 line3NextCharacterAddress = in_Patch.Add(
                INTRO_LINE3_ADDRESS,
                INTRO_LINE3_PREFIX.AsIntroString(),
                $"Splash Text: {INTRO_LINE3_PREFIX}");

            in_Patch.Add(
                line3NextCharacterAddress,
                countryName.GetFormattedPrefix(),
                $"Splash Text: {countryName.Prefix}");

            in_Patch.Add(
                INTRO_LINE4_ADDRESS,
                countryName.GetFormattedName(),
                $"Splash Text: {countryName.Name}");
        }


        /// <summary>
        /// This method patches the intro story text.
        /// </summary>
        /// <remarks>
        /// Intro Story: 0x036D56 - 0x036E64 (270 chars)
        /// 27 characters per line
        /// 10 lines
        /// </remarks>
        public static void PatchIntroStroy(Patch in_Patch, Random in_Random)
        {
            const Int32 INTRO_STORY_PAGE1_ADDRESS = 0x036D56;

            IntroStorySet introStorySet = Properties.Resources.IntroStoryConfig.Deserialize<IntroStorySet>();
            IEnumerable<IntroStory> introStories = introStorySet.Where(x => true == x.Enabled);
            IntroStory introStory = introStories.ElementAt(in_Random.Next(introStories.Count()));

            in_Patch.Add(
                INTRO_STORY_PAGE1_ADDRESS,
                introStory.GetFormattedString(),
                $"Intro Text: {introStory.Title}");
        }


        public static void PatchWeaponNames(Patch in_Patch, Random in_Random, IEnumerable<WeaponName> in_WeaponNames, out List<Char> out_NewWeaponLetters)
        {
            const Int32 WEAPON_GET_LETTERS_ADDRESS = 0x037E22;
            const Int32 WEAPON_GET_NAME_ADDRESS = 0x037E2E;

            if (8 != in_WeaponNames.Count())
            {
                throw new ArgumentException("Incorrect weapon name count", nameof(in_WeaponNames));
            }

            List<WeaponName> weaponNames = in_WeaponNames.ToList();

            // Write in new weapon names
            for (Int32 weaponIndex = 0; weaponIndex < weaponNames.Count; ++weaponIndex)
            {
                // Magic numbers?
                Int32 offsetAddress = WEAPON_GET_NAME_ADDRESS + (weaponIndex * 0x10);
                String weaponName = weaponNames[weaponIndex].Name.PadRight(12, ' ');

                Int32 characterIndex = 0;
                foreach (Char c in weaponName)
                {
                    in_Patch.Add(
                        offsetAddress + characterIndex,
                        c.AsPrintCharacter(),
                        String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)weaponIndex).Name, characterIndex, c.ToString()));

                    characterIndex++;
                }
            }

            // Erase "Boomerang" for now
            // TODO: why?
            for (Int32 i = 0; i < 10; i++)
            {
                in_Patch.Add(0x037f5e + i, ' '.AsPrintCharacter(), $"Quick Boomerang Name Erase Char #{i}: @");
            }

            // Pick a new weapon letter for buster
            Char? busterWeaponLetter = WeaponName.GetUnusedWeaponLetter(in_Random, weaponNames);

            List<Char> weaponLetters = weaponNames.Select(x => x.Letter).ToList();
            weaponLetters.Insert(0, busterWeaponLetter ?? 'P');

            // Write in new weapon letters
            for (Int32 weaponLetterIndex = 0; weaponLetterIndex < weaponLetters.Count; ++weaponLetterIndex)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                Char weaponLetter = weaponLetters[weaponLetterIndex];

                in_Patch.Add(
                    WEAPON_GET_LETTERS_ADDRESS + weaponLetterIndex,
                    weaponLetter.AsPrintCharacter(),
                    $"Weapon Get {((EDmgVsBoss.Offset)weaponLetterIndex).Name} Letter: {weaponLetter}");

                // Write to pause menu
                Byte[] pauseLetterBytes = weaponLetter.AsPauseScreenString();
                Int32 weaponLetterAddress = PauseScreenWpnAddressByBossIndex[weaponLetterIndex];

                in_Patch.Add(
                    weaponLetterAddress,
                    pauseLetterBytes,
                    $"Pause menu weapon letter GFX for \'{weaponLetter}\'");
            }

            out_NewWeaponLetters = weaponLetters;
        }


        public static void PatchCredits(Patch in_Patch, Random in_Random, CompanyName in_CompanyName)
        {
            // Credits: Text content and line lengths (Starting with "Special Thanks")
            CreditTextSet creditTextSet = Properties.Resources.CreditTextConfig.Deserialize<CreditTextSet>();

            StringBuilder creditsSb = new StringBuilder();

            Int32 k = 0;
            foreach (CreditText creditText in creditTextSet)
            {
                if (true == creditText.Enabled)
                {
                    in_Patch.Add(0x024C78 + k, (Byte)creditText.Text.Length, $"Credits Line {k} Length");
                    Byte value = Convert.ToByte(creditText.Value, 16);
                    in_Patch.Add(0x024C3C + k, value, $"Credits Line {k} X-Pos");

                    k++;

                    // Content of line of text
                    creditsSb.Append(creditText.Text);
                }
            }

            Int32 startChar = 0x024D36; // First byte of credits text

            for (Int32 i = 0; i < creditsSb.Length; i++)
            {
                in_Patch.Add(startChar, creditsSb[i].AsCreditsCharacter(), $"Credits char #{i}");
                startChar++;
            }

            // Last line "Capcom Co.Ltd."
            String companyName = in_CompanyName.GetCompanyName();

            for (Int32 i = 0; i < companyName.Length; i++)
            {
                in_Patch.Add(startChar, companyName[i].AsCreditsCharacter(), $"Credits company char #{i}");
                startChar++;
            }

            in_Patch.Add(0x024CA4, (Byte)companyName.Length, "Credits Company Line Length");

            Int32[] txtRobos = new Int32[8]
            {
                0x024D6B, // Heat
                0x024D83, // Air
                0x024D9C, // Wood
                0x024DB7, // Bubble
                0x024DD1, // Quick
                0x024DEB, // Flash
                0x024E05, // Metal
                0x024E1F, // Clash
            };

            int[] txtWilys = new int[6]
            {
                0x024E54, // Dragon
                0x024E6C, // Picopico
                0x024E80, // Guts
                0x024E97, // Boobeam
                0x024EAE, // Machine
                0x024EC3, // Alien
            };

            // Write Robot Master damage table
            StringBuilder sb;
            for (Int32 i = 0; i < txtRobos.Length; i++)
            {
                sb = new StringBuilder();

                // Since weaknesses are for the "Room", and the room bosses were shuffled,
                // obtain the weakness for the boss at this room
                // TODO: Optimize this mess; when the bossroom is shuffled it should save
                // a mapping that could be reused here.
                Int32 newIndex = 0;
                for (Int32 m = 0; m < RandomMM2.randomBossInBossRoom.Components.Count; m++)
                {
                    RBossRoom.BossRoomRandomComponent room = RandomMM2.randomBossInBossRoom.Components[m];

                    if (room.OriginalBossIndex == i)
                    {
                        newIndex = m;
                        break;
                    }
                }

                for (Int32 j = 0; j < 9; j++)
                {
                    Int32 dmg = RWeaknesses.BotWeaknesses[newIndex, j];
                    sb.Append($"{RText.GetBossWeaknessDamageChar(dmg)} ");
                }

                String rowString = sb.ToString().Trim();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtRobos[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits robo weakness table char #{j + i * rowString.Length}");
                }
            }

            // Write Wily Boss damage table
            for (Int32 i = 0; i < txtWilys.Length; i++)
            {
                sb = new StringBuilder();

                for (Int32 j = 0; j < 8; j++)
                {
                    Int32 dmg = RWeaknesses.WilyWeaknesses[i, j];
                    sb.Append($"{RText.GetBossWeaknessDamageChar(dmg)} ");
                }

                sb.Remove(sb.Length - 1, 1);
                String rowString = sb.ToString();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtWilys[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits wily weakness table char #{j + i * rowString.Length}");
                }
            }
        }



        public void FixWeaponLetters(Patch in_Patch, Int32[] in_Permutation)
        {
            const Int32 WEAPON_GET_LETTERS_ADDRESS = 0x037E22;

            // Re-order the letters array to match the ordering of the shuffled weapons
            Char[] newLettersPermutation = new Char[9];
            newLettersPermutation[0] = this.mNewWeaponLetters[0];

            for (Int32 i = 0; i < 8; i++)
            {
                newLettersPermutation[i + 1] = this.mNewWeaponLetters[in_Permutation[i] + 1];
            }

            // Write new weapon letters to weapon get screen
            for (Int32 i = 1; i < 9; i++)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                in_Patch.Add(
                    WEAPON_GET_LETTERS_ADDRESS + i - 1,
                    newLettersPermutation[i].AsPrintCharacter(),
                    $"Weapon Get {((EDmgVsBoss.Offset)i).Name} Letter: {this.mNewWeaponLetters[i]}");
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

        private static Char GetBossWeaknessDamageChar(Int32 dmg)
        {
            Char c;

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


        /// <summary>
        /// These ROM addresses point to the graphical data of the sprites in the pause menu, namely the weapon 
        /// letters. Use the data at <see cref="PauseScreenCipher"/> to write new values at these locations to
        /// change the weapon letter graphics.
        /// </summary>
        public static Int32[] PauseScreenWpnAddressByBossIndex = new Int32[]
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


        //
        // Private Data Members
        //

        List<Char> mNewWeaponLetters = new List<Char>()
        {
            'P',
            'H',
            'A',
            'W',
            'B',
            'Q',
            'F',
            'M',
            'C',
        };
    }
}
