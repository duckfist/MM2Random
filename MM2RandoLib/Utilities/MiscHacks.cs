using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Stages;
using System;
using System.Linq;

namespace MM2Randomizer.Utilities
{
    public static class MiscHacks
    {
        public static void DrawTitleScreenChanges(Patch p, int seed, RandoSettings settings)
        {
            // Adjust cursor positions
            p.Add(0x0362D4, 0x90, "Title screen Cursor top position"); // default 0x98
            p.Add(0x0362D5, 0xA0, "Title screen Cursor bottom position"); // default 0xA8

            // Draw version number
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(RandomMM2));
            string version = assembly.GetName().Version.ToString();
            for (int i = 0; i < version.Length; i++)
            {
                byte value = RText.IntroCipher[version[i]];
                p.Add(0x037408 + i, value, "Title Screen Version Number");
            }

            // Draw seed
            string seedAlpha = SeedConvert.ConvertBase10To26(seed);
            for (int i = 0; i < seedAlpha.Length; i++)
            {
                byte value = RText.IntroCipher[seedAlpha[i]];
                p.Add(0x0373C8 + i, value, "Title Screen Seed");
            }

            // Draw flags
            string flags = settings.GetFlagsString();
            for (int i = 0; i < flags.Length; i++)
            {
                byte value = RText.IntroCipher[flags[i]];
                p.Add(0x037388 + i, value, $"Title Screen Flags: {flags[i]}");
            }

            // Draw tournament mode/spoiler free information
            if (settings.IsSpoilerFree)
            {
                // 0x037367 = Start of row beneath "seed"
                string flagsAlpha = "TOURNAMENT";
                for (int i = 0; i < flagsAlpha.Length; i++)
                {
                    char ch = flagsAlpha.ElementAt(i);
                    byte charIndex = (byte)(Convert.ToByte(ch) - Convert.ToByte('A'));

                    p.Add(0x037564 + i, (byte)(0xC1 + charIndex), "Title Screen Tournament Text");
                }

                string flags2Alpha = "MODE";
                for (int i = 0; i < flags2Alpha.Length; i++)
                {
                    char ch = flags2Alpha.ElementAt(i);
                    byte charIndex = (byte)(Convert.ToByte(ch) - Convert.ToByte('A'));

                    p.Add(0x03756F + i, (byte)(0xC1 + charIndex), "Title Screen Tournament Text");
                }

                // Draw Hash symbols
                // Use $B8-$BF with custom gfx, previously unused tiles after converting from MM2U to RM2
                //p.Add(0x037367, (byte)(0xB0), "Title Screen Flags");
                //p.Add(0x037368, (byte)(0xB1), "Title Screen Flags");
                //p.Add(0x037369, (byte)(0xB2), "Title Screen Flags");
                //p.Add(0x03736A, (byte)(0xB3), "Title Screen Flags");
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetETankKeep(Patch p)
        {
            p.Add(0x03C1CC, 0xEA, "Disable ETank clear on Game Over 1");
            p.Add(0x03C1CD, 0xEA, "Disable ETank clear on Game Over 2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="jVersion"></param>
        public static void SetWily5NoMusicChange(Patch p)
        {
            p.Add(0x0383DA, 0xEA, "Disable Music on Boss Defeat 1");
            p.Add(0x0383DB, 0xEA, "Disable Music on Boss Defeat 2");
            p.Add(0x0383DC, 0xEA, "Disable Music on Boss Defeat 3");
            p.Add(0x03848A, 0xEA, "Disable Music on Boss Defeat 4");
            p.Add(0x03848B, 0xEA, "Disable Music on Boss Defeat 5");
            p.Add(0x03848C, 0xEA, "Disable Music on Boss Defeat 6");
            p.Add(0x02E070, 0xEA, "Disable Music on Boss Defeat 7");
            p.Add(0x02E071, 0xEA, "Disable Music on Boss Defeat 8");
            p.Add(0x02E072, 0xEA, "Disable Music on Boss Defeat 9");
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetFastWeaponGetText(Patch p)
        {
            //int address = (jVersion) ? 0x037C51 : 0x037D4A;
            int address = 0x037D4A;
            p.Add(address, 0x04, "Weapon Get Text Write Delay");
        }

        /// <summary>
        /// This will speed up the wily map cutscene in between Wily stages by about 2 seconds
        /// </summary>
        /// <param name="p"></param>
        public static void SetFastWilyMap(Patch p)
        {
            // This is the number of frames to wait after drawing the path on the map before fade out.
            // Default value 0x7D (125 frames), change to 0x10.
            p.Add(0x0359B8, 0x10, "Fast Wily Map");
        }

        /// <summary>
        /// This will change the delay defeating a boss and teleporting out of the field to be much shorter.
        /// The victory fanfare will not play, and you teleport out exactly 10 frames after landing the killing
        /// blow on a robot master, and faster for Wily bosses as well. This indirectly fixes the issue of
        /// potentially zipping out of Bubbleman or other robot masters' chambers, since you teleport immediately.
        /// </summary>
        /// <param name="p"></param>
        public static void SetFastBossDefeatTeleport(Patch p)
        {
            // 0x02E0AF: Time until teleport after fanfare starts. ($FD, change to $40)
            // 0x02E0A2: Time until boss-defeat fanfare starts. Note that if set too low without any additional
            //           changes, a softlock may occur after some Wily bosses. Change from $FD to $10, then
            //           modify other areas that set the intial value of $05A7 (address storing our comparison).
            //           It turns out taht Mechadragon, Picopico-kun, and Gutsdozer set the intial value to $70
            //           (at 0x02D16F). Buebeam has its own special routine with extra explosions, setting the 
            //           initial value to $80 (at 0x02D386). Wily Machine and Alien do not call these subroutines
            //           and no further modification is needed.
            // 0x02D170: Wily 1/2/3 boss defeat, time until fanfare starts. ($70, change to $10)
            //           Must be less or equal to value above (at 0x02E0A2)
            // 0x02D386: Buebeam defeat, time until fanfare starts. ($80 change to $10)
            //           Must be less or equal to value above (at 0x02E0A2)
            //
            // The original subroutine that uses 0x02E0A2 is as follows:
            //
            // BossDefeatWaitForTeleport:
            //  0B:A08B: 4E 21 04  LSR $0421    // Not sure what this is for, but it gets zeroed out after a couple loops
            //  0B:A08E: AD A7 05  LDA $05A7    // $05A7 frequently stores a frame-counter or a 'state' value
            //  0B:A091: C9 10     CMP #$FD     // Compare value at $05A7 with 0xFD
            //  0B:A093: B0 04     BCS PlayFanfare_ThenWait  // If value at $05A7 >= 0xFD, jump to PlayFanfare_ThenWait
            //  0B:A095: EE A7 05  INC $05A7    // Increase value at $05A7 by 1
            //  0B:A098: 60        RTS          // Return
            // PlayFanfare_ThenWait:
            //  0B:A099                         // Play fanfare once, then wait to teleport....
            //  ...
            //
            // When defeating Wily 1, 2, 3, or 4, the BossDefeatWaitForTeleport subroutine is entered for the first time
            // with $05A7 having a value of 0x70 or 0x80; if you change the comparison value at $2E0A2 from 0xFD to a
            // value smaller than the intial $05A7, an infinite loop occurs. 

            p.Add(0x02E0AF, 0x40, "Fast Boss Defeat Teleport: Teleport delay after fanfare");
            p.Add(0x02E0A2, 0x10, "Fast Boss Defeat Teleport: Global delay before fanfare");
            p.Add(0x02D170, 0x10, "Fast Boss Defeat Teleport: W1/2/3 boss delay before fanfare");
            p.Add(0x02D386, 0x10, "Fast Boss Defeat Teleport: W4 boss delay before fanfare");

            // Also, NOP out the code that plays the fanfare. It's too distorted sounding when immediately teleporting.
            // Or TODO in the future, change to a different sound?
            //  02E0B3: A9 15      LDA #$15       // Let A = the fanfare sound value (15)
            //  02E0B5: 20 51 C0   JSR PlaySound  // Jump to "PlaySound" function, which plays the value in A
            for (int i = 0; i < 5; i++)
            {
                p.Add(0x02E0B3 + i, 0xEA, "Fast Boss Defeat Teleport: Fanfare sound NOP");
            }
        }

        public static void SetFastReadyText(Patch p)
        {
            p.Add(0x038147, 0x60, "READY Text Delay");
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetBurstChaser(Patch p)
        {
            p.Add(0x038921, 0x03, "Mega Man Walk X-Velocity Integer");
            p.Add(0x03892C, 0x00, "Mega Man Walk X-Velocity Fraction");
            p.Add(0x038922, 0x03, "Mega Man Air X-Velocity Integer");
            p.Add(0x03892D, 0x00, "Mega Man Air X-Velocity Fraction");
            p.Add(0x0386EF, 0x01, "Mega Man Ladder Climb Up Integer");
            p.Add(0x03872E, 0xFE, "Mega Man Ladder Climb Down Integer");

            //int address = (jVersion) ? 0x03D4A4 : 0x03D4A7;
            int address = 0x03D4A7;
            p.Add(address, 0x08, "Buster Projectile X-Velocity Integer");
        }

        /// <summary>
        /// Skip 3 of the 4 extra pages of text that occur when receiving an item, and only show
        /// the last page "GET EQUIPPED WITH ITEM X"
        /// </summary>
        /// <param name="p"></param>
        public static void SkipItemGetPages(Patch p)
        {
            // At 0x037C88, A62ABD81C24A09A08D2004EE20044CD0BC
            p.Add(0x037C88, 0xA6, "Fast Item Get Patch");
            p.Add(0x037C89, 0x2A, "Fast Item Get Patch");
            p.Add(0x037C8A, 0xBD, "Fast Item Get Patch");
            p.Add(0x037C8B, 0x81, "Fast Item Get Patch");
            p.Add(0x037C8C, 0xC2, "Fast Item Get Patch");
            p.Add(0x037C8D, 0x4A, "Fast Item Get Patch");
            p.Add(0x037C8E, 0x09, "Fast Item Get Patch");
            p.Add(0x037C8F, 0xA0, "Fast Item Get Patch");
            p.Add(0x037C90, 0x8D, "Fast Item Get Patch");
            p.Add(0x037C91, 0x20, "Fast Item Get Patch");
            p.Add(0x037C92, 0x04, "Fast Item Get Patch");
            p.Add(0x037C93, 0xEE, "Fast Item Get Patch");
            p.Add(0x037C94, 0x20, "Fast Item Get Patch");
            p.Add(0x037C95, 0x04, "Fast Item Get Patch");
            p.Add(0x037C96, 0x4C, "Fast Item Get Patch");
            p.Add(0x037C97, 0xD0, "Fast Item Get Patch");
            p.Add(0x037C98, 0xBC, "Fast Item Get Patch");
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
                Patch.Add(0x034541 + i, portraitBG_y[i], $"Stage Select Portrait {i + 1} Y-Pos Fix");
                Patch.Add(0x034549 + i, portraitBG_x[i], $"Stage Select Portrait {i + 1} X-Pos Fix");
                // Changing this sprite table misplaces their positions by default.
                //stream.Position = 0x03460D + i;
                //stream.WriteByte(portraitSprite_y[i]);
                //stream.Position = 0x034615 + i;
                //stream.WriteByte(portraitSprite_x[i]);
            }
        }

        /// <summary>
        /// No longer needed since press is included in enemy damage rando table
        /// </summary>
        public static void EnablePressDamage(Patch Patch)
        {
            Patch.Add(EDmgVsEnemy.DamageP + EDmgVsEnemy.Offset.Press, 0x01, "Buster Damage Against Press");
        }

        public static void FixM445PaletteGlitch(Patch p)
        {
            for (int i = 0; i < 3; i++)
                p.Add(0x395BD + i, 0xEA, "M-445 Palette Glitch Fix");
        }

        /// <summary>
        /// Manual tuning of specific enemy damage values on top of vanilla MM2.
        /// </summary>
        /// <param name="p"></param>
        public static void NerfDamageValues(Patch p)
        {
            p.Add(0x3ED6C + 0x61, 0x04, "Woodman's Leaf Shield Attack Nerf");
        }

        public static void DisableChangkeyMakerPaletteSwap(Patch p)
        {
            // Stop palette change when enemy appears
            // $3A4F6 > 0E:A4E6: 20 59 F1 JSR $F159
            // Change to 4C 55 A5 (JMP $A555, which returns immediately) 
            p.Add(0x3A4F6, 0x4C, "Disable Changkey Maker palette swap 1");
            p.Add(0x3A4F7, 0x55, "Disable Changkey Maker palette swap 1");
            p.Add(0x3A4F8, 0xA5, "Disable Changkey Maker palette swap 1");

            // Stop palette change on kill/despawn:
            // $3A562 > 0E:A552: 20 59 F1 JSR $F159
            // Change to EA EA EA (NOP)
            p.Add(0x3A562, 0xEA, "Disable Changkey Maker palette swap 2");
            p.Add(0x3A563, 0xEA, "Disable Changkey Maker palette swap 2");
            p.Add(0x3A564, 0xEA, "Disable Changkey Maker palette swap 2");
        }

        /// <summary>
        /// Prevents E-Tank use when MegaMan has full life.
        /// </summary>
        /// <param name="p">Patch to apply the data to.</param>
        public static void PreventETankUseAtFullLife(Patch p)
        {
            // Original E-Tank Menu Command begins at 0D:9281:
            // $9281: Menu Page and Menu Position Checking.        
            // $9292:A5 A7     LDA $00A7 ;$00A7 is ETankCount
            // $9294:F0 DE     BEQ $9274 ;Return if ETankCount == 0          
            // $9296:C6 A7     DEC $00A7 ;Decrement ETankCount <=== Replacing this line.
            // $9298:AD C0 06  LDA $06C0 ;$06C0 is Life
            // $929B:C9 1C     CMP #$1C
            // $929D:F0 D5     BEQ $9274 ;Return if Life == 28
            // while (Life != 28)
            // {
            //      $929F:A5 1C     LDA $001C ;$001C is a FrameCounter.
            //      $92A1:29 03     AND #$03
            //      $92A3:D0 08     BNE $92AD ;if FrameCounter % 4 != 0 JMP $92AD
            //      if(FrameCounter % 4 == 0)
            //      {
            //           $92A5: EE C0 06 INC Life  ;Raise Life by 1.
            //           $92A8: A9 28    LDA #$28
            //           $92AA: 20 51 C0 JSR $C051 ;Play Life Gain Sound
            //      }
            //
            //      ; Not sure what the next 2 commands are doing.
            //      ; Seem like part of the reglar game/draw loop since FrameCounter is updated.
            //      $92AD: 20 96 93 JSR $9396
            //      $92B0: 20 AB C0 JSR $C0AB
            //      $92B3: 4C 98 92 JMP $9298 ;Loop while (Life != 28)
            // }

            // Change $9296 to call a subroutine. Choosing $BF77 for the location.
            // $9296:20 77 BF   JSR $BF77
            // ---
            // $BF77:AD C0 06   LDA $06C0 ;$06C0 is Life
            // $BF7A:C9 1C      CMP #$1C
            // $BF7C:F0 02      BEQ $BF81 ;if(Life == 28) goto RTS
            // $BF7E:C6 A7      DEC $00A7 ;Decrement ETankCount
            // $BF81:60         RTS 

            int prgOffset = 0x30010 - 0x4000;
            // Inject new jump subroutine at 0D:9296 (should be 0x352A6).
            int jsrLocation = 0x9296 + prgOffset;
            var jsrBytes = new byte[]
            {
                0x20, 0x77, 0xBF,   // JSR $BF77
            };
            for(int offset = 0; offset < jsrBytes.Length; ++offset)
            {
                p.Add(jsrLocation + offset, jsrBytes[offset], "Prevent E-Tank Use at Full Life");
            }

            // Subroutine to decrement E-Tank Count. Skips decrement if Life == 28.
            var eTankSubroutineBytes = new byte[]
            {
                0xAD, 0xC0, 0x06,       // LDA $06C0 ;$06C0 is Life
                0xC9, 0x1C,             // CMP #$1C
                0xF0, 0x02,             // BEQ 2 ;if(Life == 28) goto RTS
                0xC6, 0xA7,             // DEC $00A7 ;Decrement ETankCount
                0x60,                   // RTS 
            };

            // Start at 0D:BF77 (should be 0x37F87).
            int etankLocation = 0xBF77 + prgOffset;
            for(int offset = 0; offset < eTankSubroutineBytes.Length; ++offset)
            {
                p.Add(etankLocation + offset, eTankSubroutineBytes[offset], "Prevent E-Tank Use at Full Life");
            }
        }
    }
}
