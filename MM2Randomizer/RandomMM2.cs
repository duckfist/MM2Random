using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MM2Randomizer.Enums;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Enemies;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static Random Random;
        public static MainWindowViewModel Settings;
        public static string DestinationFileName = "";

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static void Randomize()
        {
            try
            {
                CopyRom();
                InitializeSeed();

                if (Settings.Is8StagesRandom)
                {
                    RandomStagePtrs();
                }
                if (Settings.IsWeaponsRandom)
                {
                    RandomWeapons();
                }
                if (Settings.IsItemsRandom)
                {
                    RandomItemNums();
                }
                if (Settings.IsTeleportersRandom)
                {
                    RandomTeleporters();
                }
                if (Settings.IsWeaknessRandom)
                {
                    // Offsets are different in Rockman 2 and Mega Man 2
                    if (Settings.IsJapanese)
                        RandomWeaknesses();
                    else
                        RandomWeaknessesMM2();
                }
                if (Settings.IsEnemiesRandom)
                {
                    RandomEnemies();
                }
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
                if (Settings.BurstChaserMode)
                {
                    SetBurstChaser();
                }

                string newfilename = (Settings.IsJapanese) ? "RM2" : "MM2";
                newfilename = String.Format("{0}-RNG-{1}.nes", newfilename, Seed);

                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }

                File.Move(DestinationFileName, newfilename);

                string str = string.Format("/select,\"{0}\\{1}\"",
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                newfilename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private static void SetBurstChaser()
        {
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Set fast weapon get text (J ONLY)
                stream.Position = 0x037C51;
                stream.WriteByte(0x02);

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

                // Set fast buster projectiles (J ONLY)
                stream.Position = 0x03D4A4;
                stream.WriteByte(0x08);

                // Set fast ladder climb up
                stream.Position = 0x0386EF;
                stream.WriteByte(0x01);

                // Set fast ladder climb down
                stream.Position = 0x03872E;
                stream.WriteByte(0xFE);

            }
        }

        private static void RandomEnemies()
        {
            EnemyRandomizer er = new EnemyRandomizer();
        }

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
            List<ColorSet> colorSets = new List<ColorSet>()
            {
                new ColorSet() { // Heat | River 
                    addresses = new int[] {
                        0x3e1f, 0x3e20, 0x3e21, // default BG
                        0x3e3f, 0x3e4f, 0x3e5f, // animated BG
                        0x3e40, 0x3e50, 0x3e60,
                        0x3e41, 0x3e51, 0x3e61 },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.Taupe,EColorsHex.LightOrange,EColorsHex.Orange,
                            EColorsHex.Taupe,       EColorsHex.LightOrange,  EColorsHex.Orange,
                            EColorsHex.LightOrange, EColorsHex.Orange,  EColorsHex.Taupe,
                            EColorsHex.Orange,      EColorsHex.Taupe,  EColorsHex.LightOrange,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightGreen,EColorsHex.Green,EColorsHex.ForestGreen,
                            EColorsHex.LightGreen,  EColorsHex.Green,  EColorsHex.ForestGreen,
                            EColorsHex.Green,       EColorsHex.ForestGreen,  EColorsHex.LightGreen,
                            EColorsHex.ForestGreen, EColorsHex.LightGreen,  EColorsHex.Green,
                        },
                        new EColorsHex[] {
                            EColorsHex.Yellow,EColorsHex.GoldenRod,EColorsHex.Brown,
                            EColorsHex.Yellow,      EColorsHex.GoldenRod,  EColorsHex.Brown,
                            EColorsHex.GoldenRod,   EColorsHex.Brown,  EColorsHex.Yellow,
                            EColorsHex.Brown,       EColorsHex.Yellow,  EColorsHex.GoldenRod,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightPink,EColorsHex.Magenta,EColorsHex.DarkMagenta,
                            EColorsHex.LightPink,   EColorsHex.Magenta,  EColorsHex.DarkMagenta,
                            EColorsHex.Magenta,     EColorsHex.DarkMagenta,  EColorsHex.LightPink,
                            EColorsHex.DarkMagenta, EColorsHex.LightPink,  EColorsHex.Magenta,
                        }
                    }
                },
                new ColorSet() { // Heat | Background
                    addresses = new int[] {
                        0x3e1b, 0x3e1c, 0x3e1d,  // default BG
                        0x3e3b, 0x3e4b, 0x3e5b,  // animated BG1
                        0x3e3c, 0x3e4c, 0x3e5c,  // animated BG2
                        0x3e3d, 0x3e4d, 0x3e5d },// animated BG3
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.Orange,EColorsHex.Crimson,EColorsHex.DarkRed,
                            EColorsHex.Orange, EColorsHex.Orange,  EColorsHex.Orange,
                            EColorsHex.Crimson,EColorsHex.Crimson,  EColorsHex.Crimson,
                            EColorsHex.DarkRed,EColorsHex.DarkRed,  EColorsHex.DarkRed,
                        },
                        new EColorsHex[] {
                            EColorsHex.Magenta,EColorsHex.DarkMagenta,EColorsHex.RoyalPurple,
                            EColorsHex.Magenta,     EColorsHex.Magenta,  EColorsHex.Magenta,
                            EColorsHex.DarkMagenta, EColorsHex.DarkMagenta,  EColorsHex.DarkMagenta,
                            EColorsHex.RoyalPurple, EColorsHex.RoyalPurple,  EColorsHex.RoyalPurple,
                        },
                        new EColorsHex[] {
                            EColorsHex.Magenta,EColorsHex.DarkMagenta,EColorsHex.RoyalPurple,
                            EColorsHex.MediumGray,  EColorsHex.MediumGray,  EColorsHex.MediumGray,
                            EColorsHex.GoldenRod,   EColorsHex.GoldenRod,  EColorsHex.GoldenRod,
                            EColorsHex.Brown,       EColorsHex.Brown,  EColorsHex.Brown,
                        },
                        new EColorsHex[] {
                            EColorsHex.DarkTeal,EColorsHex.RoyalBlue,EColorsHex.Blue,
                            EColorsHex.DarkTeal,    EColorsHex.DarkTeal,  EColorsHex.DarkTeal,
                            EColorsHex.RoyalBlue,   EColorsHex.RoyalBlue,  EColorsHex.RoyalBlue,
                            EColorsHex.Blue,        EColorsHex.Blue,  EColorsHex.Blue,
                        },
                        new EColorsHex[] {
                            EColorsHex.DarkGreen,EColorsHex.Black3,EColorsHex.Kelp,
                            EColorsHex.DarkGreen,   EColorsHex.DarkGreen,  EColorsHex.DarkGreen,
                            EColorsHex.Black3,      EColorsHex.Black3,  EColorsHex.Black3,
                            EColorsHex.Kelp,        EColorsHex.Kelp,  EColorsHex.Kelp,
                        },
                    }
                },
                new ColorSet() { // Heat | Foreground
                    addresses = new int[] {
                        0x3e13, 0x3e14, 0x3e15,  // default BG
                        0x3e33, 0x3e43, 0x3e53,  // animated BG1
                        0x3e34, 0x3e44, 0x3e54,  // animated BG2
                        0x3e35, 0x3e45, 0x3e55 },// animated BG3
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.Taupe,EColorsHex.LightOrange,EColorsHex.VioletRed,
                            EColorsHex.Taupe,       EColorsHex.Taupe,  EColorsHex.Taupe,
                            EColorsHex.LightOrange, EColorsHex.LightOrange,  EColorsHex.LightOrange,
                            EColorsHex.VioletRed,   EColorsHex.VioletRed,  EColorsHex.VioletRed,
                        },
                        new EColorsHex[] {
                            EColorsHex.PastelPink,EColorsHex.LightPink,EColorsHex.Purple,
                            EColorsHex.PastelPink,  EColorsHex.PastelPink,  EColorsHex.PastelPink,
                            EColorsHex.LightPink,   EColorsHex.LightPink,  EColorsHex.LightPink,
                            EColorsHex.Purple,      EColorsHex.Purple,  EColorsHex.Purple,
                        },
                        new EColorsHex[] {
                            EColorsHex.PastelGreen,EColorsHex.LightGreen,EColorsHex.Grass,
                            EColorsHex.PastelGreen, EColorsHex.PastelGreen,  EColorsHex.PastelGreen,
                            EColorsHex.LightGreen,  EColorsHex.LightGreen,  EColorsHex.LightGreen,
                            EColorsHex.Grass,       EColorsHex.Grass,  EColorsHex.Grass,
                        },
                        new EColorsHex[] {
                            EColorsHex.PaleBlue,EColorsHex.SoftBlue,EColorsHex.MediumBlue,
                            EColorsHex.PaleBlue,    EColorsHex.PaleBlue,  EColorsHex.PaleBlue,
                            EColorsHex.SoftBlue,    EColorsHex.SoftBlue,  EColorsHex.SoftBlue,
                            EColorsHex.MediumBlue,  EColorsHex.MediumBlue,  EColorsHex.MediumBlue,
                        },
                    }
                },
                new ColorSet() { // Heat | Foreground2
                    addresses = new int[] {
                        0x3e17, 0x3e18, 0x3e19,  // default BG
                        0x3e37, 0x3e47, 0x3e57,  // animated BG1
                        0x3e38, 0x3e48, 0x3e58,  // animated BG2
                        0x3e39, 0x3e49, 0x3e59 },// animated BG3
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.LightGray,EColorsHex.Gray,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,
                            EColorsHex.LightGray,  EColorsHex.LightGray,  EColorsHex.LightGray,
                            EColorsHex.Gray,       EColorsHex.Gray,  EColorsHex.Gray,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Beige,EColorsHex.YellowOrange,
                            EColorsHex.White,          EColorsHex.White,  EColorsHex.White,
                            EColorsHex.Beige,          EColorsHex.Beige,  EColorsHex.Beige,
                            EColorsHex.YellowOrange,   EColorsHex.YellowOrange,  EColorsHex.YellowOrange,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.PastelBlue,EColorsHex.LightBlue,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,
                            EColorsHex.PastelBlue, EColorsHex.PastelBlue,  EColorsHex.PastelBlue,
                            EColorsHex.LightBlue,  EColorsHex.LightBlue,  EColorsHex.LightBlue,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightGray,EColorsHex.Gray,EColorsHex.Brown,
                            EColorsHex.LightGray,  EColorsHex.LightGray,  EColorsHex.LightGray,
                            EColorsHex.Gray,       EColorsHex.Gray,  EColorsHex.Gray,
                            EColorsHex.Brown,      EColorsHex.Brown,  EColorsHex.Brown,
                        },
                    }
                },
                new ColorSet() { // Air | Platforms
                    addresses = new int[] {
                        0x7e17, 0x7e18, 0x7e19,
                        0x7e37, 0x7e47, 0x7e57, 0x7e67,
                        0x7e38, 0x7e48, 0x7e58, 0x7e68,
                        0x7e39, 0x7e49, 0x7e59, 0x7e69 },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.VioletRed,EColorsHex.Black2,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.VioletRed,  EColorsHex.VioletRed,  EColorsHex.VioletRed,  EColorsHex.VioletRed,
                            EColorsHex.Black2,     EColorsHex.Black2,  EColorsHex.Black2,  EColorsHex.Black2,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Orange,EColorsHex.Black2,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.Orange,     EColorsHex.Orange,  EColorsHex.Orange,  EColorsHex.Orange,
                            EColorsHex.Black2,     EColorsHex.Black2,  EColorsHex.Black2,  EColorsHex.Black2,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Yellow,EColorsHex.Black2,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.Yellow,     EColorsHex.Yellow,  EColorsHex.Yellow,  EColorsHex.Yellow,
                            EColorsHex.Black2,     EColorsHex.Black2,  EColorsHex.Black2,  EColorsHex.Black2,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Grass,EColorsHex.Black2,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.Grass,      EColorsHex.Grass,  EColorsHex.Grass,  EColorsHex.Grass,
                            EColorsHex.Black2,     EColorsHex.Black2,  EColorsHex.Black2,  EColorsHex.Black2,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.SoftBlue,EColorsHex.Black2,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.SoftBlue,   EColorsHex.SoftBlue,  EColorsHex.SoftBlue,  EColorsHex.SoftBlue,
                            EColorsHex.Black2,     EColorsHex.Black2,  EColorsHex.Black2,  EColorsHex.Black2,
                        },
                    }
                },
                new ColorSet() { // Air | Clouds 
                    addresses = new int[] {
                        0x7e13, 0x7e14, 0x7e15,
                        0x7e33, 0x7e43, 0x7e53, 0x7e63,
                        0x7e34, 0x7e44, 0x7e54, 0x7e64,
                        0x7e35, 0x7e45, 0x7e55, 0x7e65 },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.LightBlue,EColorsHex.PastelBlue,EColorsHex.White,
                            EColorsHex.LightBlue,  EColorsHex.PastelBlue,  EColorsHex.White,  EColorsHex.PastelBlue,
                            EColorsHex.PastelBlue, EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                            EColorsHex.White,      EColorsHex.White,  EColorsHex.White,  EColorsHex.White,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightGray,EColorsHex.LightGray,EColorsHex.LightGray,
                            EColorsHex.LightGray,  EColorsHex.Gray,  EColorsHex.DarkRed,  EColorsHex.Gray,
                            EColorsHex.LightGray,  EColorsHex.LightGray,  EColorsHex.Gray,  EColorsHex.LightGray,
                            EColorsHex.LightGray,  EColorsHex.LightGray,  EColorsHex.LightGray,  EColorsHex.LightGray,
                        },
                    }
                },
                new ColorSet() { // Air | Sky 
                    addresses = new int[] { 0x7e22 },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {  EColorsHex.LightBlue },
                        new EColorsHex[] {  EColorsHex.LightPurple },
                        new EColorsHex[] {  EColorsHex.LightOrange },
                        new EColorsHex[] {  EColorsHex.YellowOrange },
                        new EColorsHex[] {  EColorsHex.Yellow },
                        new EColorsHex[] {  EColorsHex.Lime },
                        new EColorsHex[] {  EColorsHex.RoyalBlue },
                        new EColorsHex[] {  EColorsHex.RoyalBlue },
                        new EColorsHex[] {  EColorsHex.DarkGreen },
                        new EColorsHex[] {  EColorsHex.Black3 },
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe13, 0xbe14, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | Leaves | Default
                        new EColorsHex[] {  EColorsHex.Lemon, EColorsHex.Grass,},
                        // Wood | Leaves | Blue
                        new EColorsHex[] {  EColorsHex.MediumBlue, EColorsHex.RoyalBlue,},
                        // Wood | Leaves | Red
                        new EColorsHex[] {  EColorsHex.Orange, EColorsHex.Red,},
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe17, 0xbe18, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | Trunk | Default
                        new EColorsHex[] {  EColorsHex.Yellow,  EColorsHex.GoldenRod },
                        // Wood | Trunk | Purple
                        new EColorsHex[] {  EColorsHex.LightPurple,  EColorsHex.Purple },
                        // Wood | Trunk | Pink
                        new EColorsHex[] {  EColorsHex.LightVioletRed,  EColorsHex.VioletRed },
                        // Wood | Trunk | Orange
                        new EColorsHex[] {  EColorsHex.YellowOrange,  EColorsHex.Tangerine },
                        // Wood | Trunk | Green
                        new EColorsHex[] {  EColorsHex.LightGreen,  EColorsHex.Green },
                        // Wood | Trunk | Teal
                        new EColorsHex[] {  EColorsHex.LightCyan,  EColorsHex.Teal },
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe1b,0xbe1c,0xbe1d,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | Floor | Default
                        new EColorsHex[] {  EColorsHex.YellowOrange, EColorsHex.Tangerine, EColorsHex.DarkRed,},
                        // Wood | Floor | Yellow
                        new EColorsHex[] {  EColorsHex.Yellow, EColorsHex.GoldenRod, EColorsHex.Brown,},
                        // Wood | Floor | Green
                        new EColorsHex[] {  EColorsHex.LightGreen, EColorsHex.Green, EColorsHex.ForestGreen,},
                        // Wood | Floor | Teal
                        new EColorsHex[] {  EColorsHex.LightCyan, EColorsHex.Teal, EColorsHex.DarkTeal,},
                        // Wood | Floor | Purple
                        new EColorsHex[] {  EColorsHex.LightPurple, EColorsHex.Purple, EColorsHex.RoyalPurple,},
                        // Wood | Floor | Gray
                        new EColorsHex[] {  EColorsHex.NearWhite, EColorsHex.LightGray, EColorsHex.Black2,},
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe1f, 0x03a118, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | UndergroundBG | Default
                        new EColorsHex[] {  EColorsHex.Brown,  EColorsHex.Brown },
                        // Wood | UndergroundBG | Dark Purple
                        new EColorsHex[] {  EColorsHex.DarkMagenta,  EColorsHex.DarkMagenta },
                        // Wood | UndergroundBG | Dark Red
                        new EColorsHex[] {  EColorsHex.Crimson,  EColorsHex.Crimson },
                        // Wood | UndergroundBG | Dark Green
                        new EColorsHex[] {  EColorsHex.Kelp,  EColorsHex.Kelp },
                        // Wood | UndergroundBG | Dark Teal
                        new EColorsHex[] {  EColorsHex.DarkGreen,  EColorsHex.DarkGreen },
                        // Wood | UndergroundBG | Dark Blue1
                        new EColorsHex[] {  EColorsHex.DarkTeal,  EColorsHex.DarkTeal },
                        // Wood | UndergroundBG | Dark Blue2
                        new EColorsHex[] {  EColorsHex.RoyalBlue,  EColorsHex.RoyalBlue },
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe15,0xbe19,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | SkyBG | Default
                        new EColorsHex[] {  EColorsHex.LightCyan,  EColorsHex.LightCyan },
                        // Wood | SkyBG | Light Green
                        new EColorsHex[] {  EColorsHex.LightGreen , EColorsHex.LightGreen },
                        // Wood | SkyBG | Blue
                        new EColorsHex[] {  EColorsHex.Blue,  EColorsHex.Blue },
                        // Wood | SkyBG | Dark Purple
                        new EColorsHex[] {  EColorsHex.RoyalPurple,  EColorsHex.RoyalPurple },
                        // Wood | SkyBG | Dark Red
                        new EColorsHex[] {  EColorsHex.Crimson,  EColorsHex.Crimson },
                        // Wood | SkyBG | Light Yellow
                        new EColorsHex[] {  EColorsHex.PastelYellow,  EColorsHex.PastelYellow },
                        // Wood | SkyBG | Black
                        new EColorsHex[] {  EColorsHex.Black2,  EColorsHex.Black2 },
                    }
                },

                new ColorSet() { // Bubble | White Floors
                    addresses = new int[] {
                        0xfe17,0xfe18, // default BG
                        0xfe37,0xfe38, // frame 1
                        0xfe47,0xfe48, // frame 2
                        0xfe57,0xfe58, // frame 3
                    },
                    ColorBytes = new List<EColorsHex[]>() { 
                        new EColorsHex[] { // original colors
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f, (EColorsHex)0x05,
                            (EColorsHex)0x0f, (EColorsHex)0x05,
                            (EColorsHex)0x0f, (EColorsHex)0x09,
                            (EColorsHex)0x0f, (EColorsHex)0x01,
                        },
                    }
                },

                new ColorSet() { // Bubble | Underwater Floors
                    addresses = new int[] {
                        0xfe13,0xfe14, // default BG
                        0xfe33,0xfe34, // frame 1
                        0xfe43,0xfe44, // frame 2
                        0xfe53,0xfe54 // frame 3
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] { // original colors
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                            (EColorsHex)0x20, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                            (EColorsHex)0x31, (EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                            (EColorsHex)0x33, (EColorsHex)0x23,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                            (EColorsHex)0x36, (EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                            (EColorsHex)0x38, (EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                            (EColorsHex)0x3a, (EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f, (EColorsHex)0x05,
                            (EColorsHex)0x0f, (EColorsHex)0x05,
                            (EColorsHex)0x0f, (EColorsHex)0x09,
                            (EColorsHex)0x0f, (EColorsHex)0x01,
                        },
                    }
                },

                new ColorSet() { // Bubble | Waterfall
                    addresses = new int[] {
                        0xfe1f,0xfe20,0xfe21, // default BG
                        0xfe3f,0xfe40,0xfe41, // frame 1
                        0xfe4f,0xfe50,0xfe51, // frame 2
                        0xfe5f,0xfe60,0xfe61, // frame 3
                    },
                    ColorBytes = new List<EColorsHex[]>() { 
                        new EColorsHex[] { // original colors
                            (EColorsHex)0x20,(EColorsHex)0x21,(EColorsHex)0x11,
                            (EColorsHex)0x20,(EColorsHex)0x21,(EColorsHex)0x11,
                            (EColorsHex)0x21,(EColorsHex)0x11,(EColorsHex)0x20,
                            (EColorsHex)0x11,(EColorsHex)0x20,(EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x21,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x21,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x22,(EColorsHex)0x12,(EColorsHex)0x21,
                            (EColorsHex)0x12,(EColorsHex)0x21,(EColorsHex)0x22,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x26,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x26,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x27,(EColorsHex)0x17,(EColorsHex)0x26,
                            (EColorsHex)0x17,(EColorsHex)0x26,(EColorsHex)0x27,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x27,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x27,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x28,(EColorsHex)0x18,(EColorsHex)0x27,
                            (EColorsHex)0x18,(EColorsHex)0x27,(EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2a,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x2a,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x2b,(EColorsHex)0x1b,(EColorsHex)0x2a,
                            (EColorsHex)0x1b,(EColorsHex)0x2a,(EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2b,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x2b,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x2c,(EColorsHex)0x1c,(EColorsHex)0x2b,
                            (EColorsHex)0x1c,(EColorsHex)0x2b,(EColorsHex)0x2c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f,(EColorsHex)0x0c,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x0c,(EColorsHex)0x00,
                            (EColorsHex)0x0c,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x0c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f,(EColorsHex)0x05,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x05,(EColorsHex)0x00,
                            (EColorsHex)0x05,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x05,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f,(EColorsHex)0x08,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x08,(EColorsHex)0x00,
                            (EColorsHex)0x08,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x08,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f,(EColorsHex)0x09,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x09,(EColorsHex)0x00,
                            (EColorsHex)0x09,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x09,
                        },
                    }
                },

                new ColorSet() { // Bubble | Water
                    addresses = new int[] { 0xfe22, },
                    ColorBytes = new List<EColorsHex[]>() { 
                        new EColorsHex[] {
                            (EColorsHex)0x11, // original color
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x01,
                        },
                        new EColorsHex[] {
                            EColorsHex.Black3,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x13,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x04,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x05,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x16,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x07,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x17,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x09,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0c,
                        },
                    }
                                },

                new ColorSet() { // Flash | Background
                    addresses = new int[] { 0x017e14, 0x017e15,            // default BG colors
                                            0x017e34, 0x017e44, 0x017e54,  // animated BG frame 1
                                            0x017e35, 0x017e45, 0x017e55 },// animated BG frame 2 
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                             EColorsHex.Blue,  EColorsHex.DarkBlue,
                             EColorsHex.Blue,  EColorsHex.Blue,  EColorsHex.Blue,
                             EColorsHex.DarkBlue,  EColorsHex.DarkBlue,  EColorsHex.DarkBlue,
                        },
                        new EColorsHex[] {
                             EColorsHex.Magenta,  EColorsHex.DarkMagenta,
                             EColorsHex.Magenta,  EColorsHex.Magenta,  EColorsHex.Magenta,
                             EColorsHex.DarkMagenta,  EColorsHex.DarkMagenta,  EColorsHex.DarkMagenta,
                        },
                        new EColorsHex[] {
                             EColorsHex.Orange,  EColorsHex.Red,
                             EColorsHex.Orange,  EColorsHex.Orange,  EColorsHex.Orange,
                             EColorsHex.Red,  EColorsHex.Red,  EColorsHex.Red,
                        },
                        new EColorsHex[] {
                             EColorsHex.GoldenRod,  EColorsHex.Brown,
                             EColorsHex.GoldenRod,  EColorsHex.GoldenRod,  EColorsHex.GoldenRod,
                             EColorsHex.Brown,  EColorsHex.Brown,  EColorsHex.Brown,
                        },
                        new EColorsHex[] {
                             EColorsHex.Green,  EColorsHex.ForestGreen,
                             EColorsHex.Green,  EColorsHex.Green,  EColorsHex.Green,
                             EColorsHex.ForestGreen,  EColorsHex.ForestGreen,  EColorsHex.ForestGreen,
                        },
                        new EColorsHex[] {
                             EColorsHex.Gray,  EColorsHex.Black3,
                             EColorsHex.Gray,  EColorsHex.Gray,  EColorsHex.Gray,
                             EColorsHex.Black3,  EColorsHex.Black3,  EColorsHex.Black3,
                        },
                    }
                },
                new ColorSet() { // Flash | Foreground
                    // Note: 3 color sets are used for the flashing blocks.
                    // I've kept them grouped here for common color themes.
                    addresses = new int[] {
                        0x017e17, 0x017e18, 0x017e19, 0x017e1b, 0x017e1c, 0x017e1d, 0x017e1f, 0x017e20, 0x017e21, // default BG colors
                        0x017e37, 0x017e47, 0x017e57, // animated BG block 1 frame 1
                        0x017e38, 0x017e48, 0x017e58, // animated BG block 1 frame 2
                        0x017e39, 0x017e49, 0x017e59, // animated BG block 1 frame 3
                        0x017e3b, 0x017e4b, 0x017e5b, // animated BG block 2 frame 1
                        0x017e3c, 0x017e4c, 0x017e5c, // animated BG block 2 frame 2
                        0x017e3d, 0x017e4d, 0x017e5d, // animated BG block 2 frame 3
                        0x017e3f, 0x017e4f, 0x017e5f, // animated BG block 3 frame 1
                        0x017e40, 0x017e50, 0x017e60, // animated BG block 3 frame 2
                        0x017e41, 0x017e51, 0x017e61},// animated BG block 3 frame 3
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.White, EColorsHex.PastelBlue, EColorsHex.LightCyan,EColorsHex.NearWhite, EColorsHex.LightBlue,EColorsHex.MediumBlue,EColorsHex.NearWhite,EColorsHex.LightBlue,EColorsHex.MediumBlue,
                            EColorsHex.White, EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.PastelBlue,  EColorsHex.LightBlue,  EColorsHex.LightBlue,
                            EColorsHex.LightCyan,  EColorsHex.MediumBlue,  EColorsHex.MediumBlue,
                            EColorsHex.NearWhite,  EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.LightBlue,  EColorsHex.PastelBlue,  EColorsHex.LightBlue,
                            EColorsHex.MediumBlue,  EColorsHex.LightCyan,  EColorsHex.MediumBlue,
                            EColorsHex.NearWhite,  EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.LightBlue,  EColorsHex.LightBlue,  EColorsHex.PastelBlue,
                            EColorsHex.MediumBlue,  EColorsHex.MediumBlue,  EColorsHex.LightCyan,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.PastelPurple,EColorsHex.LightPink,EColorsHex.NearWhite,EColorsHex.LightPurple,EColorsHex.Purple,EColorsHex.NearWhite,EColorsHex.LightPurple,EColorsHex.Purple,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.PastelPurple,EColorsHex.LightPurple,  EColorsHex.LightPurple,
                            EColorsHex.LightPink,   EColorsHex.Purple,  EColorsHex.Purple,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.LightPurple, EColorsHex.PastelPurple,  EColorsHex.LightPurple,
                            EColorsHex.Purple,      EColorsHex.LightPink,  EColorsHex.Purple,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.LightPurple, EColorsHex.LightPurple,  EColorsHex.PastelPurple,
                            EColorsHex.Purple,      EColorsHex.Purple,  EColorsHex.LightPink,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Taupe,EColorsHex.LightOrange,EColorsHex.NearWhite,EColorsHex.Orange,EColorsHex.Red,EColorsHex.NearWhite,EColorsHex.LightOrange,EColorsHex.Orange,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.Taupe,       EColorsHex.LightOrange,  EColorsHex.Orange,
                            EColorsHex.LightOrange, EColorsHex.Orange,  EColorsHex.Red,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.Orange,      EColorsHex.Taupe,  EColorsHex.LightOrange,
                            EColorsHex.Red,         EColorsHex.LightOrange,  EColorsHex.Orange,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.LightOrange, EColorsHex.Orange,  EColorsHex.Taupe,
                            EColorsHex.Orange,      EColorsHex.Red,  EColorsHex.LightOrange,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.PastelYellow,EColorsHex.Yellow,EColorsHex.NearWhite,EColorsHex.GoldenRod,EColorsHex.Brown,EColorsHex.NearWhite,EColorsHex.Yellow,EColorsHex.GoldenRod,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.PastelYellow,EColorsHex.Yellow,  EColorsHex.GoldenRod,
                            EColorsHex.Yellow,      EColorsHex.GoldenRod,  EColorsHex.Brown,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.GoldenRod,   EColorsHex.PastelYellow,  EColorsHex.Yellow,
                            EColorsHex.Brown,       EColorsHex.Yellow,  EColorsHex.GoldenRod,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.Yellow,      EColorsHex.GoldenRod,  EColorsHex.PastelYellow,
                            EColorsHex.GoldenRod,   EColorsHex.Brown,  EColorsHex.Yellow,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.PastelGreen,EColorsHex.LightGreen,EColorsHex.NearWhite,EColorsHex.Green,EColorsHex.ForestGreen,EColorsHex.NearWhite,EColorsHex.LightGreen,EColorsHex.Green,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.PastelGreen, EColorsHex.LightGreen,  EColorsHex.Green,
                            EColorsHex.LightGreen,  EColorsHex.Green,  EColorsHex.ForestGreen,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.Green,       EColorsHex.PastelGreen,  EColorsHex.LightGreen,
                            EColorsHex.ForestGreen, EColorsHex.LightGreen,  EColorsHex.Green,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.LightGreen,  EColorsHex.Green,  EColorsHex.PastelGreen,
                            EColorsHex.Green,       EColorsHex.ForestGreen,  EColorsHex.LightGreen,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.PastelCyan,EColorsHex.Lime,EColorsHex.NearWhite,EColorsHex.Moss,EColorsHex.DarkGreen,EColorsHex.NearWhite,EColorsHex.Lime,EColorsHex.Moss,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.PastelCyan,  EColorsHex.Lime,  EColorsHex.Moss,
                            EColorsHex.Lime,        EColorsHex.Moss,  EColorsHex.DarkGreen,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.Moss,        EColorsHex.PastelCyan,  EColorsHex.Lime,
                            EColorsHex.DarkGreen,   EColorsHex.Lime,  EColorsHex.Moss,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.Lime,        EColorsHex.Moss,  EColorsHex.PastelCyan,
                            EColorsHex.Moss,        EColorsHex.DarkGreen,  EColorsHex.Lime,
                        },
                        new EColorsHex[] {
                            EColorsHex.White,EColorsHex.Black3,EColorsHex.LightGray,EColorsHex.NearWhite,EColorsHex.Black3,EColorsHex.DarkRed,EColorsHex.NearWhite,EColorsHex.Black3,EColorsHex.Gray,
                            EColorsHex.White,       EColorsHex.NearWhite,  EColorsHex.NearWhite,
                            EColorsHex.Black3,      EColorsHex.Black3,  EColorsHex.Black3,
                            EColorsHex.LightGray,   EColorsHex.Gray,  EColorsHex.DarkRed,
                            EColorsHex.NearWhite,   EColorsHex.White,  EColorsHex.NearWhite,
                            EColorsHex.Black3,      EColorsHex.Black3,  EColorsHex.Black3,
                            EColorsHex.DarkRed,     EColorsHex.LightGray,  EColorsHex.Gray,
                            EColorsHex.NearWhite,   EColorsHex.NearWhite,  EColorsHex.White,
                            EColorsHex.Black3,      EColorsHex.Black3,  EColorsHex.Black3,
                            EColorsHex.Gray,        EColorsHex.DarkRed,  EColorsHex.LightGray,
                        },
                    }
                },
                // TODO: Comment later, missing a color in boss corridor
                new ColorSet() {
                    addresses = new int[] { 0x01fe13, 0x01fe14, 0x03b63a, 0x03b63b, 0x03b642, 0x03b643, 0x03b646, 0x03b647, 0x03b64a, 0x03b64b, 0x03b64e, 0x03b64f, 0x039188, 0x039189, 0x03918c, 0x03918d, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Clash | Border1 | Default
                        new EColorsHex[] {  EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod, EColorsHex.PastelLemon, EColorsHex.GoldenRod,},
                        // Clash | Border1 | Blue
                        new EColorsHex[] {  EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue, EColorsHex.MediumBlue, EColorsHex.RoyalBlue,},
                        // Clash | Border1 | Orange
                        new EColorsHex[] {  EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange, EColorsHex.YellowOrange, EColorsHex.Orange,},
                        // Clash | Border1 | Green
                        new EColorsHex[] {  EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen, EColorsHex.Lime, EColorsHex.ForestGreen,},
                        // Clash | Border1 | Red Black
                        new EColorsHex[] {  EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Red,},
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x01fe15,0x01fe17,0x03b63c,0x03b644,0x03b648,0x03b64c,0x03b650,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Clash | Background | Default
                        new EColorsHex[] {  EColorsHex.Blue, EColorsHex.Blue, EColorsHex.RoyalBlue, EColorsHex.RoyalBlue, EColorsHex.Black2, EColorsHex.Black2, EColorsHex.Black2,},
                        // Clash | Background | Yellow
                        new EColorsHex[] {  EColorsHex.Yellow, EColorsHex.Yellow, EColorsHex.Brown, EColorsHex.Brown, EColorsHex.Black2, EColorsHex.Black2, EColorsHex.Black2,},
                        // Clash | Background | Orange
                        new EColorsHex[] {  EColorsHex.Orange, EColorsHex.Orange, EColorsHex.Red, EColorsHex.Red, EColorsHex.Black2, EColorsHex.Black2, EColorsHex.Black2,},
                        // Clash | Background | Green
                        new EColorsHex[] {  EColorsHex.Lime, EColorsHex.Lime, EColorsHex.Moss, EColorsHex.Moss, EColorsHex.Black2, EColorsHex.Black2, EColorsHex.Black2,},
                        // Clash | Background | Purple
                        new EColorsHex[] {  EColorsHex.LightPink, EColorsHex.LightPink, EColorsHex.DarkMagenta, EColorsHex.DarkMagenta, EColorsHex.Black2, EColorsHex.Black2, EColorsHex.Black2,}
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x01fe18,0x01fe19, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Clash | Doodads | Default
                        new EColorsHex[] {  EColorsHex.YellowOrange, EColorsHex.NearWhite,},
                        // Clash | Doodads | Green
                        new EColorsHex[] {  EColorsHex.Green, EColorsHex.NearWhite,},
                        // Clash | Doodads | Teal
                        new EColorsHex[] {  EColorsHex.Teal, EColorsHex.NearWhite,},
                        // Clash | Doodads | Purple
                        new EColorsHex[] {  EColorsHex.Purple, EColorsHex.NearWhite,},
                        // Clash | Doodads | Red
                        new EColorsHex[] {  EColorsHex.Crimson, EColorsHex.NearWhite,},
                        // Clash | Doodads | Gray
                        new EColorsHex[] {  EColorsHex.Gray, EColorsHex.NearWhite,},
                    }
                },
                
            };

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (ColorSet set in colorSets)
                {
                    set.RandomizeAndWrite(stream, Random);
                }
            }
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
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily345);
            newBGMOrder.Add(robos[0]);
            newBGMOrder.Add(robos[1]);

            // Randomize tracks and write to ROM
            newBGMOrder.Shuffle(Random);
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = 0x0381E0; // Heatman BGM ID, both J and U
                foreach (EMusicID bgm in newBGMOrder)
                {
                    stream.WriteByte((byte)bgm);
                }
            }
        }

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

            var newWeaponOrder = new List<ERMWeaponValue>()
            {
                ERMWeaponValue.HeatMan,
                ERMWeaponValue.AirMan,
                ERMWeaponValue.WoodMan,
                ERMWeaponValue.BubbleMan,
                ERMWeaponValue.QuickMan,
                ERMWeaponValue.FlashMan,
                ERMWeaponValue.MetalMan,
                ERMWeaponValue.CrashMan
            }.Select(s => (byte)s).ToList();
            
            newWeaponOrder.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Create table for which weapon is awarded by which robot master
                // This also affects which portrait is blacked out on the stage select
                // This also affects which teleporter deactivates after defeating a Wily 5 refight boss
                stream.Position = (long) ERMStageWeaponAddress.HeatMan;// 0x03c289;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
                }

                // Create a copy of the default weapon order table to be used by teleporter function
                // This is needed to fix teleporters breaking from the new weapon order.
                //stream.Position = 0x03f2D0; // Unused space at end of bank
                stream.Position = 0x03f310; // Unused space at end of bank
                stream.WriteByte((byte) ERMWeaponValue.HeatMan);
                stream.WriteByte((byte) ERMWeaponValue.AirMan);
                stream.WriteByte((byte) ERMWeaponValue.WoodMan);
                stream.WriteByte((byte) ERMWeaponValue.BubbleMan);
                stream.WriteByte((byte) ERMWeaponValue.QuickMan);
                stream.WriteByte((byte) ERMWeaponValue.FlashMan);
                stream.WriteByte((byte) ERMWeaponValue.MetalMan);
                stream.WriteByte((byte) ERMWeaponValue.CrashMan);

                // Change function to call $f300 instead of $c279 when looking up defeated refight boss to
                // get our default weapon table, fixing the teleporter softlock
                stream.Position = 0x03843b;
                stream.WriteByte(0x00);
                stream.WriteByte(0xf3);

                // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
                stream.Position = (long) ERMStageSelect.FirstStageInMemory; // 0x0346E1;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
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
        /// Modify the damage values of each weapon against each Robot Master for Rockman 2 (J).
        /// </summary>
        private static void RandomWeaknesses()
        {
            List<WeaponTable> Weapons = new List<WeaponTable>();

            Weapons.Add(new WeaponTable()
            {
                Name = "Buster",
                ID = 0,
                Address = ERMWeaponAddress.Buster,
                RobotMasters = new int[8] { 2,2,1,1,2,2,1,1 }
                // Heat = 2,
                // Air = 2,
                // Wood = 1,
                // Bubble = 1,
                // Quick = 2,
                // Flash = 2,
                // Metal = 1,
                // Clash = 1,
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Atomic Fire",
                ID = 1,
                Address = ERMWeaponAddress.AtomicFire,
                // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Air Shooter",
                ID = 2,
                Address = ERMWeaponAddress.AirShooter,
                RobotMasters = new int[8] {2, 0, 4, 0, 2, 0, 0, 0x0A }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Leaf Shield",
                ID = 3,
                Address = ERMWeaponAddress.LeafShield,
                RobotMasters = new int[8] { 0, 8, 0xFF, 0, 0, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Bubble Lead",
                ID = 4,
                Address = ERMWeaponAddress.BubbleLead,
                RobotMasters = new int[8] { 6, 0, 0, 0xFF, 0, 2, 0, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Quick Boomerang",
                ID = 5,
                Address = ERMWeaponAddress.QuickBoomerang,
                RobotMasters = new int[8] { 2, 2, 0, 2, 0, 0, 4, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Time Stopper",
                ID = 6,
                Address = ERMWeaponAddress.TimeStopper,
                // NOTE: These values affect damage per tick
                RobotMasters = new int[8] { 0, 0, 0, 0, 1, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Metal Blade",
                ID = 7,
                Address = ERMWeaponAddress.MetalBlade,
                RobotMasters = new int[8] { 1, 0, 2, 4, 0, 4, 0x0E, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Clash Bomber",
                ID = 8,
                Address = ERMWeaponAddress.ClashBomber,
                RobotMasters = new int[8] { 0xFF, 0, 2, 2, 4, 3, 0, 0 }
            });

            foreach (WeaponTable weapon in Weapons)
            {
                weapon.RobotMasters.Shuffle(Random);
            }

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (WeaponTable weapon in Weapons)
                {
                    stream.Position = (long)weapon.Address;
                    for (int i = 0; i < 8; i++)
                    { 
                        stream.WriteByte((byte)weapon.RobotMasters[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Identical to RandomWeaknesses() but using Mega Man 2 (U).nes offsets
        /// </summary>
        private static void RandomWeaknessesMM2()
        {
            List<WeaponTable> Weapons = new List<WeaponTable>();

            Weapons.Add(new WeaponTable()
            {
                Name = "Buster",
                ID = 0,
                Address = ERMWeaponAddress.Eng_Buster,
                RobotMasters = new int[8] { 2, 2, 1, 1, 2, 2, 1, 1 }
                // Heat = 2,
                // Air = 2,
                // Wood = 1,
                // Bubble = 1,
                // Quick = 2,
                // Flash = 2,
                // Metal = 1,
                // Clash = 1,
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Atomic Fire",
                ID = 1,
                Address = ERMWeaponAddress.Eng_AtomicFire,
                // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Air Shooter",
                ID = 2,
                Address = ERMWeaponAddress.Eng_AirShooter,
                RobotMasters = new int[8] { 2, 0, 4, 0, 2, 0, 0, 0x0A }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Leaf Shield",
                ID = 3,
                Address = ERMWeaponAddress.Eng_LeafShield,
                RobotMasters = new int[8] { 0, 8, 0xFF, 0, 0, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Bubble Lead",
                ID = 4,
                Address = ERMWeaponAddress.Eng_BubbleLead,
                RobotMasters = new int[8] { 6, 0, 0, 0xFF, 0, 2, 0, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Quick Boomerang",
                ID = 5,
                Address = ERMWeaponAddress.Eng_QuickBoomerang,
                RobotMasters = new int[8] { 2, 2, 0, 2, 0, 0, 4, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Time Stopper",
                ID = 6,
                Address = ERMWeaponAddress.Eng_TimeStopper,
                // NOTE: These values affect damage per tick
                RobotMasters = new int[8] { 0, 0, 0, 0, 1, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Metal Blade",
                ID = 7,
                Address = ERMWeaponAddress.Eng_MetalBlade,
                RobotMasters = new int[8] { 1, 0, 2, 4, 0, 4, 0x0E, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Clash Bomber",
                ID = 8,
                Address = ERMWeaponAddress.Eng_ClashBomber,
                RobotMasters = new int[8] { 0xFF, 0, 2, 2, 4, 3, 0, 0 }
            });

            foreach (WeaponTable weapon in Weapons)
            {
                weapon.RobotMasters.Shuffle(Random);
            }

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (WeaponTable weapon in Weapons)
                {
                    stream.Position = (long) weapon.Address;
                    for (int i = 0; i < 8; i++)
                    {
                        stream.WriteByte((byte)weapon.RobotMasters[i]);
                    }
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
            
            List<StageFromSelect> StageSelect = new List<StageFromSelect>();

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
                PortraitDestinationNew = 0,
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
