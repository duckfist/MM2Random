using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Utilities
{
    public static class MiscHacks
    {
        public static void DrawTitleScreenChanges(Patch p, int seed)
        {
            // Draw version number
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string version = assembly.GetName().Version.ToString();

            for (int i = 0; i < version.Length; i++)
            {
                byte value = TitleChars.GetChar(version[i]).ID;
                p.Add(0x0373C7 + i, value, String.Format("Title Screen Version Number"));
            }

            // Draw seed
            string seedAlpha = SeedConvert.ConvertBase10To26(seed);
            for (int i = 0; i < seedAlpha.Length; i++)
            {
                char ch = seedAlpha.ElementAt(i);
                byte charIndex = (byte)(Convert.ToByte(ch) - Convert.ToByte('A'));

                // 'A' starts at C1 in the pattern table
                p.Add(0x037387 + i, (byte)(0xC1 + charIndex), String.Format("Title Screen Seed"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="jVersion"></param>
        public static void SetWily5NoMusicChange(Patch p, bool jVersion)
        {
            p.Add(0x0383DA, 0xEA, String.Format("Disable Music on Boss Defeat 1"));
            p.Add(0x0383DB, 0xEA, String.Format("Disable Music on Boss Defeat 2"));
            p.Add(0x0383DC, 0xEA, String.Format("Disable Music on Boss Defeat 3"));
            p.Add(0x03848A, 0xEA, String.Format("Disable Music on Boss Defeat 4"));
            p.Add(0x03848B, 0xEA, String.Format("Disable Music on Boss Defeat 5"));
            p.Add(0x03848C, 0xEA, String.Format("Disable Music on Boss Defeat 6"));
            p.Add(0x02E070, 0xEA, String.Format("Disable Music on Boss Defeat 7"));
            p.Add(0x02E071, 0xEA, String.Format("Disable Music on Boss Defeat 8"));
            p.Add(0x02E072, 0xEA, String.Format("Disable Music on Boss Defeat 9"));
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetFastText(Patch p, bool jVersion)
        {
            int address = (jVersion) ? 0x037C51 : 0x037D4A;
            p.Add(address, 0x04, String.Format("Weapon Get Text Write Delay"));
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetBurstChaser(Patch p, bool jVersion)
        {
            p.Add(0x038147, 0x60, String.Format("READY Text Delay"));
            p.Add(0x038921, 0x03, String.Format("Mega Man Walk X-Velocity Integer"));
            p.Add(0x03892C, 0x00, String.Format("Mega Man Walk X-Velocity Fraction"));
            p.Add(0x038922, 0x03, String.Format("Mega Man Air X-Velocity Integer"));
            p.Add(0x03892D, 0x00, String.Format("Mega Man Air X-Velocity Fraction"));
            p.Add(0x0386EF, 0x01, String.Format("Mega Man Ladder Climb Up Integer"));
            p.Add(0x03872E, 0xFE, String.Format("Mega Man Ladder Climb Down Integer"));

            int address = (jVersion) ? 0x03D4A4 : 0x03D4A7;
            p.Add(address, 0x08, String.Format("Buster Projectile X-Velocity Integer"));
        }

        /// <summary>
        /// Enabling Random Weapons or Random Stages will cause the wrong Robot Master portrait to
        /// be blacked out when a stage is completed. The game uses your acquired weapons to determine
        /// which portrait to black-out. This function changes the lookup table for x and y positions
        /// of portraits to black-out based on what was randomized.
        /// </summary>
        public static void FixPortraits(Patch Patch, bool is8StagesRandom, RStages randomStages, bool isWeaponGetRandom, RWeaponGet randomWeaponGet)
        {
            // Arrays of default values for X and Y of the black square that marks out each portrait
            // Index of arrays are stage order, e.g. Heat, Air, etc.
            byte[] portraitBG_y = new byte[] { 0x21, 0x20, 0x21, 0x20, 0x20, 0x22, 0x22, 0x22 };
            byte[] portraitBG_x = new byte[] { 0x86, 0x8E, 0x96, 0x86, 0x96, 0x8E, 0x86, 0x96 };

            // Adjusting the sprites is not necessary because the hacked portrait graphics ("?" images)
            // only use the background, and the sprites have been blacked out. Left in for reference.
            //byte[] portraitSprite_x = new byte[] { 0x3C, 0x0C, 0x4C, 0x00, 0x20, 0x84, 0x74, 0xA4 };
            //byte[] portraitSprite_y = new byte[] { 0x10, 0x14, 0x28, 0x0C, 0x1C, 0x20, 0x10, 0x18 };

            // Apply changes to portrait arrays based on shuffled stages
            if (is8StagesRandom)
            {
                randomStages.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            // Apply changes to portrait arrays based on shuffled weapons. Only need a standard "if" with no "else",
            // because the arrays must be permuted twice if both randomization settings are enabled.
            if (isWeaponGetRandom)
            {
                randomWeaponGet.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            for (int i = 0; i < 8; i++)
            {
                Patch.Add(0x034541 + i, portraitBG_y[i], String.Format("Stage Select Portrait {0} Y-Pos Fix", i + 1));
                Patch.Add(0x034549 + i, portraitBG_x[i], String.Format("Stage Select Portrait {0} X-Pos Fix", i + 1));
                // Changing this sprite table misplaces their positions by default.
                //stream.Position = 0x03460D + i;
                //stream.WriteByte(portraitSprite_y[i]);
                //stream.Position = 0x034615 + i;
                //stream.WriteByte(portraitSprite_x[i]);
            }
        }

        public static void EnablePressDamage(Patch Patch)
        {
            Patch.Add(EDmgVsEnemy.DamageP + EDmgVsEnemy.Offset.Press, 0x01, "Buster Damage Against Press");
        }
    }
}
