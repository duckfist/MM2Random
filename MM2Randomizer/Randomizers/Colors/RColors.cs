using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Colors
{
    /// <summary>
    /// Stage Color Palette Randomizer
    /// </summary>
    public class RColors
    {
        List<ColorSet> StagesColorSets { get; set; }
        List<ColorSet> WeaponColorSets { get; set; }
        List<ColorSet> StageSelectColorSets { get; set; }

        public static int MegaManColorAddressU = 0x03d314;
        public static int MegaManColorAddressJ = 0x03d311;

        public RColors()
        {
            RandomizeStageColors();

            RandomizeWeaponColors();

            RandomizeRobotMasterColors();

            RandomizeStageSelectColors();
        }

        private void RandomizeStageSelectColors()
        {
            StageSelectColorSets = new List<ColorSet>()
            {
                new ColorSet() { // Stage select background color sets
                    addresses = new int[] {
                        0x0344ab, 0x0344ac,
                        0x0344af, 0x0344b0,
                        0x0344b3, 0x0344b4,
                        0x0344b7, 0x0344b8,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] { // Default light blue
                            (EColorsHex)0x11,(EColorsHex)0x2C,
                            (EColorsHex)0x11,(EColorsHex)0x2C,
                            (EColorsHex)0x11,(EColorsHex)0x2C,
                            (EColorsHex)0x11,(EColorsHex)0x2C,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x12,(EColorsHex)0x21,
                            (EColorsHex)0x12,(EColorsHex)0x21,
                            (EColorsHex)0x12,(EColorsHex)0x21,
                            (EColorsHex)0x12,(EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x13,(EColorsHex)0x22,
                            (EColorsHex)0x13,(EColorsHex)0x22,
                            (EColorsHex)0x13,(EColorsHex)0x22,
                            (EColorsHex)0x13,(EColorsHex)0x22,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x15,(EColorsHex)0x24,
                            (EColorsHex)0x15,(EColorsHex)0x24,
                            (EColorsHex)0x15,(EColorsHex)0x24,
                            (EColorsHex)0x15,(EColorsHex)0x24,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x16,(EColorsHex)0x25,
                            (EColorsHex)0x16,(EColorsHex)0x25,
                            (EColorsHex)0x16,(EColorsHex)0x25,
                            (EColorsHex)0x16,(EColorsHex)0x25,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x17,(EColorsHex)0x26,
                            (EColorsHex)0x17,(EColorsHex)0x26,
                            (EColorsHex)0x17,(EColorsHex)0x26,
                            (EColorsHex)0x17,(EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x18,(EColorsHex)0x27,
                            (EColorsHex)0x18,(EColorsHex)0x27,
                            (EColorsHex)0x18,(EColorsHex)0x27,
                            (EColorsHex)0x18,(EColorsHex)0x27,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x19,(EColorsHex)0x28,
                            (EColorsHex)0x19,(EColorsHex)0x28,
                            (EColorsHex)0x19,(EColorsHex)0x28,
                            (EColorsHex)0x19,(EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x1b,(EColorsHex)0x2a,
                            (EColorsHex)0x1b,(EColorsHex)0x2a,
                            (EColorsHex)0x1b,(EColorsHex)0x2a,
                            (EColorsHex)0x1b,(EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x1c,(EColorsHex)0x2b,
                            (EColorsHex)0x1c,(EColorsHex)0x2b,
                            (EColorsHex)0x1c,(EColorsHex)0x2b,
                            (EColorsHex)0x1c,(EColorsHex)0x2b,
                        },

                        new EColorsHex[] {
                            (EColorsHex)0x01,(EColorsHex)0x1c,
                            (EColorsHex)0x01,(EColorsHex)0x1c,
                            (EColorsHex)0x01,(EColorsHex)0x1c,
                            (EColorsHex)0x01,(EColorsHex)0x1c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x03,(EColorsHex)0x16,
                            (EColorsHex)0x03,(EColorsHex)0x16,
                            (EColorsHex)0x03,(EColorsHex)0x16,
                            (EColorsHex)0x03,(EColorsHex)0x16,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0b,(EColorsHex)0x22,
                            (EColorsHex)0x0b,(EColorsHex)0x22,
                            (EColorsHex)0x0b,(EColorsHex)0x22,
                            (EColorsHex)0x0b,(EColorsHex)0x22,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x00,(EColorsHex)0x0f,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0f,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x00,
                            (EColorsHex)0x0f,(EColorsHex)0x00,
                        },
                    }
                },
            };

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (ColorSet set in StageSelectColorSets)
                {
                    set.RandomizeAndWrite(stream, RandomMM2.Random);
                }
            }
        }

        private void RandomizeWeaponColors()
        {
            // Create lists of possible colors to choose from and shuffle them
            List<byte> PossibleDarkColors = new List<byte>();
            List<byte> PossibleLightColors = new List<byte>();

            for (byte i = 0x01; i <= 0x0C; i++)
            {
                // Add first two rows of colors to dark list (except black/white/gray)
                PossibleDarkColors.Add(i);
                PossibleDarkColors.Add((byte)(i + 0x10));
                // Add third and fourth rows to light list (except black/white/gray)
                PossibleLightColors.Add((byte)(i + 0x20));
                PossibleLightColors.Add((byte)(i + 0x30));
            }
            // Add black and dark-gray to dark list, white and light-gray to light list
            PossibleDarkColors.Add(0x0F);
            PossibleDarkColors.Add(0x00);
            PossibleLightColors.Add(0x10);
            PossibleLightColors.Add(0x20);
            
            // Randomize lists, and pick the first 9 and 8 elements to use as new colors
            PossibleDarkColors.Shuffle(RandomMM2.Random);
            PossibleLightColors.Shuffle(RandomMM2.Random);
            Queue<byte> DarkColors = new Queue<byte>(PossibleDarkColors.GetRange(0, 9));
            Queue<byte> LightColors = new Queue<byte>(PossibleLightColors.GetRange(0, 8));

            // Get starting address depending on game version
            int startAddress = (RandomMM2.Settings.IsJapanese) ? MegaManColorAddressJ : MegaManColorAddressU;

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Change 8 robot master weapon colors
                for (int i = 0; i < 8; i++)
                {
                    byte dark = DarkColors.Dequeue();
                    byte light = LightColors.Dequeue();

                    stream.Position = startAddress + 0x04 + i * 0x04;
                    stream.WriteByte(light);
                    stream.WriteByte(dark);

                    if (i == 0)
                    {
                        //0x03DE49 - H charge colors
                        //    0F 15 - flash neutral color (15 = weapon color)
                        //    31 15 - flash lv 1(outline only; keep 15 from weapon color)
                        //    35 2C - flash lv 2
                        //    30 30 - flash lv 3
                        stream.Position = 0x03DE4A;
                        stream.WriteByte(dark);
                        stream.Position = 0x03DE4C;
                        stream.WriteByte(dark);
                    }
                }

                // Change 3 Item colors
                byte itemColor = DarkColors.Dequeue();
                for (int i = 0; i < 3; i++)
                {
                    stream.Position = startAddress + 0x25 + i * 0x04;
                    stream.WriteByte(itemColor);
                }
            }
        }

        private void RandomizeRobotMasterColors()
        {
            //// Robot Master Color Palettes
            List<int> SolidColorSolo = new List<int>
            {
                0x00B4EA, // Wood leaf color 0x29
                0x01B4A1, // Metal blade color 0x30
            };

            List<int> SolidColorPair1Main = new List<int> {
                0x01F4ED, // Clash red color 0x16
                0x0174B7, // Flash blue color 0x12
                0x0074B4, // Air projectile blue color 0x11
                0x00B4ED, // Wood orange color 0x17
            };

            List<int> SolidColorPair1White = new List<int> {
                0x01F4EC, // Clash white color 0x30
                0x0174B6, // Flash white color 0x30
                0x0074B3, // Air projectile white color 0x30
                0x00B4EC, // Wood white color 0x36
            };

            List<int> SolidColorPair2Dark = new List<int> {
                0x0034B4, // Heat projectile red color 0x15
                0x0034B7, // Heat red color 0x15
                0x0074B7, // Air blue color 0x11
                0x0134C6, // Quick intro color 2 0x28
                0x0134C9, // Quick red color 0x15
                0x01B4A5, // Metal red color 0x15
                0x00F4B7, // Bubble green color 0x19
            };

            List<int> SolidColorPair2Light = new List<int> {
                0x0034B3, // Heat projectile yellow color 0x28
                0x0034B6, // Heat yellow color 0x28
                0x0074B6, // Air yellow color 0x28
                0x0134C5, // Quick intro color 1 0x30
                0x0134C8, // Quick yellow color 0x28
                0x01B4A4, // Metal yellow color 0x28
                0x00F4B6, // Bubble white & projectile color 0x30
            };

            // Colors for bosses with 1 solid color and 1 white
            List<byte> goodSolidColors = new List<byte>()
            {
                0x0F,0x20,0x31,0x22,0x03,0x23,0x14,0x05,0x15,0x16,0x07,0x27,0x28,0x09,0x1A,0x2A,0x0B,0x2B,0x0C,0x1C,
            };

            // Colors for bosses with a dark and a light color
            List<byte> goodDarkColors = new List<byte>()
            {
                0x01,0x12,0x03,0x04,0x05,0x16,0x07,0x18,0x09,0x1A,0x0B,0x0C,0x0F,0x00,
            };
            List<byte> goodLightColors = new List<byte>()
            {
                0x21,0x32,0x23,0x34,0x15,0x26,0x27,0x28,0x29,0x3A,0x1B,0x2C,0x10,0x20,
            };

            // Dark colors only
            List<byte> darkOnly = new List<byte>()
            {
                0x0F,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C
            };
            // Medium colors only
            List<byte> mediumOnly = new List<byte>()
            {
                0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C
            };


            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                int rColor = 0;
                for (int i = 0; i < SolidColorSolo.Count; i++)
                {
                    stream.Position = SolidColorSolo[i];
                    rColor = RandomMM2.Random.Next(goodSolidColors.Count);
                    stream.WriteByte(goodSolidColors[rColor]);
                }

                for (int i = 0; i < SolidColorPair1Main.Count; i++)
                {
                    rColor = RandomMM2.Random.Next(goodSolidColors.Count);
                    stream.Position = SolidColorPair1Main[i];
                    stream.WriteByte(goodSolidColors[rColor]);
                    
                    // Make 2nd color brighter. If already bright, make white.
                    rColor = RandomMM2.Random.Next(goodSolidColors.Count);
                    int lightColor = goodSolidColors[rColor] + 0x10;
                    if (lightColor > 0x3C)
                    {
                        lightColor = 0x30;
                    }
                    stream.Position = SolidColorPair1White[i];
                    stream.WriteByte((byte)lightColor);

                }

                for (int i = 0; i < SolidColorPair2Dark.Count; i++)
                {
                    stream.Position = SolidColorPair2Dark[i];
                    rColor = RandomMM2.Random.Next(SolidColorPair2Dark.Count);
                    stream.WriteByte(goodDarkColors[rColor]);

                    stream.Position = SolidColorPair2Light[i];
                    rColor = RandomMM2.Random.Next(SolidColorPair2Light.Count);
                    stream.WriteByte(goodLightColors[rColor]);
                }

                // Wily Machine
                // choose main body color
                rColor = RandomMM2.Random.Next(darkOnly.Count);
                byte shade0 = darkOnly[rColor];
                byte shade1 = (byte)(shade0 + 0x10);
                if (shade0 == 0x0F)
                    shade1 = 0x00; // Dark gray up from black
                byte shade2 = (byte)(shade1 + 0x10);

                stream.Position = 0x02D7D5; // Wily Machine light gold      0x27
                stream.WriteByte(shade2);
                stream.Position = 0x02D7D2; // Wily Machine gold 1          0x17
                stream.WriteByte(shade1);
                stream.Position = 0x02D7D6; // Wily Machine gold 2          0x17
                stream.WriteByte(shade1);
                stream.Position = 0x02D7DA; // Wily Machine gold 3          0x17
                stream.WriteByte(shade1);
                stream.Position = 0x02D7D7; // Wily Machine dark gold 1     0x07
                stream.WriteByte(shade0);
                stream.Position = 0x02D7DB; // Wily Machine dark gold 2     0x07
                stream.WriteByte(shade0);

                // choose front color
                rColor = RandomMM2.Random.Next(mediumOnly.Count);
                shade0 = mediumOnly[rColor];
                shade1 = (byte)(shade0 + 0x20);

                stream.Position = 0x02D7D1; // Wily Machine red 1           0x15
                stream.WriteByte(shade0);
                stream.Position = 0x02D7D9; // Wily Machine red 2           0x15
                stream.WriteByte(shade0);
                stream.Position = 0x02D7D3; // Wily Machine lightest red 1  0x35
                stream.WriteByte(shade1);

                // Alien
                //0x02DC74(3 bytes) Alien Body, static   0x16 0x29 0x19
                //0x02DC78(3 bytes) Alien Head, static   0x16 0x29 0x19
                // Looks good as 4 separate color groups, should be easy. Save the animations for later.
            }
        }

        private void RandomizeStageColors()
        {
            StagesColorSets = new List<ColorSet>()
            {
                #region 01 Heatman

                new ColorSet() { // Heat | River 
                    addresses = new int[] {
                        0x3e1f, 0x3e20, 0x3e21, // default BG
                        0x3e3f, 0x3e4f, 0x3e5f, // animated BG
                        0x3e40, 0x3e50, 0x3e60,
                        0x3e41, 0x3e51, 0x3e61 },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            EColorsHex.Taupe,       EColorsHex.LightOrange, EColorsHex.Orange,
                            EColorsHex.Taupe,       EColorsHex.LightOrange, EColorsHex.Orange,
                            EColorsHex.LightOrange, EColorsHex.Orange,      EColorsHex.Taupe,
                            EColorsHex.Orange,      EColorsHex.Taupe,       EColorsHex.LightOrange,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightGreen,  EColorsHex.Green,       EColorsHex.ForestGreen,
                            EColorsHex.LightGreen,  EColorsHex.Green,       EColorsHex.ForestGreen,
                            EColorsHex.Green,       EColorsHex.ForestGreen, EColorsHex.LightGreen,
                            EColorsHex.ForestGreen, EColorsHex.LightGreen,  EColorsHex.Green,
                        },
                        new EColorsHex[] {
                            EColorsHex.Yellow,      EColorsHex.GoldenRod,   EColorsHex.Brown,
                            EColorsHex.Yellow,      EColorsHex.GoldenRod,   EColorsHex.Brown,
                            EColorsHex.GoldenRod,   EColorsHex.Brown,       EColorsHex.Yellow,
                            EColorsHex.Brown,       EColorsHex.Yellow,      EColorsHex.GoldenRod,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightPink,   EColorsHex.Magenta,     EColorsHex.DarkMagenta,
                            EColorsHex.LightPink,   EColorsHex.Magenta,     EColorsHex.DarkMagenta,
                            EColorsHex.Magenta,     EColorsHex.DarkMagenta, EColorsHex.LightPink,
                            EColorsHex.DarkMagenta, EColorsHex.LightPink,   EColorsHex.Magenta,
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
                            EColorsHex.Taupe,       EColorsHex.LightOrange,EColorsHex.VioletRed,
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

                #endregion

                #region 01 Wily 1

                new ColorSet() { // Wily 1 | Solid Background and Clouds
                    addresses = new int[] {0x3f15, 0x3f17, 0x3f18, 0x3f19,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default Dark Cyan
                        new EColorsHex[] {(EColorsHex)0x0C,(EColorsHex)0x1C,(EColorsHex)0x0C,(EColorsHex)0x0C},
                        new EColorsHex[] {(EColorsHex)0x0b,(EColorsHex)0x1b,(EColorsHex)0x0b,(EColorsHex)0x0b},
                        new EColorsHex[] {(EColorsHex)0x09,(EColorsHex)0x19,(EColorsHex)0x09,(EColorsHex)0x09},
                        new EColorsHex[] {(EColorsHex)0x08,(EColorsHex)0x18,(EColorsHex)0x08,(EColorsHex)0x08},
                        new EColorsHex[] {(EColorsHex)0x07,(EColorsHex)0x17,(EColorsHex)0x07,(EColorsHex)0x07},
                        new EColorsHex[] {(EColorsHex)0x05,(EColorsHex)0x15,(EColorsHex)0x05,(EColorsHex)0x05},
                        new EColorsHex[] {(EColorsHex)0x04,(EColorsHex)0x14,(EColorsHex)0x04,(EColorsHex)0x04},
                        new EColorsHex[] {(EColorsHex)0x03,(EColorsHex)0x13,(EColorsHex)0x03,(EColorsHex)0x03},
                        new EColorsHex[] {(EColorsHex)0x02,(EColorsHex)0x12,(EColorsHex)0x02,(EColorsHex)0x02},
                        new EColorsHex[] {(EColorsHex)0x0f,(EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x0f},
                    }
                },

                new ColorSet() { // Wily 1 | Building Exterior Walls
                    addresses = new int[] {0x3f13, 0x3f14,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default Gray
                        new EColorsHex[] {(EColorsHex)0x20,(EColorsHex)0x10},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x11,},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x13,},
                        new EColorsHex[] {(EColorsHex)0x26,(EColorsHex)0x16,},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x17,},
                        new EColorsHex[] {(EColorsHex)0x28,(EColorsHex)0x18,},
                        new EColorsHex[] {(EColorsHex)0x2a,(EColorsHex)0x1a,},
                        new EColorsHex[] {(EColorsHex)0x2c,(EColorsHex)0x1c,},

                    }
                },

                new ColorSet() { // Wily 1 | Ground and Building Interior Walls
                    addresses = new int[] {0x3f1b, 0x3f1c, 0x3f1d,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default Gold3
                        new EColorsHex[] {(EColorsHex)0x38,(EColorsHex)0x27,(EColorsHex)0x07},
                        new EColorsHex[] {(EColorsHex)0x37,(EColorsHex)0x26,(EColorsHex)0x06},
                        new EColorsHex[] {(EColorsHex)0x36,(EColorsHex)0x25,(EColorsHex)0x05},
                        new EColorsHex[] {(EColorsHex)0x34,(EColorsHex)0x23,(EColorsHex)0x03,},
                        new EColorsHex[] {(EColorsHex)0x33,(EColorsHex)0x22,(EColorsHex)0x02,},
                        new EColorsHex[] {(EColorsHex)0x32,(EColorsHex)0x21,(EColorsHex)0x01,},
                        new EColorsHex[] {(EColorsHex)0x31,(EColorsHex)0x2c,(EColorsHex)0x0c,},
                        new EColorsHex[] {(EColorsHex)0x3c,(EColorsHex)0x2b,(EColorsHex)0x0b,},
                        new EColorsHex[] {(EColorsHex)0x3b,(EColorsHex)0x2a,(EColorsHex)0x0a,},
                        new EColorsHex[] {(EColorsHex)0x39,(EColorsHex)0x28,(EColorsHex)0x08,},

                    }
                },

                new ColorSet() { // Wily 1 | Building Background
                    addresses = new int[] {0x3f1f, 0x3f20, 0x3f21,},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default Teal
                        new EColorsHex[] {(EColorsHex)0x2c,(EColorsHex)0x1b,(EColorsHex)0x0c},
                        new EColorsHex[] {(EColorsHex)0x2a,(EColorsHex)0x19,(EColorsHex)0x0a},
                        new EColorsHex[] {(EColorsHex)0x29,(EColorsHex)0x18,(EColorsHex)0x09},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x16,(EColorsHex)0x07},
                        new EColorsHex[] {(EColorsHex)0x24,(EColorsHex)0x13,(EColorsHex)0x04},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x12,(EColorsHex)0x03},
                        new EColorsHex[] {(EColorsHex)0x22,(EColorsHex)0x11,(EColorsHex)0x02},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x1c,(EColorsHex)0x01},
                    }
                },

                #endregion

                #region 02 Airman

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
                            EColorsHex.LightBlue,  EColorsHex.PastelBlue,  EColorsHex.White,
                            EColorsHex.LightBlue,  EColorsHex.PastelBlue,  EColorsHex.White,  EColorsHex.PastelBlue,
                            EColorsHex.PastelBlue, EColorsHex.White,       EColorsHex.White,  EColorsHex.White,
                            EColorsHex.White,      EColorsHex.White,       EColorsHex.White,  EColorsHex.White,
                        },
                        new EColorsHex[] {
                            EColorsHex.LightGray,  EColorsHex.LightGray,    EColorsHex.LightGray,
                            EColorsHex.LightGray,  EColorsHex.Gray,         EColorsHex.DarkRed,     EColorsHex.Gray,
                            EColorsHex.LightGray,  EColorsHex.LightGray,    EColorsHex.Gray,        EColorsHex.LightGray,
                            EColorsHex.LightGray,  EColorsHex.LightGray,    EColorsHex.LightGray,   EColorsHex.LightGray,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x04, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x02, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x0c, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x0b, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x0a, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x08, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x06, (EColorsHex)0x00,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10, (EColorsHex)0x10,
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

                #endregion

                #region 02 Wily 2

                new ColorSet() { // Wily 2 | Ground
                    addresses = new int[] {
                        0x7f13, 0x7f14, 0x7f15,
                        0x7f33, 0x7f34, 0x7f35,
                        0x7f43, 0x7f44, 0x7f45,
                        0x7f53, 0x7f54, 0x7f55,
                        0x7f63, 0x7f64, 0x7f65,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x08,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x07,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x06,
                            (EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x06,
                            (EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x06,
                            (EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x06,
                            (EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x06,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x04,
                            (EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x04,
                            (EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x04,
                            (EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x04,
                            (EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x04,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x03,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x02,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x01,
                            (EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x01,
                            (EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x01,
                            (EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x01,
                            (EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x01,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x09,
                        },
                    }
                },
                
                new ColorSet() { // Wily 2 | Background
                    addresses = new int[] { 0x7f18, 0x7f38, 0x7f48, 0x7f58, 0x7f68, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | Leaves | Default
                        new EColorsHex[] {(EColorsHex)0x07,(EColorsHex)0x07, (EColorsHex)0x07, (EColorsHex)0x07, (EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x04,(EColorsHex)0x04, (EColorsHex)0x04, (EColorsHex)0x04, (EColorsHex)0x04, },
                        new EColorsHex[] {(EColorsHex)0x02,(EColorsHex)0x02, (EColorsHex)0x02, (EColorsHex)0x02, (EColorsHex)0x02, },
                        new EColorsHex[] {(EColorsHex)0x00,(EColorsHex)0x00, (EColorsHex)0x00, (EColorsHex)0x00, (EColorsHex)0x00, },
                        new EColorsHex[] {(EColorsHex)0x0c,(EColorsHex)0x0c, (EColorsHex)0x0c, (EColorsHex)0x0c, (EColorsHex)0x0c, },
                        new EColorsHex[] {(EColorsHex)0x0b,(EColorsHex)0x0b, (EColorsHex)0x0b, (EColorsHex)0x0b, (EColorsHex)0x0b, },
                        new EColorsHex[] {(EColorsHex)0x0b,(EColorsHex)0x09, (EColorsHex)0x09, (EColorsHex)0x09, (EColorsHex)0x09, },
                        new EColorsHex[] {(EColorsHex)0x18,(EColorsHex)0x18, (EColorsHex)0x18, (EColorsHex)0x18, (EColorsHex)0x18, },
                    }
                },

                new ColorSet() { // Wily 2 | Fan
                    addresses = new int[] {
                        0x7f1f, 0x7f20,
                        0x7f3f, 0x7f40,
                        0x7f50, 0x7f51,
                        0x7f5f, 0x7f61,
                        0x7f6f, 0x7f70,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x31, (EColorsHex)0x31,
                            (EColorsHex)0x31, (EColorsHex)0x31,
                            (EColorsHex)0x31, (EColorsHex)0x31,
                            (EColorsHex)0x31, (EColorsHex)0x31,
                            (EColorsHex)0x31, (EColorsHex)0x31,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2b, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2a, (EColorsHex)0x2a,
                            (EColorsHex)0x2a, (EColorsHex)0x2a,
                            (EColorsHex)0x2a, (EColorsHex)0x2a,
                            (EColorsHex)0x2a, (EColorsHex)0x2a,
                            (EColorsHex)0x2a, (EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x38, (EColorsHex)0x38,
                            (EColorsHex)0x38, (EColorsHex)0x38,
                            (EColorsHex)0x38, (EColorsHex)0x38,
                            (EColorsHex)0x38, (EColorsHex)0x38,
                            (EColorsHex)0x38, (EColorsHex)0x38,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x26, (EColorsHex)0x26,
                            (EColorsHex)0x26, (EColorsHex)0x26,
                            (EColorsHex)0x26, (EColorsHex)0x26,
                            (EColorsHex)0x26, (EColorsHex)0x26,
                            (EColorsHex)0x26, (EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x24, (EColorsHex)0x24,
                            (EColorsHex)0x24, (EColorsHex)0x24,
                            (EColorsHex)0x24, (EColorsHex)0x24,
                            (EColorsHex)0x24, (EColorsHex)0x24,
                            (EColorsHex)0x24, (EColorsHex)0x24,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x22, (EColorsHex)0x22,
                            (EColorsHex)0x22, (EColorsHex)0x22,
                            (EColorsHex)0x22, (EColorsHex)0x22,
                            (EColorsHex)0x22, (EColorsHex)0x22,
                            (EColorsHex)0x22, (EColorsHex)0x22,
                        },
                    }
                },

                new ColorSet() { // Wily 2 | Boss Room
                    addresses = new int[] {
                        0x7f1b, 0x7f1c, 0x7f1d,
                        0x7f3b, 0x7f3c, 0x7f3d,
                        0x7f4b, 0x7f4c, 0x7f4d,
                        0x7f5b, 0x7f5c, 0x7f5d,
                        0x7f6b, 0x7f6c, 0x7f6d,},
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x34,(EColorsHex)0x15, (EColorsHex)0x05,
                            (EColorsHex)0x34,(EColorsHex)0x15, (EColorsHex)0x05,
                            (EColorsHex)0x34,(EColorsHex)0x15, (EColorsHex)0x05,
                            (EColorsHex)0x34,(EColorsHex)0x15, (EColorsHex)0x05,
                            (EColorsHex)0x34,(EColorsHex)0x15, (EColorsHex)0x05,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x32,(EColorsHex)0x13, (EColorsHex)0x03,
                            (EColorsHex)0x32,(EColorsHex)0x13, (EColorsHex)0x03,
                            (EColorsHex)0x32,(EColorsHex)0x13, (EColorsHex)0x03,
                            (EColorsHex)0x32,(EColorsHex)0x13, (EColorsHex)0x03,
                            (EColorsHex)0x32,(EColorsHex)0x13, (EColorsHex)0x03,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3c,(EColorsHex)0x11, (EColorsHex)0x01,
                            (EColorsHex)0x3c,(EColorsHex)0x11, (EColorsHex)0x01,
                            (EColorsHex)0x3c,(EColorsHex)0x11, (EColorsHex)0x01,
                            (EColorsHex)0x3c,(EColorsHex)0x11, (EColorsHex)0x01,
                            (EColorsHex)0x3c,(EColorsHex)0x11, (EColorsHex)0x01,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3b,(EColorsHex)0x1c, (EColorsHex)0x0c,
                            (EColorsHex)0x3b,(EColorsHex)0x1c, (EColorsHex)0x0c,
                            (EColorsHex)0x3b,(EColorsHex)0x1c, (EColorsHex)0x0c,
                            (EColorsHex)0x3b,(EColorsHex)0x1c, (EColorsHex)0x0c,
                            (EColorsHex)0x3b,(EColorsHex)0x1c, (EColorsHex)0x0c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x39,(EColorsHex)0x1a, (EColorsHex)0x0a,
                            (EColorsHex)0x39,(EColorsHex)0x1a, (EColorsHex)0x0a,
                            (EColorsHex)0x39,(EColorsHex)0x1a, (EColorsHex)0x0a,
                            (EColorsHex)0x39,(EColorsHex)0x1a, (EColorsHex)0x0a,
                            (EColorsHex)0x39,(EColorsHex)0x1a, (EColorsHex)0x0a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x37,(EColorsHex)0x18, (EColorsHex)0x08,
                            (EColorsHex)0x37,(EColorsHex)0x18, (EColorsHex)0x08,
                            (EColorsHex)0x37,(EColorsHex)0x18, (EColorsHex)0x08,
                            (EColorsHex)0x37,(EColorsHex)0x18, (EColorsHex)0x08,
                            (EColorsHex)0x37,(EColorsHex)0x18, (EColorsHex)0x08,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x36,(EColorsHex)0x17, (EColorsHex)0x07,
                            (EColorsHex)0x36,(EColorsHex)0x17, (EColorsHex)0x07,
                            (EColorsHex)0x36,(EColorsHex)0x17, (EColorsHex)0x07,
                            (EColorsHex)0x36,(EColorsHex)0x17, (EColorsHex)0x07,
                            (EColorsHex)0x36,(EColorsHex)0x17, (EColorsHex)0x07,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x10,(EColorsHex)0x00, (EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00, (EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00, (EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00, (EColorsHex)0x0f,
                            (EColorsHex)0x10,(EColorsHex)0x00, (EColorsHex)0x0f,
                        },
                    }
                },

                #endregion

                #region 03 Woodman

                new ColorSet() {
                    addresses = new int[] { 0xbe13, 0xbe14, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Wood | Leaves | Default
                        new EColorsHex[] {  EColorsHex.Lemon, EColorsHex.Grass,},
                        // Wood | Leaves | Blue
                        new EColorsHex[] {  EColorsHex.MediumBlue, EColorsHex.RoyalBlue,},
                        // Wood | Leaves | Red
                        new EColorsHex[] {  EColorsHex.Orange, EColorsHex.Red,},
                        new EColorsHex[] {  (EColorsHex)0x28, (EColorsHex)0x18, },
                        new EColorsHex[] {  (EColorsHex)0x27, (EColorsHex)0x17, },
                        new EColorsHex[] {  (EColorsHex)0x26, (EColorsHex)0x16, },
                        new EColorsHex[] {  (EColorsHex)0x25, (EColorsHex)0x15, },
                        new EColorsHex[] {  (EColorsHex)0x24, (EColorsHex)0x14, },
                        new EColorsHex[] {  (EColorsHex)0x23, (EColorsHex)0x13, },
                        new EColorsHex[] {  (EColorsHex)0x22, (EColorsHex)0x12, },
                        new EColorsHex[] {  (EColorsHex)0x21, (EColorsHex)0x11, },
                        new EColorsHex[] {  (EColorsHex)0x2c, (EColorsHex)0x1c, },
                        new EColorsHex[] {  (EColorsHex)0x2b, (EColorsHex)0x1b, },
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

                #endregion

                #region 03 Wily 3

                new ColorSet() { // Wily 3 | Underwater Walls
                    addresses = new int[] { 0xbf13, 0xbf14, 0xbf15},
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x28,(EColorsHex)0x17,(EColorsHex)0x18,},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x16,(EColorsHex)0x17,},
                        new EColorsHex[] {(EColorsHex)0x25,(EColorsHex)0x14,(EColorsHex)0x15,},
                        new EColorsHex[] {(EColorsHex)0x24,(EColorsHex)0x13,(EColorsHex)0x14,},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x12,(EColorsHex)0x13,},
                        new EColorsHex[] {(EColorsHex)0x22,(EColorsHex)0x11,(EColorsHex)0x12,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x1c,(EColorsHex)0x11,},
                        new EColorsHex[] {(EColorsHex)0x2c,(EColorsHex)0x1b,(EColorsHex)0x1c,},
                        new EColorsHex[] {(EColorsHex)0x2a,(EColorsHex)0x19,(EColorsHex)0x1a,},
                        new EColorsHex[] {(EColorsHex)0x29,(EColorsHex)0x18,(EColorsHex)0x19,},
                    }
                },

                new ColorSet() { // Wily 3 | Walls
                    addresses = new int[] { 0xbf17, 0xbf18, 0xbf19},
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x29,(EColorsHex)0x18,(EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x28,(EColorsHex)0x17,(EColorsHex)0x06,},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x16,(EColorsHex)0x05,},
                        new EColorsHex[] {(EColorsHex)0x25,(EColorsHex)0x14,(EColorsHex)0x03,},
                        new EColorsHex[] {(EColorsHex)0x24,(EColorsHex)0x13,(EColorsHex)0x02,},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x12,(EColorsHex)0x01,},
                        new EColorsHex[] {(EColorsHex)0x22,(EColorsHex)0x11,(EColorsHex)0x0c,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x1c,(EColorsHex)0x0b,},
                        new EColorsHex[] {(EColorsHex)0x2c,(EColorsHex)0x1b,(EColorsHex)0x0a,},
                        new EColorsHex[] {(EColorsHex)0x2a,(EColorsHex)0x19,(EColorsHex)0x08,},
                    }
                },

                new ColorSet() { // Wily 3 | Water
                    addresses = new int[] { 0xbf1c, 0xbf1d,},
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x28,(EColorsHex)0x18,},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x17,},
                        new EColorsHex[] {(EColorsHex)0x26,(EColorsHex)0x16,},
                        new EColorsHex[] {(EColorsHex)0x25,(EColorsHex)0x15,},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x13,},
                        new EColorsHex[] {(EColorsHex)0x22,(EColorsHex)0x12,},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x11,},
                        new EColorsHex[] {(EColorsHex)0x2c,(EColorsHex)0x1c,},
                        new EColorsHex[] {(EColorsHex)0x2b,(EColorsHex)0x1b,},
                        new EColorsHex[] {(EColorsHex)0x2a,(EColorsHex)0x1a,},
                        new EColorsHex[] {(EColorsHex)0x0f,(EColorsHex)0x00,},
                    }
                },

                new ColorSet() { // Wily 3 | Background
                    addresses = new int[] { 0xbf1f, 0xbf20, 0xbf21},
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x0a,(EColorsHex)0x08,(EColorsHex)0x0b,},
                        new EColorsHex[] {(EColorsHex)0x08,(EColorsHex)0x06,(EColorsHex)0x09,},
                        new EColorsHex[] {(EColorsHex)0x06,(EColorsHex)0x04,(EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x04,(EColorsHex)0x02,(EColorsHex)0x05,},
                        new EColorsHex[] {(EColorsHex)0x02,(EColorsHex)0x0c,(EColorsHex)0x03,},
                        new EColorsHex[] {(EColorsHex)0x01,(EColorsHex)0x0b,(EColorsHex)0x02,},
                        new EColorsHex[] {(EColorsHex)0x0c,(EColorsHex)0x0a,(EColorsHex)0x01,},
                        new EColorsHex[] {(EColorsHex)0x0f,(EColorsHex)0x00,(EColorsHex)0x10,},
                    }
                },

                #endregion

                #region 04 Bubbleman

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
                    addresses = new int[] { 0xfe22,
                        0xfe1b, 0xfe1c, 0xfe1d,
                        0xfe3b, 0xfe3c, 0xfe3d,
                        0xfe4b, 0xfe4c, 0xfe4d,
                        0xfe5b, 0xfe5c, 0xfe5d,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,(EColorsHex)0x11,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,
                        },
                        new EColorsHex[] {
                            EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,EColorsHex.Black3,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,(EColorsHex)0x13,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,(EColorsHex)0x05,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,(EColorsHex)0x16,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,(EColorsHex)0x07,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,(EColorsHex)0x17,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,(EColorsHex)0x09,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,(EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,
                        },
                    }
                },

                #endregion

                #region 04 Wily 4

                new ColorSet() { // Wily 4 | Walls
                    addresses = new int[] { 0xff13, 0xff14, 0xff15, },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x14,(EColorsHex)0x03,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x13,(EColorsHex)0x02,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x12,(EColorsHex)0x01,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x11,(EColorsHex)0x0c,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x1c,(EColorsHex)0x0b,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x1b,(EColorsHex)0x0a,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x19,(EColorsHex)0x08,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x18,(EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x17,(EColorsHex)0x06,},
                        new EColorsHex[] {(EColorsHex)0x10,(EColorsHex)0x16,(EColorsHex)0x05,},
                    }
                },

                new ColorSet() { // Wily 4 | Background
                    addresses = new int[] { 0xff18, },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x02, },
                        new EColorsHex[] {(EColorsHex)0x04, },
                        new EColorsHex[] {(EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x09, },
                        new EColorsHex[] {(EColorsHex)0x0b, },
                        new EColorsHex[] {(EColorsHex)0x0c, },
                        new EColorsHex[] {(EColorsHex)0x18, },
                        new EColorsHex[] {(EColorsHex)0x00, },
                    }
                },

                new ColorSet() { // Wily 4 | Track
                    addresses = new int[] { 0xff1B, 0xff1C, 0xff1d, },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x20,(EColorsHex)0x10,(EColorsHex)0x00,},
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x11,(EColorsHex)0x01,},
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x13,(EColorsHex)0x03,},
                        new EColorsHex[] {(EColorsHex)0x25,(EColorsHex)0x15,(EColorsHex)0x05,},
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x17,(EColorsHex)0x07,},
                        new EColorsHex[] {(EColorsHex)0x29,(EColorsHex)0x19,(EColorsHex)0x09,},
                        new EColorsHex[] {(EColorsHex)0x2b,(EColorsHex)0x1b,(EColorsHex)0x0b,},
                    }
                },

                #endregion

                #region 05 Quickman

                new ColorSet() { // Quick | Walls
                    addresses = new int[] { 0x013e13, 0x013e14, 0x013e15, },
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default
                        new EColorsHex[] {(EColorsHex)0x2C,(EColorsHex)0x10,(EColorsHex)0x1C, },
                        // Green
                        new EColorsHex[] {(EColorsHex)0x2b,(EColorsHex)0x10,(EColorsHex)0x1d, },
                        // Yellow
                        new EColorsHex[] {(EColorsHex)0x28,(EColorsHex)0x10,(EColorsHex)0x18, },
                        // Orange
                        new EColorsHex[] {(EColorsHex)0x27,(EColorsHex)0x10,(EColorsHex)0x17, },
                        // Red
                        new EColorsHex[] {(EColorsHex)0x26,(EColorsHex)0x10,(EColorsHex)0x16, },
                        // Pink
                        new EColorsHex[] {(EColorsHex)0x25,(EColorsHex)0x10,(EColorsHex)0x15, },
                        // Magenta
                        new EColorsHex[] {(EColorsHex)0x24,(EColorsHex)0x10,(EColorsHex)0x14, },
                        // Purple
                        new EColorsHex[] {(EColorsHex)0x23,(EColorsHex)0x10,(EColorsHex)0x13, },
                        // Blue
                        new EColorsHex[] {(EColorsHex)0x22,(EColorsHex)0x10,(EColorsHex)0x12, },
                        // Light Blue
                        new EColorsHex[] {(EColorsHex)0x21,(EColorsHex)0x10,(EColorsHex)0x11, },
                    }
                },

                new ColorSet() { // Quick | Beams and Background
                    addresses = new int[] {0x013e17, 0x013e18, 0x013e19, 0x013e1b, 0x013e1c, 0x013e1d},
                    ColorBytes = new List<EColorsHex[]>() {
                        // Default
                        new EColorsHex[] {(EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x07,(EColorsHex)0x28,(EColorsHex)0x16,(EColorsHex)0x07, },
                        // Purple
                        new EColorsHex[] {(EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x04,(EColorsHex)0x25,(EColorsHex)0x13,(EColorsHex)0x04, },
                        // Blue
                        new EColorsHex[] {(EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x02,(EColorsHex)0x23,(EColorsHex)0x11,(EColorsHex)0x02, },
                        // Cyan
                        new EColorsHex[] {(EColorsHex)0x3C,(EColorsHex)0x2C,(EColorsHex)0x0C,(EColorsHex)0x21,(EColorsHex)0x1B,(EColorsHex)0x0C, },
                        // Green
                        new EColorsHex[] {(EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x0a,(EColorsHex)0x2b,(EColorsHex)0x19,(EColorsHex)0x0a, },
                        // Green 2
                        new EColorsHex[] {(EColorsHex)0x39,(EColorsHex)0x29,(EColorsHex)0x09,(EColorsHex)0x2a,(EColorsHex)0x18,(EColorsHex)0x09, },
                        // Gold
                        new EColorsHex[] {(EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x08,(EColorsHex)0x29,(EColorsHex)0x17,(EColorsHex)0x08, },
                        // Gray
                        new EColorsHex[] {(EColorsHex)0x0f,(EColorsHex)0x0f,(EColorsHex)0x00,(EColorsHex)0x20,(EColorsHex)0x10,(EColorsHex)0x00, },

                    }
                },

                #endregion

                #region 05 Wily 5

                new ColorSet() { // Wily 5 | Walls
                    addresses = new int[] {
                        0x013f13, 0x013f14, 0x013f15,
                        0x013f33, 0x013f34, 0x013f35,
                        0x013f43, 0x013f44, 0x013f45,
                        0x013f53, 0x013f54, 0x013f55,
                        0x013f63, 0x013f64, 0x013f65,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x30,(EColorsHex)0x32,(EColorsHex)0x22,
                            (EColorsHex)0x30,(EColorsHex)0x32,(EColorsHex)0x22,
                            (EColorsHex)0x30,(EColorsHex)0x32,(EColorsHex)0x22,
                            (EColorsHex)0x30,(EColorsHex)0x32,(EColorsHex)0x22,
                            (EColorsHex)0x30,(EColorsHex)0x32,(EColorsHex)0x22,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x30,(EColorsHex)0x38,(EColorsHex)0x18,
                            (EColorsHex)0x30,(EColorsHex)0x38,(EColorsHex)0x18,
                            (EColorsHex)0x30,(EColorsHex)0x38,(EColorsHex)0x18,
                            (EColorsHex)0x30,(EColorsHex)0x38,(EColorsHex)0x18,
                            (EColorsHex)0x30,(EColorsHex)0x38,(EColorsHex)0x18,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x30,(EColorsHex)0x3a,(EColorsHex)0x1a,
                            (EColorsHex)0x30,(EColorsHex)0x3a,(EColorsHex)0x1a,
                            (EColorsHex)0x30,(EColorsHex)0x3a,(EColorsHex)0x1a,
                            (EColorsHex)0x30,(EColorsHex)0x3a,(EColorsHex)0x1a,
                            (EColorsHex)0x30,(EColorsHex)0x3a,(EColorsHex)0x1a,
                        },
                    }
                },

                new ColorSet() { // Wily 5 | Teleporters
                    addresses = new int[] {
                        0x013f13, 0x013f14, 0x013f15,
                        0x013f33, 0x013f34, 0x013f35,
                        0x013f43, 0x013f44, 0x013f45,
                        0x013f53, 0x013f54, 0x013f55,
                        0x013f63, 0x013f64, 0x013f65,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x17,
                            (EColorsHex)0x37,(EColorsHex)0x27,(EColorsHex)0x17,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                            (EColorsHex)0x34,(EColorsHex)0x24,(EColorsHex)0x14,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                            (EColorsHex)0x32,(EColorsHex)0x22,(EColorsHex)0x12,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                            (EColorsHex)0x3c,(EColorsHex)0x2c,(EColorsHex)0x1c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                            (EColorsHex)0x3b,(EColorsHex)0x2b,(EColorsHex)0x1b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                            (EColorsHex)0x3a,(EColorsHex)0x2a,(EColorsHex)0x1a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                            (EColorsHex)0x38,(EColorsHex)0x28,(EColorsHex)0x18,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                            (EColorsHex)0x36,(EColorsHex)0x26,(EColorsHex)0x16,
                        },
                    }
                },

                new ColorSet() { // Wily 5 | Computers 1
                    addresses = new int[] {
                        0x013f1b, 0x013f1c,
                        0x013f3b, 0x013f3c,
                        0x013f4b, 0x013f4c,
                        0x013f5b, 0x013f5c,
                        0x013f6b, 0x013f6c,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x19,(EColorsHex)0x09,
                            (EColorsHex)0x19,(EColorsHex)0x09,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x18,(EColorsHex)0x08,
                            (EColorsHex)0x18,(EColorsHex)0x08,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x17,(EColorsHex)0x07,
                            (EColorsHex)0x17,(EColorsHex)0x07,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x15,(EColorsHex)0x05,
                            (EColorsHex)0x15,(EColorsHex)0x05,
                            (EColorsHex)0x15,(EColorsHex)0x05,
                            (EColorsHex)0x15,(EColorsHex)0x05,
                            (EColorsHex)0x15,(EColorsHex)0x05,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x13,(EColorsHex)0x03,
                            (EColorsHex)0x13,(EColorsHex)0x03,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x12,(EColorsHex)0x02,
                            (EColorsHex)0x12,(EColorsHex)0x02,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x1c,(EColorsHex)0x0c,
                            (EColorsHex)0x1c,(EColorsHex)0x0c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x1b,(EColorsHex)0x0b,
                            (EColorsHex)0x1b,(EColorsHex)0x0b,
                        },
                    }
                },

                new ColorSet() { // Wily 5 | Computers 2
                    addresses = new int[] {
                        0x013f1d,
                        0x013f3d,
                        0x013f4d,
                        0x013f5d,
                        0x013f6d,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x21,
                            (EColorsHex)0x21,
                            (EColorsHex)0x21,
                            (EColorsHex)0x21,
                            (EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2b,
                            (EColorsHex)0x2b,
                            (EColorsHex)0x2b,
                            (EColorsHex)0x2b,
                            (EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x2a,
                            (EColorsHex)0x2a,
                            (EColorsHex)0x2a,
                            (EColorsHex)0x2a,
                            (EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x28,
                            (EColorsHex)0x28,
                            (EColorsHex)0x28,
                            (EColorsHex)0x28,
                            (EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x26,
                            (EColorsHex)0x26,
                            (EColorsHex)0x26,
                            (EColorsHex)0x26,
                            (EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x23,
                            (EColorsHex)0x23,
                            (EColorsHex)0x23,
                            (EColorsHex)0x23,
                            (EColorsHex)0x23,
                        },
                    }
                },

                new ColorSet() { // Wily 5 | Computers 3
                    addresses = new int[] {
                        0x013f1f, 0x013f20, 0x013f21,
                        0x013f3f, 0x013f40, 0x013f41,
                        0x013f4f, 0x013f50, 0x013f51,
                        0x013f5f, 0x013f60, 0x013f61,
                        0x013f6f, 0x013f70, 0x013f71,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                            (EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,
                            (EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x01,
                            (EColorsHex)0x01,(EColorsHex)0x21,(EColorsHex)0x01,
                            (EColorsHex)0x01,(EColorsHex)0x01,(EColorsHex)0x21,
                            (EColorsHex)0x01,(EColorsHex)0x21,(EColorsHex)0x21,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,
                            (EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x0c,
                            (EColorsHex)0x0c,(EColorsHex)0x2c,(EColorsHex)0x0c,
                            (EColorsHex)0x0c,(EColorsHex)0x0c,(EColorsHex)0x2c,
                            (EColorsHex)0x0c,(EColorsHex)0x2c,(EColorsHex)0x2c,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,
                            (EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x0b,
                            (EColorsHex)0x0b,(EColorsHex)0x2b,(EColorsHex)0x0b,
                            (EColorsHex)0x0b,(EColorsHex)0x0b,(EColorsHex)0x2b,
                            (EColorsHex)0x0b,(EColorsHex)0x2b,(EColorsHex)0x2b,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x0a,(EColorsHex)0x0a,(EColorsHex)0x0a,
                            (EColorsHex)0x0a,(EColorsHex)0x0a,(EColorsHex)0x0a,
                            (EColorsHex)0x0a,(EColorsHex)0x2a,(EColorsHex)0x0a,
                            (EColorsHex)0x0a,(EColorsHex)0x0a,(EColorsHex)0x2a,
                            (EColorsHex)0x0a,(EColorsHex)0x2a,(EColorsHex)0x2a,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x08,(EColorsHex)0x08,(EColorsHex)0x08,
                            (EColorsHex)0x08,(EColorsHex)0x08,(EColorsHex)0x08,
                            (EColorsHex)0x08,(EColorsHex)0x28,(EColorsHex)0x08,
                            (EColorsHex)0x08,(EColorsHex)0x08,(EColorsHex)0x28,
                            (EColorsHex)0x08,(EColorsHex)0x28,(EColorsHex)0x28,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x06,(EColorsHex)0x06,(EColorsHex)0x06,
                            (EColorsHex)0x06,(EColorsHex)0x06,(EColorsHex)0x06,
                            (EColorsHex)0x06,(EColorsHex)0x26,(EColorsHex)0x06,
                            (EColorsHex)0x06,(EColorsHex)0x06,(EColorsHex)0x26,
                            (EColorsHex)0x06,(EColorsHex)0x26,(EColorsHex)0x26,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,
                            (EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x04,
                            (EColorsHex)0x04,(EColorsHex)0x24,(EColorsHex)0x04,
                            (EColorsHex)0x04,(EColorsHex)0x04,(EColorsHex)0x24,
                            (EColorsHex)0x04,(EColorsHex)0x24,(EColorsHex)0x24,
                        },
                        new EColorsHex[] {
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x00,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x00,
                            (EColorsHex)0x00,(EColorsHex)0x20,(EColorsHex)0x00,
                            (EColorsHex)0x00,(EColorsHex)0x0f,(EColorsHex)0x20,
                            (EColorsHex)0x00,(EColorsHex)0x20,(EColorsHex)0x20,
                        },
                    }
                },

                #endregion

                #region 06 Flashman

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

                #endregion

                #region 06 Wily 6

                new ColorSet() { // Wily 6 | Walls
                    addresses = new int[] { 0x017f13, 0x017f14, 0x017f15, },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {
                             (EColorsHex)0x27,(EColorsHex)0x18,(EColorsHex)0x0a,
                             (EColorsHex)0x28,(EColorsHex)0x19,(EColorsHex)0x0b,
                             (EColorsHex)0x28,(EColorsHex)0x1a,(EColorsHex)0x0c,
                             (EColorsHex)0x2a,(EColorsHex)0x1b,(EColorsHex)0x01,
                             (EColorsHex)0x2b,(EColorsHex)0x1c,(EColorsHex)0x02,
                             (EColorsHex)0x2c,(EColorsHex)0x11,(EColorsHex)0x03,
                             (EColorsHex)0x21,(EColorsHex)0x12,(EColorsHex)0x04,
                             (EColorsHex)0x22,(EColorsHex)0x13,(EColorsHex)0x05,
                             (EColorsHex)0x25,(EColorsHex)0x16,(EColorsHex)0x08,
                             (EColorsHex)0x26,(EColorsHex)0x17,(EColorsHex)0x09,
                             (EColorsHex)0x10,(EColorsHex)0x00,(EColorsHex)0x0f,
                        },
                    }
                },

                new ColorSet() { // Wily 6 | Background
                    addresses = new int[] { 0x017f18, },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] {(EColorsHex)0x0b,},
                        new EColorsHex[] {(EColorsHex)0x08, },
                        new EColorsHex[] {(EColorsHex)0x17, },
                        new EColorsHex[] {(EColorsHex)0x07, },
                        new EColorsHex[] {(EColorsHex)0x05, },
                        new EColorsHex[] {(EColorsHex)0x04, },
                        new EColorsHex[] {(EColorsHex)0x03, },
                        new EColorsHex[] {(EColorsHex)0x01, },
                        new EColorsHex[] {(EColorsHex)0x0c, },
                    }
                },

                #endregion

                #region 07 Metalman

                new ColorSet() { // Metal | Platforms
                    addresses = new int[] {
                        0x01be13, 0x01be14, 0x01be15,
                        0x01be33, 0x01be34, 0x01be35,
                        0x01be43, 0x01be44, 0x01be45,
                        0x01be53, 0x01be54, 0x01be55,
                        0x01be63, 0x01be64, 0x01be65,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] { // Default
                            (EColorsHex)0x38, (EColorsHex)0x2a, (EColorsHex)0x17,
                            (EColorsHex)0x38, (EColorsHex)0x2a, (EColorsHex)0x17,
                            (EColorsHex)0x38, (EColorsHex)0x2a, (EColorsHex)0x17,
                            (EColorsHex)0x38, (EColorsHex)0x2a, (EColorsHex)0x17,
                            (EColorsHex)0x38, (EColorsHex)0x2a, (EColorsHex)0x17,},
                        new EColorsHex[] { // Turquoise
                            (EColorsHex)0x3c, (EColorsHex)0x22, (EColorsHex)0x1d,
                            (EColorsHex)0x3c, (EColorsHex)0x22, (EColorsHex)0x1d,
                            (EColorsHex)0x3c, (EColorsHex)0x22, (EColorsHex)0x1d,
                            (EColorsHex)0x3c, (EColorsHex)0x22, (EColorsHex)0x1d,
                            (EColorsHex)0x3c, (EColorsHex)0x22, (EColorsHex)0x1d,},
                        new EColorsHex[] { // Pink
                            (EColorsHex)0x32, (EColorsHex)0x24, (EColorsHex)0x13,
                            (EColorsHex)0x32, (EColorsHex)0x24, (EColorsHex)0x13,
                            (EColorsHex)0x32, (EColorsHex)0x24, (EColorsHex)0x13,
                            (EColorsHex)0x32, (EColorsHex)0x24, (EColorsHex)0x13,
                            (EColorsHex)0x32, (EColorsHex)0x24, (EColorsHex)0x13,},
                        new EColorsHex[] { // Purple
                            (EColorsHex)0x34, (EColorsHex)0x26, (EColorsHex)0x15,
                            (EColorsHex)0x34, (EColorsHex)0x26, (EColorsHex)0x15,
                            (EColorsHex)0x34, (EColorsHex)0x26, (EColorsHex)0x15,
                            (EColorsHex)0x34, (EColorsHex)0x26, (EColorsHex)0x15,
                            (EColorsHex)0x34, (EColorsHex)0x26, (EColorsHex)0x15,},
                        new EColorsHex[] { // Yellow
                            (EColorsHex)0x36, (EColorsHex)0x28, (EColorsHex)0x17,
                            (EColorsHex)0x36, (EColorsHex)0x28, (EColorsHex)0x17,
                            (EColorsHex)0x36, (EColorsHex)0x28, (EColorsHex)0x17,
                            (EColorsHex)0x36, (EColorsHex)0x28, (EColorsHex)0x17,
                            (EColorsHex)0x36, (EColorsHex)0x28, (EColorsHex)0x17,},
                        new EColorsHex[] { // White
                            (EColorsHex)0x20, (EColorsHex)0x10, (EColorsHex)0x0f,
                            (EColorsHex)0x20, (EColorsHex)0x10, (EColorsHex)0x0f,
                            (EColorsHex)0x20, (EColorsHex)0x10, (EColorsHex)0x0f,
                            (EColorsHex)0x20, (EColorsHex)0x10, (EColorsHex)0x0f,
                            (EColorsHex)0x20, (EColorsHex)0x10, (EColorsHex)0x0f,},
                        new EColorsHex[] { // Dark Blue/Orange
                            (EColorsHex)0x1c, (EColorsHex)0x0b, (EColorsHex)0x07,
                            (EColorsHex)0x1c, (EColorsHex)0x0b, (EColorsHex)0x07,
                            (EColorsHex)0x1c, (EColorsHex)0x0b, (EColorsHex)0x07,
                            (EColorsHex)0x1c, (EColorsHex)0x0b, (EColorsHex)0x07,
                            (EColorsHex)0x1c, (EColorsHex)0x0b, (EColorsHex)0x07,},
                        new EColorsHex[] { // Blue
                            (EColorsHex)0x2c, (EColorsHex)0x22, (EColorsHex)0x11,
                            (EColorsHex)0x2c, (EColorsHex)0x22, (EColorsHex)0x11,
                            (EColorsHex)0x2c, (EColorsHex)0x22, (EColorsHex)0x11,
                            (EColorsHex)0x2c, (EColorsHex)0x22, (EColorsHex)0x11,
                            (EColorsHex)0x2c, (EColorsHex)0x22, (EColorsHex)0x11,},

                    }
                },

                //new ColorSet() { // Metal | Solid Background
                //    addresses = new int[] {
                //        0x01be21, 0x01be22,
                //        0x01be41, 0x01be51,
                //        0x01b260, 0x01be70,
                //    },
                //    ColorBytes = new List<EColorsHex[]>() {
                //        new EColorsHex[] { // Default Black
                //            (EColorsHex)0x0f, (EColorsHex)0x0f,
                //            (EColorsHex)0x0f, (EColorsHex)0x0f,
                //            (EColorsHex)0x0f, (EColorsHex)0x0f,},
                //    }
                //},

                new ColorSet() { // Metal | Background Gears
                    addresses = new int[] {
                        0x01be1f, 0x01be20,0x01be3f, 0x01be40,0x01be4f, 0x01be50,0x01be5f, 0x01be61,0x01be6f, 0x01be71,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] { // Default Dark Red
                            (EColorsHex)0x07, (EColorsHex)0x07,(EColorsHex)0x07, (EColorsHex)0x07,(EColorsHex)0x07, (EColorsHex)0x07,(EColorsHex)0x07, (EColorsHex)0x07,(EColorsHex)0x07, (EColorsHex)0x07,},
                        new EColorsHex[] { // Dark Magenta
                            (EColorsHex)0x04, (EColorsHex)0x04,(EColorsHex)0x04, (EColorsHex)0x04,(EColorsHex)0x04, (EColorsHex)0x04,(EColorsHex)0x04, (EColorsHex)0x04,(EColorsHex)0x04, (EColorsHex)0x04,},
                        new EColorsHex[] { // Dark Purple
                            (EColorsHex)0x03, (EColorsHex)0x03,(EColorsHex)0x03, (EColorsHex)0x03,(EColorsHex)0x03, (EColorsHex)0x03,(EColorsHex)0x03, (EColorsHex)0x03,(EColorsHex)0x03, (EColorsHex)0x03,},
                        new EColorsHex[] { // Dark Blue
                            (EColorsHex)0x02, (EColorsHex)0x02,(EColorsHex)0x02, (EColorsHex)0x02,(EColorsHex)0x02, (EColorsHex)0x02,(EColorsHex)0x02, (EColorsHex)0x02,(EColorsHex)0x02, (EColorsHex)0x02,},
                        new EColorsHex[] { // Dark Gray
                            (EColorsHex)0x00, (EColorsHex)0x00,(EColorsHex)0x00, (EColorsHex)0x00,(EColorsHex)0x00, (EColorsHex)0x00,(EColorsHex)0x00, (EColorsHex)0x00,(EColorsHex)0x00, (EColorsHex)0x00,},
                        new EColorsHex[] { // Dark Cyan
                            (EColorsHex)0x0C, (EColorsHex)0x0C,(EColorsHex)0x0C, (EColorsHex)0x0C,(EColorsHex)0x0C, (EColorsHex)0x0C,(EColorsHex)0x0C, (EColorsHex)0x0C,(EColorsHex)0x0C, (EColorsHex)0x0C,},
                        new EColorsHex[] { // Dark Turquoise
                            (EColorsHex)0x0B, (EColorsHex)0x0B,(EColorsHex)0x0B, (EColorsHex)0x0B,(EColorsHex)0x0B, (EColorsHex)0x0B,(EColorsHex)0x0B, (EColorsHex)0x0B,(EColorsHex)0x0B, (EColorsHex)0x0B,},
                        new EColorsHex[] { // Dark Brown
                            (EColorsHex)0x08, (EColorsHex)0x08,(EColorsHex)0x08, (EColorsHex)0x08,(EColorsHex)0x08, (EColorsHex)0x08,(EColorsHex)0x08, (EColorsHex)0x08,(EColorsHex)0x08, (EColorsHex)0x08,},
                    }
                },

                new ColorSet() { // Metal | Conveyor Animation
                    addresses = new int[] {
                        0x01be1c, 0x01be1d,
                        0x01be3c, 0x01be3d,
                        0x01be4c, 0x01be4d,
                        0x01be5c, 0x01be5d,
                        0x01be6c, 0x01be6d,
                    },
                    ColorBytes = new List<EColorsHex[]>() {
                        new EColorsHex[] { // Default
                            (EColorsHex)0x05, (EColorsHex)0x27,
                            (EColorsHex)0x27, (EColorsHex)0x05,
                            (EColorsHex)0x05, (EColorsHex)0x27,
                            (EColorsHex)0x27, (EColorsHex)0x05,
                            (EColorsHex)0x05, (EColorsHex)0x27,},
                        new EColorsHex[] { // Dark Brown, Light Green
                            (EColorsHex)0x07, (EColorsHex)0x29,
                            (EColorsHex)0x29, (EColorsHex)0x07,
                            (EColorsHex)0x07, (EColorsHex)0x29,
                            (EColorsHex)0x29, (EColorsHex)0x07,
                            (EColorsHex)0x07, (EColorsHex)0x29,},
                        new EColorsHex[] { // Dark Green, Light Turquoise
                            (EColorsHex)0x09, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x09,
                            (EColorsHex)0x09, (EColorsHex)0x2b,
                            (EColorsHex)0x2b, (EColorsHex)0x09,
                            (EColorsHex)0x09, (EColorsHex)0x2b,},
                        new EColorsHex[] { // Dark Turquoise, Light Blue
                            (EColorsHex)0x0b, (EColorsHex)0x21,
                            (EColorsHex)0x21, (EColorsHex)0x0b,
                            (EColorsHex)0x0b, (EColorsHex)0x21,
                            (EColorsHex)0x21, (EColorsHex)0x0b,
                            (EColorsHex)0x0b, (EColorsHex)0x21,},
                        new EColorsHex[] { // Dark Blue, Light Purple
                            (EColorsHex)0x01, (EColorsHex)0x23,
                            (EColorsHex)0x23, (EColorsHex)0x01,
                            (EColorsHex)0x01, (EColorsHex)0x23,
                            (EColorsHex)0x23, (EColorsHex)0x01,
                            (EColorsHex)0x01, (EColorsHex)0x23,},
                        new EColorsHex[] { // Orange, Yellow
                            (EColorsHex)0x16, (EColorsHex)0x28,
                            (EColorsHex)0x28, (EColorsHex)0x16,
                            (EColorsHex)0x16, (EColorsHex)0x28,
                            (EColorsHex)0x28, (EColorsHex)0x16,
                            (EColorsHex)0x16, (EColorsHex)0x28,},
                        new EColorsHex[] { // Dark Gray, Light Gray
                            (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00,
                            (EColorsHex)0x00, (EColorsHex)0x10,
                            (EColorsHex)0x10, (EColorsHex)0x00,
                            (EColorsHex)0x00, (EColorsHex)0x10,},
                    }
                },

                #endregion

                #region 08 Clashman

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

            #endregion
            };

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (ColorSet set in StagesColorSets)
                {
                    set.RandomizeAndWrite(stream, RandomMM2.Random);
                }
            }
        }
    }
}
