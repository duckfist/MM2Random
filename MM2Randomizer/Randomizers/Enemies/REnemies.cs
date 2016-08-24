using MM2Randomizer.Enums;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MM2Randomizer.Randomizers.Enemies
{
    /// <summary>
    /// Stage Enemy Type Randomizer
    /// </summary>
    public class REnemies
    {
        public List<EnemyType> EnemyTypes { get; set; }
        public List<EnemyInstance> AEI = new List<EnemyInstance>();
        public Dictionary<EEnemyID, EnemyType> EnemiesByType { get; set; }
        public List<SpriteBankRoomGroup> Rooms { get; set; }

        public static int Stage0EnemyScreenAddress = 0x3610;
        public static int Stage0EnemyYAddress = 0x3810;
        public static int Stage0EnemyIDAddress = 0x3910;
        public static int StageLength = 0x4000;

        public static int MAX_MOLES = 2;
        public static int MAX_PIPIS = 6;

        public int numMoles = 0;
        public int numPipis = 0;

        public REnemies()
        {
            EnemyTypes = new List<EnemyType>();
            EnemiesByType = new Dictionary<EEnemyID, EnemyType>();
            Rooms = new List<SpriteBankRoomGroup>();

            ReadEnemyInstancesFromFile();
            ChangeRoomSpriteBankSlots();
            InitializeEnemies();
            InitializeRooms();
            Randomize();
        }

        private void ReadEnemyInstancesFromFile()
        {
            using (StreamReader sr = new StreamReader("enemylist.csv"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] args = line.Split(new char[] { ',' });

                    EnemyInstance enemy = new EnemyInstance(
                        Convert.ToInt32(args[0], 16),
                        Convert.ToInt32(args[1], 16),
                        Convert.ToInt32(args[2], 16),
                        Convert.ToInt32(args[3], 16),
                        Int32.Parse(args[4]));

                    if (args.Length == 6)
                    {
                        enemy.Y = Convert.ToInt32(args[5], 16);
                    }

                    AEI.Add(enemy);
                }
            }
        }

        private void Randomize()
        {
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (SpriteBankRoomGroup room in Rooms)
                {
                    // Skip processing the room if every sprite bank row is taken
                    if (room.IsSpriteRestricted && room.SpriteBankRowsRestriction.Length >= 6)
                        continue;

                    // Create valid random combination of enemies to place
                    List<EnemyType> newEnemies = GenerateEnemyCombinations(room);

                    // Change each enemy ID for the room to a random enemy from the new enemy set
                    for (int i = 0; i < room.EnemyInstances.Length; i++)
                    {
                        int randomIndex = RandomMM2.Random.Next(newEnemies.Count);
                        EnemyType newEnemyType = newEnemies[randomIndex];

                        room.NewEnemyTypes.Add(newEnemies[randomIndex]);

                        // Change the enemy ID in the ROM
                        int IDposition = Stage0EnemyIDAddress +
                            room.EnemyInstances[i].StageNum * StageLength +
                            room.EnemyInstances[i].Offset;

                        stream.Position = IDposition;
                        stream.WriteByte((byte)newEnemies[randomIndex].ID);

                        // Change enemy Y position for position-sensitive enemies and high spawn points
                        if (!newEnemyType.ScreenEdgeOK && room.EnemyInstances[i].NeedYAdjust)
                        {
                            int newY = room.EnemyInstances[i].Y + newEnemyType.YAdjust;

                            stream.Position = Stage0EnemyYAddress +
                                room.EnemyInstances[i].StageNum * StageLength +
                                room.EnemyInstances[i].Offset;
                            stream.WriteByte((byte)newY);
                        }
                        // Adjust enemy positions for position-sensitive enemies only
                        else if (newEnemyType.YAdjust != 0)
                        {
                            int newY = room.EnemyInstances[i].YPrev + newEnemyType.YAdjust;

                            stream.Position = Stage0EnemyYAddress +
                                room.EnemyInstances[i].StageNum * StageLength +
                                room.EnemyInstances[i].Offset;
                            stream.WriteByte((byte)newY);
                        }
                        
                        // Update object with new ID for future use
                        room.EnemyInstances[i].EnemyID = (byte)newEnemies[randomIndex].ID;
                    }

                    // Change sprite banks for the room
                    stream.Position = room.PatternAddressStart;
                    foreach (EnemyType e in room.NewEnemyTypes)
                    {
                        for (int i = 0; i < e.SpriteBankRows.Count; i++)
                        {
                            stream.Position = room.PatternAddressStart + e.SpriteBankRows[i] * 2;
                            stream.WriteByte(e.PatternTableAddresses[2 * i]);
                            stream.WriteByte(e.PatternTableAddresses[2 * i + 1]);
                        }
                    }
                }
            }
        }

        public bool CheckEnemySpriteFitInBank(List<EnemyType> currentSprites, EnemyType spriteToAdd)
        {
            List<int> currentRows = new List<int>();
            List<int> currentAddresses = new List<int>();
            
            foreach (EnemyType e in currentSprites)
            {
                // Return false if enemy is already in the list
                if (spriteToAdd.ID == e.ID)
                {
                    return false;
                }

                // Return false if the room restricts changing 

                // Add the candidate enemy's sprite bank rows and pattern table addresses to their owns lists
                for (int i = 0; i < e.SpriteBankRows.Count; i++)
                {
                    currentRows.Add(e.SpriteBankRows[i]);
                }
                for (int i = 0; i < e.PatternTableAddresses.Count; i++)
                {
                    currentAddresses.Add(e.PatternTableAddresses[i]);
                }
            }
            
            for (int i = 0; i < currentRows.Count; i++)
            {
                for (int j = 0; j < spriteToAdd.SpriteBankRows.Count; j++)
                {
                    if (currentRows[i] == spriteToAdd.SpriteBankRows[j])
                    {
                        if (currentAddresses[i * 2] == spriteToAdd.PatternTableAddresses[j * 2] &&
                            currentAddresses[i * 2 + 1] == spriteToAdd.PatternTableAddresses[j * 2 + 1])
                        {
                            // This enemy contains the same pattern table address as the one in the list, add it
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void InitializeEnemies()
        {
            EnemyTypes.Add(new EnemyType(EEnemyID.Claw_Activator,
                new List<byte>() { 0x9D, 0x03 },
                new List<int>() { 3 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Tanishi,
                new List<byte>() { 0x9B, 0x03, 0x9C, 0x03 },
                new List<int>() { 0, 1 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Kerog,
                new List<byte>() { 0x9B, 0x02, 0x9C, 0x02, 0x9D, 0x02 },
                new List<int>() { 0, 1, 2 },
                false,
                -4));
            EnemyTypes.Add(new EnemyType(EEnemyID.Batton,
                new List<byte>() { 0x94, 0x02, 0x93, 0x02 },
                new List<int>() { 3, 4 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Robbit,
                new List<byte>() { 0x98, 0x02, 0x99, 0x02, 0x9A, 0x02 },
                new List<int>() { 0, 1, 2 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Monking,
                new List<byte>() { 0x98, 0x01, 0x99, 0x01, 0x9A, 0x01, 0x9B, 0x01 },
                new List<int>() { 0, 1, 2, 3 }
                )); // TODO
            EnemyTypes.Add(new EnemyType(EEnemyID.Kukku_Activator,
                new List<byte>() { 0x90, 0x01, 0x91, 0x01, 0x92, 0x01, 0x93, 0x01 },
                new List<int>() { 0, 1, 2, 3 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Telly,
                new List<byte>() { 0x93, 0x01 },
                new List<int>() { 4 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Pierrobot,
                new List<byte>() { 0x96, 0x01, 0x97, 0x01 },
                new List<int>() { 0, 1 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.FlyBoy,
                new List<byte>() { 0x94, 0x01, 0x95, 0x01 },
                new List<int>() { 0, 1 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Press,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Blocky,
                new List<byte>() { 0x9E, 0x03 },
                new List<int>() { 3 },
                false,
                -32));
            EnemyTypes.Add(new EnemyType(EEnemyID.NeoMetall,
                new List<byte>() { 0x92, 0x02, 0x9A, 0x03 },
                new List<int>() { 2, 3 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Matasaburo,
                new List<byte>() { 0x90, 0x02, 0x91, 0x02, 0x92, 0x02 },
                new List<int>() { 0, 1, 2 },
                false,
                -4));
            EnemyTypes.Add(new EnemyType(EEnemyID.Pipi_Activator,
                new List<byte>() { 0x9C, 0x01 },
                new List<int>() { 4 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.LightningGoro,
                new List<byte>() { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01 },
                new List<int>() { 0, 1, 2 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Mole_Activator,
                new List<byte>() { 0x90, 0x03 },
                new List<int>() { 4 },
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Shotman_Left,
                new List<byte>() { 0x98, 0x03, 0x99, 0x03 },
                new List<int>() { 0, 1 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.SniperArmor,
                new List<byte>() { 0x91, 0x03, 0x92, 0x03, 0x93, 0x03, 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 0, 1, 2, 3, 4 },
                false, 
                -16));
            EnemyTypes.Add(new EnemyType(EEnemyID.SniperJoe,
                new List<byte>() { 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 3, 4 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Scworm,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 },
                false, 
                8));

            // Copy enemy list to dictionary
            foreach (EnemyType e in EnemyTypes)
            {
                EnemiesByType.Add(e.ID, e);
            }
        }

        /// <summary>
        /// This method makes some preliminary modifications to the Mega Man 2 ROM to increase the enemy variety
        /// by changing the sprite banks used by certain rooms.
        /// </summary>
        public void ChangeRoomSpriteBankSlots()
        {
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = 0x00b444; // Wood 9th room, change slot from 3 to ? (0x90 special slot)
                stream.WriteByte(0x90);
                stream.Position = 0x00b445; // Wood 10th room, change slot from 3 to ? (0xa2 special slot)
                stream.WriteByte(0xa2);
                stream.Position = 0x00b446; // Wood 11th room, change slot from 3 to 0
                stream.WriteByte(0x00);
                stream.Position = 0x01743e; // Flash 3rd room, change slot from 0 to 2
                stream.WriteByte(0x24);
                stream.Position = 0x01f43d; // Clash 2nd room, change slot from 2 to 7
                stream.WriteByte(0x48);
            }
        }

        private void InitializeRooms()
        {
            // Heatman & Wily 1 stage enemies
            // Restriction: Yoku blocks, Dragon
            // NOTE: Can only use sprite banks 0-5
            // Heat Bank 3 - Heat fight
            // Heat Bank 4 - Dragon fight
            Rooms.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003470, // Bank 0
                new int[] { 0, 12 },
                AEI[0], AEI[1], AEI[2], AEI[3], AEI[4], AEI[5], AEI[6], AEI[7], AEI[8], AEI[9], AEI[10], AEI[11], AEI[12], AEI[13], AEI[14],
                AEI[40], AEI[41]));
                //0x003910, 0x003911, 0x003912, 0x003913, 0x003914, 0x003915, 0x003916, 0x003917, 0x003918, 0x003919, 0x00391A, 0x00391B, 0x00391C, 0x00391D, 0x00391E,
                //0x00395c, 0x00395d));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003494, // Bank 2
                new int[] { 1, 2 },
                new int[] { 3 }, new int[] { 0x97, 0x03 },
                AEI[15],
                AEI[16], AEI[17], AEI[18], AEI[19], AEI[20], AEI[21], AEI[22], AEI[23], AEI[24]));
                //0x00391F,
                //0x003924, 0x003925, 0x003927, 0x00392A, 0x00392B, 0x00392C, 0x00392E, 0x003931, 0x003933));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003482, // Bank 1
                new int[] { 3, 8, 9, 10 },
                AEI[25],
                AEI[37],
                AEI[38],
                AEI[39]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x0034ca, // Bank 5
                new int[] { 7 },
                AEI[26], AEI[27], AEI[28], AEI[29], AEI[30], AEI[31], AEI[32], AEI[33], AEI[34], AEI[35], AEI[36]));

            // Airman & Wily 2 stage enemies
            // Restriction: Goblins, Lightning Goros
            // Air Bank 3 - Air fight
            // Air Bank 7 - Picopico-kun fight
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007470, // Bank 0
                new int[] { 0 },
                new int[] { 0, 1, 2, 5 }, new int[] { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01, 0x96, 0x03 },
                AEI[42], AEI[43]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007494, // Bank 2
                new int[] { 1 },
                AEI[44], AEI[45]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007482, // Bank 1
                new int[] { 2 },
                new int[] { 3, 5 }, new int[] { 0x9A, 0x03, 0x96, 0x03 },
                AEI[46], AEI[47], AEI[48], AEI[49], AEI[50], AEI[51], AEI[52]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074b8, // Bank 4
                new int[] { 5 },
                AEI[53], AEI[54], AEI[55]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074ca, // Bank 5
                new int[] { 7 },
                AEI[56], AEI[57]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074dc, // Bank 6
                new int[] { 9 },
                AEI[58], AEI[59], AEI[60], AEI[61], AEI[62], AEI[63], AEI[64], AEI[65], AEI[66]));

            // Woodman & Wily 3 stage enemies
            // Restriction: Wolves (but only used in wolf rooms)
            // NOTE: Access to sprite banks 0-7, plus extra banks 0x90 and 0xA2
            // Wood Bank 6 - Wood fight
            // Wood Bank 7 - Gutsdozer fight
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B4A6, // Bank 3
                new int[] { 0 },
                AEI[67], AEI[68], AEI[69], AEI[70], AEI[71], AEI[72], AEI[73], AEI[74], AEI[75], AEI[76], AEI[77], AEI[78], AEI[79]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B482, // Bank 1
                new int[] { 1, 6 },
                AEI[80], AEI[81], AEI[82],
                AEI[83], AEI[84]));
            // Wolf rooms. When replaced with other enemies, cannot proceed due to solid tiles
            //Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B4CA,         // Bank 5
            //    new int[] { 2, 3, 4 },
            //    0x00B920, 
            //    0x00B921, 
            //    0x00B922));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B494, // Bank 2
                new int[] { 7 },
                AEI[85], AEI[86], AEI[87], AEI[88], AEI[89], AEI[90], AEI[91], AEI[92]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B4B8, // Bank 4
                new int[] { 11 },
                AEI[96], AEI[97], AEI[98], AEI[99]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b500, // Bank ? (0x90)
                new int[] { 8, 16 },
                AEI[93], // Moved Room 8 from bank 3
                AEI[100]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b512, // Bank ? (0xA2)
                new int[] { 9, 17 },
                AEI[94], // Moved Room 9 from bank 3
                AEI[101], AEI[102]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b470, // Bank 0
                new int[] { 10, 22 },
                AEI[95], // Moved Room 10 from bank 3
                AEI[103], AEI[104], AEI[105]));

            // Bubbleman & Wily 4 stage enemies
            // Restrictions: Dropping platform, Track platforms
            // Bubble Bank 3 - Bubbleman fight
            // Bubble Bank 7 - Buebeam fight
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F470, // Bank 0
                new int[] { 0, 5 },
                AEI[106], AEI[107], AEI[108],
                AEI[125], AEI[126], AEI[127], AEI[128], AEI[129], AEI[130]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F482, // Bank 1
                new int[] { 1, 2, 3 },
                AEI[109], AEI[110], AEI[111], AEI[112], AEI[113], AEI[114]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F494, // Bank 2
                new int[] { 4 },
                AEI[115], AEI[116], AEI[117], AEI[118], AEI[119], AEI[120], AEI[121], AEI[122], AEI[123], AEI[124]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4b8, // Bank 4
                new int[] { 9, 10, 13 },
                AEI[131],
                AEI[132],
                AEI[133], AEI[134]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4ca, // Bank 5
                new int[] { 15, 17 },
                new int[] { 3 }, new int[] { 0x95, 0x03 },
                AEI[135], AEI[136], AEI[137],
                AEI[138], AEI[139], AEI[140]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4dc, // Bank 6
                new int[] { 19 },
                AEI[141], AEI[142], AEI[143], AEI[144]));

            // Quick
            // Quick Bank 0 - Used in empty room only
            // Quick Bank 4 - Quick fight
            // Quick Bank 6 - W5 Teleporters
            // Quick Bank 7 - Wily Machine
            Rooms.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x0134CA, // Bank 5
                new int[] { 1, 2 },
                AEI[145], AEI[146]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x0134A6, // Bank 3
                new int[] { 3, 4, 5, 7, 8, 9, 10, 11, 12, 13 },
                AEI[147], AEI[148]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x013482, // Bank 1
                new int[] { 6 },
                AEI[149], AEI[150], AEI[151], AEI[152], AEI[153], AEI[154]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x013494, // Bank 2
                new int[] { 14 },
                AEI[155], AEI[156]));

            // Flash
            Rooms.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017470, // Bank 0
                new int[] { 0, 3, 5 },
                AEI[157], AEI[158], AEI[159], AEI[160], AEI[161], AEI[162],
                // Room 3 empty
                AEI[167]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017482, // Bank 1
                new int[] { 1, 6, 7 },
                AEI[163],
                AEI[168],
                AEI[169], AEI[170], AEI[171]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017494, // Bank 2
                new int[] { 2, 4 }, // Moved room 2 from bank 0
                AEI[164],
                AEI[165], AEI[166]));
            // Flash Bank 3: Flashman fight
            // Flash Bank 4: W6 Alien fight
            // Flash Bank 5: Wily defeated cutscene?
            // Flash Bank 6: Droplets


            // Metal
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Metal, 0x01B470,
                new int[] { 0, 1 },
                AEI[172], AEI[173], AEI[174], AEI[175], AEI[176], AEI[177], AEI[178], AEI[179], AEI[180]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Metal, 0x01B482,
                new int[] { 2 },
                AEI[181], AEI[182], AEI[183], AEI[184], AEI[185], AEI[186], AEI[187], AEI[188], AEI[189], AEI[190], AEI[191], AEI[192], AEI[193]));

            // Clash
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f494, 
                new int[] { 0, 3, 4, 5 },
                new int[] { 3 }, new int[] { 0x95, 0x03 },
                AEI[194], AEI[195], AEI[196],
                AEI[203], AEI[204], AEI[205],
                AEI[206], AEI[207], AEI[208],
                AEI[209], AEI[210], AEI[211]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f482, 
                new int[] { 2, 8, 9 },
                AEI[200], AEI[201], AEI[202],
                AEI[213],
                AEI[214]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4a6,
                new int[] { 6, 7 },
                AEI[212]));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f470,
                new int[] { 10, 11, 12 },
                AEI[215],
                AEI[216],
                AEI[217], AEI[218]));
            // Slot 4, changed from empty room 13 to room 1
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4b8,
                new int[] { 1 },
                AEI[197], AEI[198], AEI[199]
                ));
            Rooms.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4ca,
                new int[] { 14 },
                AEI[219], AEI[220], AEI[221]));
        }

        private List<EnemyType> GenerateEnemyCombinations(SpriteBankRoomGroup room)
        {
            // Create a random enemy set
            List<EnemyType> NewEnemies = new List<EnemyType>();
            List<EnemyType> PotentialEnemies = new List<EnemyType>();
            bool done = false;
            while (!done)
            {
                foreach (EnemyType en in EnemyTypes)
                {
                    // Reject enemies that have exceeded the type's maximum
                    switch (en.ID)
                    {
                        case EEnemyID.Pipi_Activator:
                            if (numPipis >= MAX_PIPIS)
                                continue;
                            numPipis++;
                            break;
                        case EEnemyID.Mole_Activator:
                            if (numMoles >= MAX_MOLES)
                                continue;
                            numMoles++;
                            break;
                        default:
                            break;
                    }

                    // Reject certain enemy types for certain stages or rooms
                    switch (room.Stage)
                    {
                        case EStageID.HeatW1:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            if (en.ID == EEnemyID.Press && room.RoomNums.Last() >= 7) continue;
                            break;
                        case EStageID.AirW2:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            break;
                        case EStageID.BubbleW4:
                            if (en.ID == EEnemyID.Mole_Activator && room.RoomNums[0] < 9) continue;
                            if (en.ID == EEnemyID.Press && room.RoomNums[0] < 9) continue;
                            break;
                        case EStageID.Clash:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            if (en.ID == EEnemyID.Press) continue;
                            break;
                        default:
                            break;
                    }

                    // If room has sprite restrictions, check if this enemy's sprite can be used
                    // (i.e. certain rooms must use certain rows on the sprite table to draw mandatory objects or effects
                    if (room.IsSpriteRestricted)
                    {
                        // Check if this enemy uses the restricted row in the sprite bank
                        List<int> commonRows = en.SpriteBankRows.Intersect(room.SpriteBankRowsRestriction).ToList();
                        if (commonRows.Count != 0)
                        {
                            // Enemy rejected: goto next enemy without adding this one
                            continue;
                            // TODO: allow placing enemies that use the same pattern table addresses for restricted rows
                        }
                            // TODO: Doesn't work yet.
                            //// If enemy uses this row, check if its sprite belongs to the same pattern table
                            //if (en.PatternTableAddresses[rowIndex * 2] == room.PatternTableAddressesRestriction[i * 2] &&
                            //    en.PatternTableAddresses[rowIndex * 2 + 1] == room.PatternTableAddressesRestriction[i * 2 + 1])
                            //{
                            //    // Enemy and the restricted sprite use the same pattern table, allow it
                            //    break;
                            //}
                            //else
                            //{
                            //    // Enemy draws with this row, but using a different set of graphics. reject it.
                            //    reject = true;
                            //    break;
                            //}
                    }

                    // Check if this enemy would fit in the sprite bank, given other new enemies already added
                    if (CheckEnemySpriteFitInBank(NewEnemies, en))
                    {
                        PotentialEnemies.Add(en);
                    }
                }

                // Unable to add any more enemies, done
                if (PotentialEnemies.Count == 0)
                {
                    done = true;
                }
                else
                {
                    NewEnemies.Add(PotentialEnemies[RandomMM2.Random.Next(PotentialEnemies.Count)]);
                    PotentialEnemies.Clear();
                }
            }

            return NewEnemies;
        }
    }
}

