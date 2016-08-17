using MM2Randomizer.Enums;

using System;
using System.Collections.Generic;
using System.IO;

namespace MM2Randomizer.Randomizers.Colors
{
    /// <summary>
    /// Stage Color Palette Randomizer
    /// </summary>
    public class RColors
    {
        List<ColorSet> StagesColorSets { get; set; }

        public RColors()
        {
            InitializeStageColorSets();

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (ColorSet set in StagesColorSets)
                {
                    set.RandomizeAndWrite(stream, RandomMM2.Random);
                }
            }
        }

        private void InitializeStageColorSets()
        {
            StagesColorSets = new List<ColorSet>()
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

        }
    }

    
}
