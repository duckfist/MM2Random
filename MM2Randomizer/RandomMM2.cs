using MM2Randomizer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                if (Settings.IsColorsRandom)
                {
                RandomColors();
                }
                if (Settings.IsWeaknessRandom)
                {
                    // Offsets are different in Rockman 2 and Mega Man 2
                    if (Settings.IsJapanese)
                        RandomWeaknesses();
                    else
                        RandomWeaknessesMM2();
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

        private static void RandomColors()
        {
            List<ColorSet> colorSets = new List<ColorSet>()
            {
                new ColorSet() {
                    addresses = new int[] { 0x3e3f, 0x3e4f, 0x3e5f,
                                            0x3e40, 0x3e50, 0x3e60,
                                            0x3e41, 0x3e51, 0x3e61 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Heat | River | Default Orange
                            0x36, 0x26, 0x16,
                            0x26, 0x16, 0x36,
                            0x16, 0x36, 0x26,
                        },
                        new byte[] { // Heat | River | Green
                            0x2a, 0x1a, 0x0a,
                            0x1a, 0x0a, 0x2a,
                            0x0a, 0x2a, 0x1a,
                        },
                        new byte[] { // Heat | River | Yellow
                            0x28, 0x18, 0x08,
                            0x18, 0x08, 0x28,
                            0x08, 0x28, 0x18,
                        },
                        new byte[] { // Heat | River | Purple
                            0x24, 0x14, 0x04,
                            0x14, 0x04, 0x24,
                            0x04, 0x24, 0x14,
                        }
                    }
                },
                new ColorSet()
            {
                    addresses = new int[] { 0x3e3b, 0x3e4b, 0x3e5b,
                                            0x3e3c, 0x3e4c, 0x3e5c,
                                            0x3e3d, 0x3e4d, 0x3e5d },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Heat | Background | Default Red
                            0x16, 0x16, 0x16,
                            0x05, 0x05, 0x05,
                            0x07, 0x07, 0x07,
                        },
                        new byte[] { // Heat | Background | Purple 
                            0x14, 0x14, 0x14,
                            0x04, 0x04, 0x04,
                            0x03, 0x03, 0x03,
                        },
                        new byte[] { // Heat | Background | Gold Gray 
                            0x2d, 0x2d, 0x2d,
                            0x18, 0x18, 0x18,
                            0x08, 0x08, 0x08,
                        },
                        new byte[] { // Heat | Background | Deep Blue 
                            0x0c, 0x0c, 0x0c,
                            0x01, 0x01, 0x01,
                            0x12, 0x12, 0x12,
                        },
                        new byte[] { // Heat | Background | Dark Green
                            0x0b, 0x0b, 0x0b,
                            0x1d, 0x1d, 0x1d,
                            0x09, 0x09, 0x09,
                        },
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x3e33, 0x3e43, 0x3e53,
                                            0x3e34, 0x3e44, 0x3e54,
                                            0x3e35, 0x3e45, 0x3e55 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Heat | Foreground | Default Pink
                            0x36, 0x36, 0x36,
                            0x26, 0x26, 0x26,
                            0x15, 0x15, 0x15,
                        },
                        new byte[] { // Heat | Foreground | Purple 
                            0x34, 0x34, 0x34,
                            0x24, 0x24, 0x24,
                            0x13, 0x13, 0x13,
                        },
                        new byte[] { // Heat | Foreground | Light Green 
                            0x3a, 0x3a, 0x3a,
                            0x2a, 0x2a, 0x2a,
                            0x19, 0x19, 0x19,
                        },
                        new byte[] { // Heat | Foreground | Light Blue 
                            0x32, 0x32, 0x32,
                            0x22, 0x22, 0x22,
                            0x11, 0x11, 0x11,
                        },
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x3e37, 0x3e47, 0x3e57,
                                            0x3e38, 0x3e48, 0x3e58,
                                            0x3e39, 0x3e49, 0x3e59 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Heat | Foreground2 | Default Light Gray
                            0x30, 0x30, 0x30,
                            0x10, 0x10, 0x10,
                            0x00, 0x00, 0x00,
                        },
                        new byte[] { // Heat | Foreground2 | Light Orange
                            0x30, 0x30, 0x30,
                            0x37, 0x37, 0x37,
                            0x27, 0x27, 0x27,
                        },
                        new byte[] { // Heat | Foreground2 | Light Blue
                            0x30, 0x30, 0x30,
                            0x31, 0x31, 0x31,
                            0x21, 0x21, 0x21,
                        },
                        new byte[] { // Heat | Foreground2 | Dark
                            0x10, 0x10, 0x10,
                            0x00, 0x00, 0x00,
                            0x08, 0x08, 0x08,
                        },
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x7e37, 0x7e47, 0x7e57, 0x7e67,
                                            0x7e38, 0x7e48, 0x7e58, 0x7e68,
                                            0x7e39, 0x7e49, 0x7e59, 0x7e69 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Air | Platforms | Default
                            0x30, 0x30, 0x30, 0x30,
                            0x15, 0x15, 0x15, 0x15,
                            0x0F, 0x0F, 0x0F, 0x0F,
                        },
                        new byte[] { // Air | Platforms | Orange
                            0x30, 0x30, 0x30, 0x30,
                            0x16, 0x16, 0x16, 0x16,
                            0x0F, 0x0F, 0x0F, 0x0F,
                        },
                        new byte[] { // Air | Platforms | Yellow
                            0x30, 0x30, 0x30, 0x30,
                            0x28, 0x28, 0x28, 0x28,
                            0x0F, 0x0F, 0x0F, 0x0F,
                        },
                        new byte[] { // Air | Platforms | Green
                            0x30, 0x30, 0x30, 0x30,
                            0x19, 0x19, 0x19, 0x19,
                            0x0F, 0x0F, 0x0F, 0x0F,
                        },
                        new byte[] { // Air | Platforms | Blue
                            0x30, 0x30, 0x30, 0x30,
                            0x22, 0x22, 0x22, 0x22,
                            0x0F, 0x0F, 0x0F, 0x0F,
                        },
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x7e33, 0x7e43, 0x7e53, 0x7e63,
                                            0x7e34, 0x7e44, 0x7e54, 0x7e64,
                                            0x7e35, 0x7e45, 0x7e55, 0x7e65 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Air | Clouds | Default
                            0x21, 0x31, 0x30, 0x31,
                            0x31, 0x30, 0x30, 0x30,
                            0x30, 0x30, 0x30, 0x30,
                        },
                        new byte[] { // Air | Clouds | Light Gray Dark Red
                            0x10, 0x00, 0x07, 0x00,
                            0x10, 0x10, 0x00, 0x10,
                            0x10, 0x10, 0x10, 0x10,
                        },
                    }
                },
                new ColorSet() {
                // $0366 only
                    addresses = new int[] { 0x7e22 },
                    ColorBytes = new List<byte[]>() {
            // Air | Sky | Default
                        new byte[] { 0x21 },
            // Air | Sky | Pink
                        new byte[] { 0x23 },
            // Air | Sky | Dark Orange
                        new byte[] { 0x26 },
            // Air | Sky | Light Orange
                        new byte[] { 0x27 },
            // Air | Sky | Yellow
                        new byte[] { 0x28 },
            // Air | Sky | Light Green
                        new byte[] { 0x2B },
            // Air | Sky | Blue
                        new byte[] { 0x01 },
            // Air | Sky | Dark Red
                        new byte[] { 0x01 },
            // Air | Sky | Dark Green
                        new byte[] { 0x0B },
            // Air | Sky | Black
                        new byte[] { 0x1D },
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x017e34, 0x017e44, 0x017e54 ,
                                            0x017e35, 0x017e45, 0x017e55 },
                    ColorBytes = new List<byte[]>() {
                        new byte[] { // Flash | Background | Default
                            0x12, 0x12, 0x12,
                            0x02, 0x02, 0x02,
                        },
                        new byte[] { // Flash | Background | Magenta
                            0x14, 0x14, 0x14,
                            0x04, 0x04, 0x04,
                        },
                        new byte[] { // Flash | Background | Orange
                            0x16, 0x16, 0x16,
                            0x06, 0x06, 0x06,
                        },
                     new byte[] { // Flash | Background | Yellow
                            0x18, 0x18, 0x18,
                            0x08, 0x08, 0x08,
                        },
                        new byte[] { // Flash | Background | Green
                            0x1a, 0x1a, 0x1a,
                            0x0a, 0x0a, 0x0a,
                        },
                        new byte[] { // Flash | Background | Black
                            0x00, 0x00, 0x00,
                            0x1d, 0x1d, 0x1d,
                        },
                    }
                },
                new ColorSet() {
                // Note: 3 color sets are used for the flashing blocks.
                // I've kept them grouped here for common color themes.
                    addresses = new int[] { 0x017e37, 0x017e47, 0x017e57,
                                            0x017e38, 0x017e48, 0x017e58,
                                            0x017e39, 0x017e49, 0x017e59,
                                            0x017e3b, 0x017e4b, 0x017e5b,
                                            0x017e3c, 0x017e4c, 0x017e5c,
                                            0x017e3d, 0x017e4d, 0x017e5d,
                                            0x017e3f, 0x017e4f, 0x017e5f,
                                            0x017e40, 0x017e50, 0x017e60,
                                            0x017e41, 0x017e51, 0x017e61},
                    ColorBytes = new List<byte[]>() {
                         new byte[] { // Flash | Foreground | Default
                            0x30, 0x20, 0x20,
                            0x31, 0x21, 0x21,
                            0x2C, 0x11, 0x11,
                            0x20, 0x30, 0x20,
                            0x21, 0x31, 0x21,
                            0x11, 0x2C, 0x11,
                            0x20, 0x20, 0x30,
                            0x21, 0x21, 0x31,
                            0x11, 0x11, 0x2c,
                        },
                        new byte[] { // Flash | Foreground | Magenta
                            0x30, 0x20, 0x20,
                            0x33, 0x23, 0x23,
                            0x24, 0x13, 0x13,
                            0x20, 0x30, 0x20,
                            0x23, 0x33, 0x23,
                            0x13, 0x24, 0x13,
                            0x20, 0x20, 0x30,
                            0x23, 0x23, 0x33,
                            0x13, 0x13, 0x24,
                        },
                        new byte[] { // Flash | Foreground | Orange
                            0x30, 0x20, 0x20,
                            0x36, 0x26, 0x16,
                            0x26, 0x16, 0x06,
                            0x20, 0x30, 0x20,
                            0x16, 0x36, 0x26,
                            0x06, 0x26, 0x16,
                            0x20, 0x20, 0x30,
                            0x26, 0x16, 0x36,
                            0x16, 0x06, 0x26,
                        },
                        new byte[] { // Flash | Foreground | Yellow
                            0x30, 0x20, 0x20,
                            0x38, 0x28, 0x18,
                            0x28, 0x18, 0x08,
                            0x20, 0x30, 0x20,
                            0x18, 0x38, 0x28,
                            0x08, 0x28, 0x18,
                            0x20, 0x20, 0x30,
                            0x28, 0x18, 0x38,
                            0x18, 0x08, 0x28,
                        },
                        new byte[] { // Flash | Foreground | Green
                            0x30, 0x20, 0x20,
                            0x3A, 0x2A, 0x1A,
                            0x2A, 0x1A, 0x0A,
                            0x20, 0x30, 0x20,
                            0x1A, 0x3A, 0x2A,
                            0x0A, 0x2A, 0x1A,
                            0x20, 0x20, 0x30,
                            0x2A, 0x1A, 0x3A,
                            0x1A, 0x0A, 0x2A,
                        },
                        new byte[] { // Flash | Foreground | Turquoise
                            0x30, 0x20, 0x20,
                            0x3b, 0x2b, 0x1b,
                            0x2b, 0x1b, 0x0b,
                            0x20, 0x30, 0x20,
                            0x1b, 0x3b, 0x2b,
                            0x0b, 0x2b, 0x1b,
                            0x20, 0x20, 0x30,
                            0x2b, 0x1b, 0x3b,
                            0x1b, 0x0b, 0x2b,
                        },
                        new byte[] { // Flash | Foreground | Black and Red
                            0x30, 0x20, 0x20,
                            0x1d, 0x1d, 0x1d,
                            0x10, 0x00, 0x07,
                            0x20, 0x30, 0x20,
                            0x1d, 0x1d, 0x1d,
                            0x07, 0x10, 0x00,
                            0x20, 0x20, 0x30,
                            0x1d, 0x1d, 0x1d,
                            0x00, 0x07, 0x10,
                        },
                    }
                },
            // TODO: Comment later, missing a color in boss corridor
                new ColorSet() {
                    addresses = new int[] { 0x01fe13, 0x01fe14, 0x03b63a, 0x03b63b, 0x03b642, 0x03b643, 0x03b646, 0x03b647, 0x03b64a, 0x03b64b, 0x03b64e, 0x03b64f, 0x039188, 0x039189, 0x03918c, 0x03918d, },
                    ColorBytes = new List<byte[]>()
            {
            // Clash | Border1 | Default
                        new byte[] { 0x39,0x18,0x39,0x18,0x39,0x18,0x39,0x18,0x39,0x18,0x39,0x18,0x39,0x18,0x39,0x18,},

            // Clash | Border1 | Blue
                        new byte[] { 0x11,0x01,0x11,0x01,0x11,0x01,0x11,0x01,0x11,0x01,0x11,0x01,0x11,0x01,0x11,0x01,},

            // Clash | Border1 | Orange
                        new byte[] { 0x27,0x16,0x27,0x16,0x27,0x16,0x27,0x16,0x27,0x16,0x27,0x16,0x27,0x16,0x27,0x16,},

            // Clash | Border1 | Green
                        new byte[] { 0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,0x2b,0x0a,},

            // Clash | Border1 | Red Black
                        new byte[] { 0x0f,0x06,0x0f,0x06,0x0f,0x06,0x0f,0x06,0x0f,0x06,0x0f,0x06,0x0f,0x06,0x0f,0x06,},
                    }
                },
                new ColorSet()
                {
                    addresses = new int[] { 0x01fe15,0x01fe17,0x03b63c,0x03b644,0x03b648,0x03b64c,0x03b650,},
                    ColorBytes = new List<byte[]>()
            {
            // Clash | Background | Default
                        new byte[] { 0x12,0x12,0x01,0x01,0x0f,0x0f,0x0f,},
            // Clash | Background | Yellow
                        new byte[] { 0x28,0x28,0x08,0x08,0x0F,0x0F,0x0F,},
            // Clash | Background | Orange
                        new byte[] { 0x16,0x16,0x06,0x06,0x0F,0x0F,0x0F,},
            // Clash | Background | Green
                        new byte[] { 0x2b,0x2b,0x1b,0x1b,0x0F,0x0F,0x0F,},
            // Clash | Background | Purple
                        new byte[] { 0x24,0x24,0x04,0x04,0x0F,0x0F,0x0F,}
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0x01fe18,0x01fe19, },
                    ColorBytes = new List<byte[]>() {
                        // Clash | Doodads | Default
                        new byte[] { 0x27,0x20,},
            // Clash | Doodads | Green
                        new byte[] { 0x1A,0x20,},
            // Clash | Doodads | Teal
                        new byte[] { 0x1C,0x20,},
            // Clash | Doodads | Purple
                        new byte[] { 0x13,0x20,},
            // Clash | Doodads | Red
                        new byte[] { 0x05,0x20,},
            // Clash | Doodads | Gray
                        new byte[] { 0x00,0x20,},
                    }
                },
                new ColorSet() {
                    addresses = new int[] { 0xbe13, 0xbe14, },
                    ColorBytes = new List<byte[]>() {
            // Wood | Leaves | Default
                        new byte[] { 0x29,0x19,},
            // Wood | Leaves | Blue
                        new byte[] { 0x11,0x01,},
                        // Wood | Leaves | Red
                        new byte[] { 0x16,0x06,},
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe17, 0xbe18, },
                    ColorBytes = new List<byte[]>() {
            // Wood | Trunk | Default
                        new byte[] { 0x28, 0x18 },
            // Wood | Trunk | Purple
                        new byte[] { 0x23, 0x13 },
            // Wood | Trunk | Pink
                        new byte[] { 0x25, 0x15 },
            // Wood | Trunk | Orange
                        new byte[] { 0x27, 0x17 },
            // Wood | Trunk | Green
                        new byte[] { 0x2A, 0x1A },
            // Wood | Trunk | Teal
                        new byte[] { 0x2C, 0x1C },
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe1b,0xbe1c,0xbe1d,},
                    ColorBytes = new List<byte[]>() {
            // Wood | Floor | Default
                        new byte[] { 0x27,0x17,0x07,},
            // Wood | Floor | Yellow
                        new byte[] { 0x28,0x18,0x08,},
            // Wood | Floor | Green
                        new byte[] { 0x2a,0x1a,0x0a,},
            // Wood | Floor | Teal
                        new byte[] { 0x2c,0x1c,0x0c,},
            // Wood | Floor | Purple
                        new byte[] { 0x23,0x13,0x03,},
            // Wood | Floor | Gray
                        new byte[] { 0x20,0x10,0x0f,},
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe1f, 0x03a118, },
                    ColorBytes = new List<byte[]>() {
            // Wood | UndergroundBG | Default
                        new byte[] { 0x08, 0x08 },
            // Wood | UndergroundBG | Dark Purple
                        new byte[] { 0x04, 0x04 },
            // Wood | UndergroundBG | Dark Red
                        new byte[] { 0x05, 0x05 },
            // Wood | UndergroundBG | Dark Green
                        new byte[] { 0x09, 0x09 },
            // Wood | UndergroundBG | Dark Teal
                        new byte[] { 0x0b, 0x0b },
            // Wood | UndergroundBG | Dark Blue1
                        new byte[] { 0x0c, 0x0c },
            // Wood | UndergroundBG | Dark Blue2
                        new byte[] { 0x01, 0x01 },
                    }
                },

                new ColorSet() {
                    addresses = new int[] { 0xbe15,0xbe19,},
                    ColorBytes = new List<byte[]>() {
            // Wood | SkyBG | Default
                        new byte[] { 0x2c, 0x2c },
            // Wood | SkyBG | Light Green
                        new byte[] { 0x2a ,0x2a },
            // Wood | SkyBG | Blue
                        new byte[] { 0x12, 0x12 },
            // Wood | SkyBG | Dark Purple
                        new byte[] { 0x03, 0x03 },
            // Wood | SkyBG | Dark Red
                        new byte[] { 0x05, 0x05 },
            // Wood | SkyBG | Light Yellow
                        new byte[] { 0x38, 0x38 },
            // Wood | SkyBG | Black
                        new byte[] { 0x0f, 0x0f },
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
                stream.Position = 0x03f2D0; // Unused space at end of bank
                stream.WriteByte(0x01);
                stream.WriteByte(0x02);
                stream.WriteByte(0x04);
                stream.WriteByte(0x08);
                stream.WriteByte(0x10);
                stream.WriteByte(0x20);
                stream.WriteByte(0x40);
                stream.WriteByte(0x80);

                // Change function to call $f2c0 instead of $c279 when looking up defeated refight boss to
                // get our default weapon table, fixing the teleporter softlock
                stream.Position = 0x03843b;
                stream.WriteByte(0xc0);
                stream.WriteByte(0xf2);

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

            List<byte> newItemOrder = new List<byte>();
            for (byte i = 0; i < 5; i++) newItemOrder.Add((byte) EItemNumber.None);
            newItemOrder.Add((byte) EItemNumber.One);
            newItemOrder.Add((byte) EItemNumber.Two);
            newItemOrder.Add((byte) EItemNumber.Three);

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
