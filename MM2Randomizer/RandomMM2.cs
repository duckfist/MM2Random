using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

using MM2Randomizer.Enums;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static Random Random;
        public static MainWindowViewModel Settings;
        public static string DestinationFileName = "";

        public static List<StageFromSelect> StageSelect;
        public static List<ERMWeaponValueBit> NewWeaponOrder;

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static void Randomize()
        {
            try
            {
                // Prepare a copy of the file for modification
                CopyRom();

                // Instantiate RNG object RandomMM2.Random based on RandomMM2.Seed
                InitializeSeed();

                // Perform randomization and file modification according to each flag
                if (Settings.Is8StagesRandom)
                {
                    RandomStagePtrs();
                }
                if (Settings.IsWeaponsRandom)
                {
                    RandomWeapons();
                }
                if (Settings.IsWeaponBehaviorRandom)
                {
                    RandomWeaponBehavior();
                }
                if (Settings.IsWeaknessRandom)
                {
                    RandomWeaknesses();
                }
                if (Settings.IsItemsRandom)
                {
                    RandomItemNums();
                }
                if (Settings.IsTeleportersRandom)
                {
                    RandomTeleporters();
                }
                if (Settings.IsEnemiesRandom)
                {
                    RandomEnemies();
                }
                RandomTilemapChanges();
                if (Settings.IsColorsRandom)
                {
                    RandomColors();
                }
                if (Settings.IsBGMRandom)
                {
                    RandomBGM();
                }
                if (Settings.IsWeaponNamesRandom)
                {
                    RandomWeaponNames();
                }
                if (Settings.FastText)
                {
                    SetFastText();
                }
                if (Settings.BurstChaserMode)
                {
                    SetBurstChaser();
                }
                if (Settings.Is8StagesRandom || Settings.IsWeaponsRandom)
                {
                    FixPortraits();
                }
                

                // Create file name based on seed and game region
                string newfilename = (Settings.IsJapanese) ? "RM2" : "MM2";
                string seedAlpha = SeedConvert.ConvertBase10To26(Seed);
                newfilename = String.Format("{0}-RNG-{1}.nes", newfilename, seedAlpha);

                // Draw seed on title screen (U only)
                if (!Settings.IsJapanese)
                {
                    using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 0x037387;
                        for (int i = 0; i < seedAlpha.Length; i++)
                        {
                            char ch = seedAlpha.ElementAt(i);
                            byte charIndex = (byte)(Convert.ToByte(ch) - Convert.ToByte('A'));
                            stream.WriteByte((byte)(0xC1 + charIndex)); // 'A' starts at C1 in the pattern table
                        }
                    }
                }

                // If a file of the same seed already exists, delete it
                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }

                // Finish the copy/rename and open Explorer at that location
                File.Move(DestinationFileName, newfilename);
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", newfilename));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void SetFastText()
        {
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                if (Settings.IsJapanese)
                {
                    // Set fast weapon get text (J ONLY)
                    stream.Position = 0x037C51;
                }
                else
                {
                    // Set fast weapon get text (U ONLY)
                    stream.Position = 0x037D4A;
                }
                stream.WriteByte(0x04);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void SetBurstChaser()
        {
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Set fast "ready" text
                stream.Position = 0x038147;
                stream.WriteByte(0x60);

                // Set fast ground speed (integer)
                stream.Position = 0x038921;
                stream.WriteByte(0x03);

                // Set fast ground speed (fraction)
                stream.Position = 0x03892C;
                stream.WriteByte(0x00);

                // Set fast air speed (integer)
                stream.Position = 0x038922;
                stream.WriteByte(0x03);

                // Set fast air speed (fraction)
                stream.Position = 0x03892D;
                stream.WriteByte(0x00);

                // Set fast ladder climb up
                stream.Position = 0x0386EF;
                stream.WriteByte(0x01);

                // Set fast ladder climb down
                stream.Position = 0x03872E;
                stream.WriteByte(0xFE);

                if (Settings.IsJapanese)
                {
                    // Set fast buster projectiles (J ONLY)
                    stream.Position = 0x03D4A4;
                    stream.WriteByte(0x08);
                }
                else
                {
                    // Set fast buster projectiles (U ONLY)
                    stream.Position = 0x03D4A7;
                    stream.WriteByte(0x08);
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomEnemies()
        {
            REnemies er = new REnemies();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void RandomTilemapChanges()
        {
            RTilemap rTilemap = new RTilemap();
        }


        /// <summary>
        /// 
        /// </summary>
        private static void RandomWeaponBehavior()
        {
            RWeaponBehavior rWeaponBehavior = new RWeaponBehavior(Random);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomTeleporters()
        {
            // Create list of default teleporter position values
            List<byte[]> coords = new List<byte[]>
            {
                new byte[]{ 0x20, 0x3B }, // Teleporter X, Y (top-left)
                new byte[]{ 0x20, 0x7B },
                new byte[]{ 0x20, 0xBB },
                new byte[]{ 0x70, 0xBB },
                new byte[]{ 0x90, 0xBB },
                new byte[]{ 0xE0, 0x3B },
                new byte[]{ 0xE0, 0x7B },
                new byte[]{ 0xE0, 0xBB }
            };

            // Randomize them
            coords.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Write the new x-coordinates
                stream.Position = (long) EMiscAddresses.WarpXCoordinateStartAddress;
                foreach (byte[] location in coords)
                {
                    stream.WriteByte(location[0]);
                }

                // Write the new y-coordinates
                stream.Position = (long) EMiscAddresses.WarpYCoordinateStartAddress;
                foreach (byte[] location in coords)
                {
                    stream.WriteByte(location[1]);
                }

                // These values will be copied over to $04b0 (y) and $0470 (x), which will be checked
                // for in real time to determine where Mega will teleport to
            }
        }

        /// <summary>
        /// Create the modified ROM.
        /// </summary>
        private static void CopyRom()
        {
            string srcFile = "";
            if (Settings.IsJapanese)
            {
                srcFile = "j.dat";
            }
            else
            {
                srcFile = "u.dat";
            }
            DestinationFileName = "temp.nes";

            File.Copy(srcFile, DestinationFileName, true);
        }

        /// <summary>
        /// Create a random seed or use the user-provided seed.
        /// </summary>
        private static void InitializeSeed()
        {
            if (Seed < 0)
            {
                Random rndSeed = new Random();
                Seed = rndSeed.Next(int.MaxValue);
            }
            Random = new Random(Seed);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomColors()
        {
            RColors rColors = new RColors();
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomBGM()
        {
            List<EMusicID> newBGMOrder = new List<EMusicID>();
            List<EMusicID> robos = new List<EMusicID>();

            // Select 2 replacement tracks for the 2 extra instance of the boring W3/4/5 theme
            robos.Add(EMusicID.Flash);
            robos.Add(EMusicID.Heat);
            robos.Add(EMusicID.Air);
            robos.Add(EMusicID.Wood);
            robos.Add(EMusicID.Quick);
            robos.Add(EMusicID.Metal);
            robos.Add(EMusicID.Clash);
            robos.Add(EMusicID.Bubble);
            robos.Shuffle(Random);
            
            newBGMOrder.Add(EMusicID.Flash);
            newBGMOrder.Add(EMusicID.Heat);
            newBGMOrder.Add(EMusicID.Air);
            newBGMOrder.Add(EMusicID.Wood);
            newBGMOrder.Add(EMusicID.Quick);
            newBGMOrder.Add(EMusicID.Metal);
            newBGMOrder.Add(EMusicID.Clash);
            newBGMOrder.Add(EMusicID.Bubble);
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily12);  // Wily 1/2 track will play twice
            newBGMOrder.Add(EMusicID.Wily345); // Wily 3/4/5 track only plays once
            newBGMOrder.Add(robos[0]);         // Add extra Robot Master tracks to the set
            newBGMOrder.Add(robos[1]);

            // Randomize tracks
            newBGMOrder.Shuffle(Random);

            // Write new track order to ROM
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Start writing at Heatman BGM ID, both J and U
                stream.Position = 0x0381E0; 

                // Loop through BGM addresses Heatman to Wily 5 (Wily 6 still silent)
                for (int i = 0; i < newBGMOrder.Count; i++)
                {
                    EMusicID bgm = newBGMOrder[i];
                    stream.WriteByte((byte)bgm);
                }

                // Finally, fix Wily 5 track when exiting a Teleporter to be the selected Wily 5 track instead of default
                stream.Position = 0x038489;
                stream.WriteByte((byte)newBGMOrder.Last());
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomWeaponNames()
        {
            RWeaponNames rWeaponNames = new RWeaponNames();
        }

        /// <summary>
        /// Shuffle which Robot Master awards which weapon.
        /// </summary>
        private static void RandomWeapons()
        {
            // StageBeat    Address    Value
            // -----------------------------
            // Heat Man     0x03C289   1
            // Air Man      0x03C28A   2
            // Wood Man     0x03C28B   4
            // Bubble Man   0x03C28C   8
            // Quick Man    0x03C28D   16
            // Flash Man    0x03C28E   32
            // Metal Man    0x03C28F   64
            // Crash Man    0x03C290   128

            NewWeaponOrder = new List<ERMWeaponValueBit>()
            {
                ERMWeaponValueBit.HeatMan,
                ERMWeaponValueBit.AirMan,
                ERMWeaponValueBit.WoodMan,
                ERMWeaponValueBit.BubbleMan,
                ERMWeaponValueBit.QuickMan,
                ERMWeaponValueBit.FlashMan,
                ERMWeaponValueBit.MetalMan,
                ERMWeaponValueBit.CrashMan
            }.Select(s => s).ToList();
            
            NewWeaponOrder.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Create table for which weapon is awarded by which robot master
                // This also affects which portrait is blacked out on the stage select
                // This also affects which teleporter deactivates after defeating a Wily 5 refight boss
                stream.Position = (long) ERMStageWeaponAddress.HeatMan;// 0x03c289;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)NewWeaponOrder[i]);
                }

                // Create a copy of the default weapon order table to be used by teleporter function
                // This is needed to fix teleporters breaking from the new weapon order.
                //stream.Position = 0x03f2D0; // Unused space at end of bank
                stream.Position = 0x03f310; // Unused space at end of bank
                stream.WriteByte((byte) ERMWeaponValueBit.HeatMan);
                stream.WriteByte((byte) ERMWeaponValueBit.AirMan);
                stream.WriteByte((byte) ERMWeaponValueBit.WoodMan);
                stream.WriteByte((byte) ERMWeaponValueBit.BubbleMan);
                stream.WriteByte((byte) ERMWeaponValueBit.QuickMan);
                stream.WriteByte((byte) ERMWeaponValueBit.FlashMan);
                stream.WriteByte((byte) ERMWeaponValueBit.MetalMan);
                stream.WriteByte((byte) ERMWeaponValueBit.CrashMan);

                // Change function to call $f300 instead of $c279 when looking up defeated refight boss to
                // get our default weapon table, fixing the teleporter softlock
                stream.Position = 0x03843b;
                stream.WriteByte(0x00);
                stream.WriteByte(0xf3);

                // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
                stream.Position = (long) ERMStageSelect.FirstStageInMemory; // 0x0346E1;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)NewWeaponOrder[i]);
                }
            }
        }

        /// <summary>
        /// Shuffle which Robot Master awards Items 1, 2, and 3.
        /// </summary>
        private static void RandomItemNums()
        {
            // 0x03C291 - Item # from Heat Man
            // 0x03C292 - Item # from Air Man
            // 0x03C293 - Item # from Wood Man
            // 0x03C294 - Item # from Bubble Man
            // 0x03C295 - Item # from Quick Man
            // 0x03C296 - Item # from Flash Man
            // 0x03C297 - Item # from Metal Man
            // 0x03C298 - Item # from Crash Man

            List<EItemNumber> newItemOrder = new List<EItemNumber>();
            for (byte i = 0; i < 5; i++) newItemOrder.Add(EItemNumber.None);
            newItemOrder.Add(EItemNumber.One);
            newItemOrder.Add(EItemNumber.Two);
            newItemOrder.Add(EItemNumber.Three);

            newItemOrder.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = (long) EItemStageAddress.HeatMan; //0x03C291;
                for (int i = 0; i < 8; i++)
                { 
                    stream.WriteByte((byte)newItemOrder[i]);
                }


            }
        }

        /// <summary>
        /// Shuffle the Robot Master stages.  This shuffling will not be indicated by the Robot Master portraits.
        /// </summary>
        private static void RandomStagePtrs()
        {
            // StageSelect  Address    Value
            // -----------------------------
            // Bubble Man   0x034670   3
            // Air Man      0x034671   1
            // Quick Man    0x034672   4
            // Wood Man     0x034673   2
            // Crash Man    0x034674   7
            // Flash Man    0x034675   5
            // Metal Man    0x034676   6
            // Heat Man     0x034677   0
            
            StageSelect = new List<StageFromSelect>();

            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Bubble Man",
                PortraitAddress = ERMPortraitAddress.BubbleMan,
                PortraitDestinationOriginal = 3,
                PortraitDestinationNew = 3,
                StageClearAddress = ERMStageClearAddress.BubbleMan,
                StageClearDestinationOriginal = 8,
                StageClearDestinationNew = 8
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Air Man",
                PortraitAddress = ERMPortraitAddress.AirMan,
                PortraitDestinationOriginal = 1,
                PortraitDestinationNew = 1,
                StageClearAddress = ERMStageClearAddress.AirMan,
                StageClearDestinationOriginal = 2,
                StageClearDestinationNew = 2
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Quick Man",
                PortraitAddress = ERMPortraitAddress.QuickMan,
                PortraitDestinationOriginal = 4,
                PortraitDestinationNew = 4,
                StageClearAddress = ERMStageClearAddress.QuickMan,
                StageClearDestinationOriginal = 16,
                StageClearDestinationNew = 16
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Wood Man",
                PortraitAddress = ERMPortraitAddress.WoodMan,
                PortraitDestinationOriginal = 2,
                PortraitDestinationNew = 2,
                StageClearAddress = ERMStageClearAddress.WoodMan,
                StageClearDestinationOriginal = 4,
                StageClearDestinationNew = 4
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Clash Man",
                PortraitAddress = ERMPortraitAddress.CrashMan,
                PortraitDestinationOriginal = 7,
                PortraitDestinationNew = 7,
                StageClearAddress = ERMStageClearAddress.CrashMan,
                StageClearDestinationOriginal = 128,
                StageClearDestinationNew = 128
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Flash Man",
                PortraitAddress = ERMPortraitAddress.FlashMan,
                PortraitDestinationOriginal = 5,
                PortraitDestinationNew = 5,
                StageClearAddress = ERMStageClearAddress.FlashMan,
                StageClearDestinationOriginal = 32,
                StageClearDestinationNew = 32
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Metal Man",
                PortraitAddress = ERMPortraitAddress.MetalMan,
                PortraitDestinationOriginal = 6,
                PortraitDestinationNew = 6,
                StageClearAddress = ERMStageClearAddress.MetalMan,
                StageClearDestinationOriginal = 64,
                StageClearDestinationNew = 64
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Heat Man",
                PortraitAddress = ERMPortraitAddress.HeatMan,
                PortraitDestinationOriginal = 0,
                PortraitDestinationNew = 0, // 4 = quick
                StageClearAddress = ERMStageClearAddress.HeatMan,
                StageClearDestinationOriginal = 1,
                StageClearDestinationNew = 1
            });


            List<byte> newStageOrder = new List<byte>();
            for (byte i = 0; i < 8; i++) newStageOrder.Add(i);

            newStageOrder.Shuffle(Random);

            for (int i = 0; i < 8; i++)
            {
                string portrait = StageSelect[i].PortraitName;
                StageSelect[i].PortraitDestinationNew = StageSelect[newStageOrder[i]].PortraitDestinationOriginal;
                //StageSelect[i].StageClearDestinationNew = StageSelect[newStageOrder[i]].StageClearDestinationOriginal;
            }
            
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (StageFromSelect stage in StageSelect)
                {
                    stream.Position = (long)stage.PortraitAddress;
                    stream.WriteByte((byte)stage.PortraitDestinationNew);
                    //stream.Position = stage.StageClearAddress;
                    //stream.WriteByte((byte)stage.StageClearDestinationNew);
                }
            }
        }

        /// <summary>
        /// Enabling Random Weapons or Random Stages will cause the wrong Robot Master portrait to
        /// be blacked out when a stage is completed. The game uses your acquired weapons to determine
        /// which portrait to black-out. This function changes the lookup table for x and y positions
        /// of portraits to black-out based on what was randomized.
        /// </summary>
        private static void FixPortraits()
        {
            // Arrays of default values for X and Y of the black square that marks out each portrait
            // Index of arrays are stage order, e.g. Heat, Air, etc.
            byte[] portraitBG_y     = new byte[] { 0x21, 0x20, 0x21, 0x20, 0x20, 0x22, 0x22, 0x22 };
            byte[] portraitBG_x     = new byte[] { 0x86, 0x8E, 0x96, 0x86, 0x96, 0x8E, 0x86, 0x96 };

            // Adjusting the sprites is not necessary because the hacked portrait graphics ("?" images)
            // only use the background, and the sprites have been blacked out. Left in for reference.
            //byte[] portraitSprite_x = new byte[] { 0x3C, 0x0C, 0x4C, 0x00, 0x20, 0x84, 0x74, 0xA4 };
            //byte[] portraitSprite_y = new byte[] { 0x10, 0x14, 0x28, 0x0C, 0x1C, 0x20, 0x10, 0x18 };

            // Apply changes to portrait arrays based on shuffled stages
            if (Settings.Is8StagesRandom)
            {
                // Get the new stage order
                int[] newOrder = new int[8];
                foreach (StageFromSelect stage in StageSelect)
                    newOrder[stage.PortraitDestinationOriginal] = stage.PortraitDestinationNew;

                // Permute portrait x/y values via the shuffled stage-order array 
                byte[] cpy = new byte[8];
                for (int i = 0; i < 8; i++)
                    cpy[newOrder[i]] = portraitBG_y[i];
                Array.Copy(cpy, portraitBG_y, 8);

                for (int i = 0; i < 8; i++)
                    cpy[newOrder[i]] = portraitBG_x[i];
                Array.Copy(cpy, portraitBG_x, 8);

                //for (int i = 0; i < 8; i++)
                //    cpy[i] = portraitSprite_y[newOrder[i]];
                //Array.Copy(cpy, portraitSprite_y, 8);

                //for (int i = 0; i < 8; i++)
                //    cpy[i] = portraitSprite_x[newOrder[i]];
                //Array.Copy(cpy, portraitSprite_x, 8);
            }

            // Apply changes to portrait arrays based on shuffled weapons. Only need a standard "if" with no "else",
            // because the arrays must be permuted twice if both randomization settings are enabled.
            if (Settings.IsWeaponsRandom)
            {
                // Since the acquired-weapons table's elements are powers of two, get a new array of their 0-7 index
                int[] newWeaponIndex = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    int j = 0;
                    byte val = (byte)NewWeaponOrder[i];
                    while (val != 0)
                    {
                        val = (byte)(val >> 1);
                        j++;
                    }
                    newWeaponIndex[i] = j - 1;
                }

                // Permute portrait x/y values via the shuffled acquired-weapons array 
                byte[] cpy = new byte[8];
                for (int i = 0; i < 8; i++)
                    cpy[newWeaponIndex[i]] = portraitBG_y[i];
                Array.Copy(cpy, portraitBG_y, 8);

                for (int i = 0; i < 8; i++)
                    cpy[newWeaponIndex[i]] = portraitBG_x[i];
                Array.Copy(cpy, portraitBG_x, 8);
            }

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                for (int i = 0; i < 8; i++)
                {
                    stream.Position = 0x034541 + i;
                    stream.WriteByte(portraitBG_y[i]);
                    stream.Position = 0x034549 + i;
                    stream.WriteByte(portraitBG_x[i]); 
                    // Changing this sprite table misplaces their positions by default.
                    //stream.Position = 0x03460D + i;
                    //stream.WriteByte(portraitSprite_y[i]);
                    //stream.Position = 0x034615 + i;
                    //stream.WriteByte(portraitSprite_x[i]);
                }
            }

        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomWeaknesses()
        {
            RWeaknesses rWeaknesses = new RWeaknesses();
        }

        /// <summary>
        /// Shuffle the elements of the provided list.
        /// </summary>
        /// <typeparam name="T">The Type of the elements in the list.</typeparam>
        /// <param name="list">The object to be shuffled.</param>
        /// <param name="rng">The seed used to perform the shuffling.</param>
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


    }
}
