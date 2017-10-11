using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class RBossRoom : IRandomizer
    {
        class BossRoomRandomComponent
        {
            // This seems to be every table necessary to shuffle for getting a boss
            // to function and display properly in a different boss room.
            public byte IntroValue { get; set; }    // 0F 0F 0B 05 09 07 05 03
            public byte AIPtrByte1 { get; set; }    // C5 E3 FB 56 9E 56 20 C3
            public byte AIPtrByte2 { get; set; }    // 80 82 84 86 87 89 8B 8C
            public byte GfxFix1 { get; set; }       // 50 66 6C 60 54 5A 63 69
            public byte GfxFix2 { get; set; }       // 51 67 6D 61 55 5C 64 6A
            public byte YPosFix1 { get; set; }      // 09 0C 0F 0A 09 09 08 08
            public byte YPosFix2 { get; set; }      // 0C 10 10 0C 0C 0C 0C 0C
            public byte[] SpriteBankSlotRowsBytes { get; set; }
            // ...and maybe room layout???

            public BossRoomRandomComponent(byte introValue, byte aiPtr1, byte aiPtr2, byte gfxfix1, byte gfxfix2, byte yFix1, byte yFix2, byte[] spriteBankSlotRows)
            {
                this.IntroValue = introValue;
                this.AIPtrByte1 = aiPtr1;
                this.AIPtrByte2 = aiPtr2;
                this.GfxFix1 = gfxfix1;
                this.GfxFix2 = gfxfix2;
                this.YPosFix1 = yFix1;
                this.YPosFix2 = yFix2;
                this.SpriteBankSlotRowsBytes = spriteBankSlotRows;
            }
        }
        
        List<BossRoomRandomComponent> Components;
        
        public RBossRoom() { }

        /// <summary>
        /// Shuffle which Robot Master awards which weapon.
        /// </summary>
        public void Randomize(Patch Patch, Random r)
        {
            Components = new List<BossRoomRandomComponent>
            {
                // Heat Man
                new BossRoomRandomComponent(
                    introValue  : 0x0F, // 0x02C15E
                    aiPtr1      : 0xC5, // 0x02C057
                    aiPtr2      : 0x80, // 0x02C065
                    gfxfix1     : 0x50, // 0x02E4E9
                    gfxfix2     : 0x51, // 0x02C166
                    yFix1       : 0x09, // 0x02C14E
                    yFix2       : 0x0C, // 0x02C156
                    spriteBankSlotRows : new byte[] {
                        0x98, 0x06,
                        0x99, 0x06,
                        0x9A, 0x06,
                        0x9B, 0x06,
                        0x9C, 0x06,
                        0x9D, 0x06,
                    }),

                // Air Man
                new BossRoomRandomComponent(
                    introValue  : 0x0F,
                    aiPtr1      : 0xE3,
                    aiPtr2      : 0x82,
                    gfxfix1     : 0x66,
                    gfxfix2     : 0x67,
                    yFix1       : 0x0C,
                    yFix2       : 0x10,
                    spriteBankSlotRows : new byte[] {
                        0xAB, 0x05,
                        0xAC, 0x05,
                        0xAD, 0x05,
                        0xAA, 0x06,
                        0xAB, 0x06,
                        0xAC, 0x06,
                    }),

                // Wood Man
                new BossRoomRandomComponent(
                    introValue  : 0x0B,
                    aiPtr1      : 0xFB,
                    aiPtr2      : 0x84,
                    gfxfix1     : 0x6C,
                    gfxfix2     : 0x6D,
                    yFix1       : 0x0F,
                    yFix2       : 0x10,
                    spriteBankSlotRows : new byte[] {
                        0xAC, 0x06,
                        0xAD, 0x06,
                        0xAE, 0x06,
                        0xAF, 0x06,
                        0xB0, 0x06,
                        0xB1, 0x06,
                    }),

                // Bubble Man
                new BossRoomRandomComponent(
                    introValue  : 0x05,
                    aiPtr1      : 0x56,
                    aiPtr2      : 0x86,
                    gfxfix1     : 0x60,
                    gfxfix2     : 0x61,
                    yFix1       : 0x0A,
                    yFix2       : 0x0C,
                    spriteBankSlotRows : new byte[] {
                        0x98, 0x07,
                        0x99, 0x07,
                        0x9A, 0x07,
                        0x9B, 0x07,
                        0x9C, 0x07,
                        0x9D, 0x07,
                    }),

                // Quick Man
                new BossRoomRandomComponent(
                    introValue  : 0x09,
                    aiPtr1      : 0x9E,
                    aiPtr2      : 0x87,
                    gfxfix1     : 0x54,
                    gfxfix2     : 0x55,
                    yFix1       : 0x09,
                    yFix2       : 0x0C,
                    spriteBankSlotRows : new byte[] {
                        0x90, 0x07,
                        0x91, 0x07,
                        0x92, 0x07,
                        0x93, 0x07,
                        0x94, 0x07,
                        0x95, 0x07,
                    }),

                // Flash Man
                new BossRoomRandomComponent(
                    introValue  : 0x07,
                    aiPtr1      : 0x56,
                    aiPtr2      : 0x89,
                    gfxfix1     : 0x5A,
                    gfxfix2     : 0x5C,
                    yFix1       : 0x09,
                    yFix2       : 0x0C,
                    spriteBankSlotRows : new byte[] {
                        0x9E, 0x06,
                        0x9F, 0x06,
                        0x96, 0x07,
                        0x97, 0x07,
                        0x9E, 0x07,
                        0x9F, 0x07,
                    }),

                // Metal Man
                new BossRoomRandomComponent(
                    introValue  : 0x05,
                    aiPtr1      : 0x20,
                    aiPtr2      : 0x8B,
                    gfxfix1     : 0x63,
                    gfxfix2     : 0x64,
                    yFix1       : 0x08,
                    yFix2       : 0x0C,
                    spriteBankSlotRows : new byte[] {
                        0xB0, 0x03,
                        0xB1, 0x03,
                        0xB2, 0x03,
                        0xB3, 0x03,
                        0xAA, 0x05,
                        0xAB, 0x05,
                    }),

                // Clash Man
                new BossRoomRandomComponent(
                    introValue  : 0x03,
                    aiPtr1      : 0xC3,
                    aiPtr2      : 0x8C,
                    gfxfix1     : 0x69,
                    gfxfix2     : 0x6A,
                    yFix1       : 0x08,
                    yFix2       : 0x0C,
                    spriteBankSlotRows : new byte[] {
                        0xAE, 0x05,
                        0xAF, 0x05,
                        0xB0, 0x05,
                        0xB1, 0x05,
                        0xB2, 0x05,
                        0xB3, 0x05,
                    }
                ),
            };

            Components.Shuffle(r);

            // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
            for (int i = 0; i < 8; i++)
            {
                var bossroom = Components[i];
                Patch.Add(0x02C15E + i, bossroom.IntroValue, $"Boss Intro Value for Boss Room {i}");
                Patch.Add(0x02C057 + i, bossroom.AIPtrByte1, $"Boss AI Ptr Byte1 for Boss Room {i}");
                Patch.Add(0x02C065 + i, bossroom.AIPtrByte2, $"Boss AI Ptr Byte2 for Boss Room {i}");
                Patch.Add(0x02E4E9 + i, bossroom.GfxFix1, $"Boss GFX Fix 1 for Boss Room {i}");
                Patch.Add(0x02C166 + i, bossroom.GfxFix1, $"Boss GFX Fix 2 for Boss Room {i}");
                Patch.Add(0x02C14E + i, bossroom.YPosFix1, $"Boss Y-Pos Fix1 for Boss Room {i}");
                Patch.Add(0x02C156 + i, bossroom.YPosFix2, $"Boss Y-Pos Fix2 for Boss Room {i}");
            }

            // Adjust sprite banks for each boss room
            int[] spriteBankBossRoomAddresses = new int[]
            {
                0x0034A6, // Heat room
                0x0074A6, // Air room
                0x00B4DC, // Wood room
                0x00F4A6, // Bubble room
                0x0134B8, // Quick room
                0x0174A6, // Flash room
                0x01B494, // Metal room
                0x01F4DC, // Clash room
            };
            for (int i = 0; i < spriteBankBossRoomAddresses.Length; i++)
            {
                for (int j = 0; j < Components[i].SpriteBankSlotRowsBytes.Length; j++)
                {
                    Patch.Add(spriteBankBossRoomAddresses[i] + j, 
                        Components[i].SpriteBankSlotRowsBytes[j], 
                        $"Boss Room {i} Sprite Bank Swap {j}");
                }
            }
            
        }


    }
}
