using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers.Enemies
{
    /// <summary>
    /// Stage Enemy Type Randomizer
    /// </summary>
    public class REnemies : IRandomizer
    {
        public static int Stage0EnemyScreenAddress = 0x3610;
        public static int Stage0EnemyYAddress = 0x3810;
        public static int Stage0EnemyIDAddress = 0x3910;
        public static int StageLength = 0x4000;

        public static double CHANCE_MOLE = 0.25;
        public static double CHANCE_PIPI = 0.4;
        public static double CHANCE_M445 = 0.4;
        public static double CHANCE_SHRINKSPAWNER = 0.25;
        public static double CHANCE_SPRINGER = 0.10;
        public static double CHANCE_TELLY = 0.15;

        public static int MAX_MOLES = 2;
        public static int MAX_PIPIS = 5;
        public static int MAX_M445S = 7;

        public List<EnemyType> EnemyTypes { get; set; } = new List<EnemyType>();
        public List<EnemyInstance> EnemyInstances { get; set; } = new List<EnemyInstance>();
        public Dictionary<EEnemyID, EnemyType> EnemiesByType { get; set; } = new Dictionary<EEnemyID, EnemyType>();
        public List<SpriteBankRoomGroup> RoomGroups { get; set; } = new List<SpriteBankRoomGroup>();

        public int numMoles = 0;
        public int numPipis = 0;
        public int numM445s = 0;

        public REnemies() { }

        public void Randomize(Patch p, Random r)
        {
            EnemyTypes.Clear();
            EnemiesByType.Clear();
            RoomGroups.Clear();
            EnemyInstances.Clear();

            ReadEnemyInstancesFromFile();
            ChangeRoomSpriteBankSlots(p);
            InitializeEnemies();
            InitializeRooms();
            Execute(p, r);
        }

        /// <summary>
        /// Read enemylist.csv to construct EnemyInstances.
        /// </summary>
        private void ReadEnemyInstancesFromFile()
        {
            string[] lines = Properties.Resources.enemylist.Split(new string[] { Environment.NewLine }, StringSplitOptions.None );

            foreach (string line in lines)
            {
                if (line.StartsWith("#")) continue; // Ignore comment lines

                string[] cols = line.Split(new char[] { ',' });

                EnemyInstance enemy = new EnemyInstance(
                    Convert.ToInt32(cols[0], 16), // Index
                    Convert.ToInt32(cols[1], 16), // StageNum
                    Convert.ToInt32(cols[2], 16), // RoomNum
                    Convert.ToInt32(cols[3], 16), // ScreenNum
                    Convert.ToBoolean(cols[4]),   // IsActive
                    Convert.ToInt32(cols[5], 16), // EnemyID
                    Convert.ToInt32(cols[6], 16), // XPosOriginal
                    Convert.ToInt32(cols[7], 16), // YPosOriginal
                    Convert.ToInt32(cols[8], 16), // YPosAir
                    Convert.ToInt32(cols[9], 16), // YPosGround
                    Convert.ToBoolean(cols[10])); // FaceRight
                EnemyInstances.Add(enemy);
            }
        }

        private void Execute(Patch Patch, Random r)
        {
            foreach (SpriteBankRoomGroup sbrg in RoomGroups)
            {
                // Skip processing the room if every sprite bank row is taken
                if (sbrg.IsSpriteRestricted && sbrg.SpriteBankRowsRestriction.Count >= 6)
                    continue;

                // Create valid random combination of enemies to place
                List<EnemyType> newEnemies = GenerateEnemyCombinations(sbrg, r);

                // No enemy can fit in this room for some reason, skip this room (GFX will be glitched)
                if (newEnemies.Count == 0)
                    continue;

                // For each enemy ID (in the room, in the room-group), change to a random enemy from the new set
                for (int i = 0; i < sbrg.Rooms.Count; i++)
                {
                    Room room = sbrg.Rooms[i];
                    for (int j = 0; j < room.EnemyInstances.Count; j++)
                    {
                        EnemyInstance instance = room.EnemyInstances[j];

                        int randomIndex = r.Next(newEnemies.Count);
                        EnemyType newEnemyType = newEnemies[randomIndex];
                        byte newId = (byte)newEnemyType.ID;

                        // When placing the last enemy, If room contains an activator, manually change the last spawn in the room to be its deactivator
                        if (j == room.EnemyInstances.Count - 1)
                        {
                            EEnemyID? activator = room.GetActivatorIfOneHasBeenAdded();
                            if (activator != null)
                            {
                                newId = (byte)EnemyType.GetCorrespondingDeactivator((EEnemyID)activator);
                            }

                            // Also, if this last instance is an activator, try to replace it
                            if (EnemyType.CheckIsActivator(newId))
                            {
                                newId = TryReplaceActivator(newEnemies, newId);
                                newEnemyType = newEnemies.Where(x => (byte)x.ID == newId).First();
                            }
                        }

                        sbrg.NewEnemyTypes.Add(newEnemyType); // TODO: This adds a bunch of extra unnecessary copies. Change.

                        // If room contains only this one enemy and it is an activator
                        // TODO: How does Clash stage work with the Pipis? They don't break normally.
                        if ((room.EnemyInstances.Count == 1 && instance.HasNewActivator()))
                        {
                            // Try to replace it with a non-activator enemy
                            //newId = TryReplaceActivator(newEnemies, newId);
                        }

                        // Last-minute adjustments to certain enemy spawns
                        switch ((EEnemyID)newId)
                        {
                            case EEnemyID.Shrink:
                                double randomSpawner = r.NextDouble();
                                if (randomSpawner < CHANCE_SHRINKSPAWNER)
                                {
                                    newId = (byte)EEnemyID.Shrink_Spawner;
                                }
                                break;
                            case EEnemyID.Shotman_Left:
                                if (instance.IsFaceRight)
                                {
                                    newId = (byte)EEnemyID.Shotman_Right;
                                }
                                break;
                            default: break;
                        }

                        // Update object with new ID for future use
                        room.EnemyInstances[j].EnemyID = newId;

                        // Change the enemy ID in the ROM
                        int IDposition = Stage0EnemyIDAddress +
                            instance.StageNum * StageLength +
                            instance.Offset;

                        Patch.Add(IDposition, newId, $"{sbrg.Stage.ToString("G")} Stage Enemy #{instance.Offset} ID (Room {instance.RoomNum}) {((EEnemyID)instance.EnemyID).ToString("G")}");

                        // Change the enemy Y pos based on Air or Ground category
                        int newY = newEnemyType.YAdjust;
                        newY += (newEnemyType.IsYPosAir) ? instance.YAir : instance.YGround;
                        IDposition = Stage0EnemyYAddress +
                            instance.StageNum * StageLength +
                            instance.Offset;
                        Patch.Add(IDposition, (byte)newY, $"{sbrg.Stage.ToString("G")} Stage Enemy #{instance.Offset} Y (Room {instance.RoomNum}) {((EEnemyID)instance.EnemyID).ToString("G")}");
                    }
                }

                // Change sprite banks for the room
                foreach (EnemyType e in sbrg.NewEnemyTypes)
                {
                    for (int i = 0; i < e.SpriteBankRows.Count; i++)
                    {
                        int rowInSlotAddress = sbrg.PatternAddressStart + e.SpriteBankRows[i] * 2;
                        int patternTblPtr1 = e.PatternTableAddresses[2 * i];
                        int patternTblPtr2 = e.PatternTableAddresses[2 * i + 1];

                        Patch.Add(rowInSlotAddress,     (byte)patternTblPtr1, $"{sbrg.Stage.ToString("G")} Stage Sprite Bank Slot ? Row {e.SpriteBankRows[i]} Indirect Address 1");
                        Patch.Add(rowInSlotAddress + 1, (byte)patternTblPtr2, $"{sbrg.Stage.ToString("G")} Stage Sprite Bank Slot ? Row {e.SpriteBankRows[i]} Indirect Address 2");
                    }
                }
            } // end foreach sbrg
        }

        public byte TryReplaceActivator(List<EnemyType> newEnemies, byte id)
        {
            byte newId = id;
            bool foundNonActivator = false;
            foreach (EnemyType enemyType in newEnemies)
            {
                if (!enemyType.IsActivator)
                {
                    newId = (byte)enemyType.ID;
                    foundNonActivator = true;
                    break;
                }
            }

            // Otherwise, switch to its corresponding deactivator(room will appear empty)
            if (!foundNonActivator)
            {
                newId = (byte)EnemyType.GetCorrespondingDeactivator(id);
            }

            return newId;
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
                true,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Tanishi,
                new List<byte>() { 0x9B, 0x03, 0x9C, 0x03 },
                new List<int>() { 0, 1 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.Kerog,
                new List<byte>() { 0x9B, 0x02, 0x9C, 0x02, 0x9D, 0x02 },
                new List<int>() { 0, 1, 2 },
                false,
                false,
                -4));
            EnemyTypes.Add(new EnemyType(EEnemyID.Batton,
                new List<byte>() { 0x94, 0x02, 0x93, 0x02 },
                new List<int>() { 3, 4 },
                false,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Robbit,
                new List<byte>() { 0x98, 0x02, 0x99, 0x02, 0x9A, 0x02 },
                new List<int>() { 0, 1, 2 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.Monking,
                new List<byte>() { 0x98, 0x01, 0x99, 0x01, 0x9A, 0x01, 0x9B, 0x01 },
                new List<int>() { 0, 1, 2, 3 },
                false)); // TODO
            EnemyTypes.Add(new EnemyType(EEnemyID.Kukku_Activator,
                new List<byte>() { 0x90, 0x01, 0x91, 0x01, 0x92, 0x01, 0x93, 0x01 },
                new List<int>() { 0, 1, 2, 3 },
                true,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Telly,
                new List<byte>() { 0x93, 0x01 },
                new List<int>() { 4 },
                false,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Pierrobot,
                new List<byte>() { 0x96, 0x01, 0x97, 0x01 },
                new List<int>() { 0, 1 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.FlyBoy,
                new List<byte>() { 0x94, 0x01, 0x95, 0x01 },
                new List<int>() { 0, 1 },
                false,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Press,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 },
                false,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Blocky,
                new List<byte>() { 0x9E, 0x03 },
                new List<int>() { 3 },
                false,
                false,
                -32));
            EnemyTypes.Add(new EnemyType(EEnemyID.NeoMetall,
                new List<byte>() { 0x92, 0x02, 0x9A, 0x03 },
                new List<int>() { 2, 3 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.Matasaburo,
                new List<byte>() { 0x90, 0x02, 0x91, 0x02, 0x92, 0x02 },
                new List<int>() { 0, 1, 2 },
                false,
                false,
                -4));
            EnemyTypes.Add(new EnemyType(EEnemyID.Pipi_Activator,
                new List<byte>() { 0x9C, 0x01 },
                new List<int>() { 4 },
                true,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.LightningGoro,
                new List<byte>() { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01 },
                new List<int>() { 0, 1, 2 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.Mole_Activator,
                new List<byte>() { 0x90, 0x03 },
                new List<int>() { 4 },
                true,
                true));
            EnemyTypes.Add(new EnemyType(EEnemyID.Shotman_Left,
                new List<byte>() { 0x98, 0x03, 0x99, 0x03 },
                new List<int>() { 0, 1 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.SniperArmor,
                new List<byte>() { 0x91, 0x03, 0x92, 0x03, 0x93, 0x03, 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 0, 1, 2, 3, 4 },
                false,
                false,
                -16));
            EnemyTypes.Add(new EnemyType(EEnemyID.SniperJoe,
                new List<byte>() { 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 3, 4 },
                false));
            EnemyTypes.Add(new EnemyType(EEnemyID.Scworm,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 },
                false,
                false,
                8));
            EnemyTypes.Add(new EnemyType(EEnemyID.Springer,
                new List<byte>() { 0x9F, 0x03 },
                new List<int>() { 5 },
                false,
                false,
                4));
            //EnemyTypes.Add(new EnemyType(EEnemyID.PetitGoblin,
            //    new List<byte>() { 0x96, 0x03 },
            //    new List<int>() { 5 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.Shrink,
                new List<byte>() { 0x9E, 0x02, 0x9F, 0x02 },
                new List<int>() { 0, 1 },
                false));
            //EnemyTypes.Add(new EnemyType(EEnemyID.BigFish,
            //    new List<byte>() { 0x94, 0x04, 0x95, 0x04, 0x96, 0x04, 0x97, 0x04 },
            //    new List<int>() { 0, 1, 2, 3 }));
            EnemyTypes.Add(new EnemyType(EEnemyID.M445_Activator,
                new List<byte>() { 0x97, 0x02 },
                new List<int>() { 2 },
                true,
                true));

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
        public void ChangeRoomSpriteBankSlots(Patch p)
        {
            p.Add(0x00b444, 0x90, "Custom Sprite Bank: Wood 9th room: slot 3->? (0x90 special slot)");
            p.Add(0x00b445, 0xa2, "Custom Sprite Bank: Wood 10th room: slot 3->? (0xa2 special slot)");
            p.Add(0x00b446, 0x00, "Custom Sprite Bank: Wood 11th room: slot 3->0");
            p.Add(0x01743e, 0x24, "Custom Sprite Bank: Flash 3rd room: slot 0->2");
            p.Add(0x01f43d, 0x48, "Custom Sprite Bank: Clash 2nd room: slot 2->7");
        }

        private void InitializeRooms()
        {
            // First, create a list of every room-group that refers to a specifc Sprite Bank Slot.
            
            // Heatman & Wily 1 stage enemies
            // NOTE: Can only use sprite banks 0-5
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003470, new int[] { 0, 12 })); // Bank 0
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003482, new int[] { 3, 8, 9, 10 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x003494, new int[] { 1, 2 },    // Bank 2
                new int[] { 3 }, new byte[] { 0x97, 0x03 })); // Force Yoku blocks
            // Heat Bank 3 - Heat fight 0x0034A6
            // Heat Bank 4 - Dragon fight
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.HeatW1, 0x0034ca, new int[] { 7 })); // Bank 5

            // Airman & Wily 2 stage enemies
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007470, new int[] { 0 }, // Bank 0
                new int[] { 0, 1, 2/*, 3, 5 */}, new byte[] { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01/*, 0x9A, 0x03, 0x96, 0x03 */})); // Force Goblins, Lightning Goro
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007482, new int[] { 2 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x007494, new int[] { 1 })); // Bank 2
            // Air Bank 3 - Air fight 0x0074A6
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074b8, new int[] { 5 })); // Bank 4
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074ca, new int[] { 7 })); // Bank 5
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.AirW2, 0x0074dc, new int[] { 9 })); // Bank 6
            // Air Bank 7 - Picopico-kun fight

            // Woodman & Wily 3 stage enemies
            // NOTE: Access to sprite banks 0-7, plus extra banks 0x90 and 0xA2
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b470, new int[] { 10, 22 })); // Bank 0; Moved Room 10 from bank 3
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B482, new int[] { 1, 6 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B494, new int[] { 7 })); // Bank 2
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B4A6, new int[] { 0 })); // Bank 3
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00B4B8, new int[] { 11 })); // Bank 4
            // Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B4CA, new int[] { 2, 3, 4 })); // Bank 5 - Friender rooms
            // Wood Bank 6 - Wood fight 0x00B4DC
            // Wood Bank 7 - Gutsdozer fight
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b500, new int[] { 8, 16 })); // Bank ? (0x90); Moved Room 8 from bank 3
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.WoodW3, 0x00b512, new int[] { 9, 17 })); // Bank ? (0xA2); Moved Room 9 from bank 3

            // Bubbleman & Wily 4 stage enemies
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F470, new int[] { 0, 5 }, // Bank 0
                new int[] { 2 }, new byte[] { 0x9D, 0x02 })); // Falling platform sprite
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F482, new int[] { 1, 2, 3 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00F494, new int[] { 4 }, // Bank 2
                new int[] { 0, 1 }, new byte[] { 0x9E, 0x02, 0x9F, 0x02 })); // Shrimp sprites
            // Bubble Bank 3 - Bubbleman fight 0x00F4A6
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4b8, new int[] { 9, 10, 13 })); // Bank 4
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4ca, new int[] { 15, 17 }, // Bank 5
                new int[] { 3 }, new byte[] { 0x95, 0x03 })); // Moving platform sprite
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.BubbleW4, 0x00f4dc, new int[] { 19 })); // Bank 6

            // Quick
            // Quick Bank 0 - Used in empty room only
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x013482, new int[] { 7 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x013494, new int[] { 15 })); // Bank 2
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x0134A6, new int[] { 3, 4, 5, 8, 9, 10, 11, 12, 13, 14 })); // Bank 3
            // Quick Bank 4 - Quick fight // 0x0134B8
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.QuickW5, 0x0134CA, new int[] { 1, 2 })); // Bank 5
            // Quick Bank 6 - W5 Teleporters
            // Quick Bank 7 - Wily Machine

            // Flash
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017470, new int[] { 0, 3, 5 })); // Bank 0
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017482, new int[] { 1, 6, 7 })); // Bank 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.FlashW6, 0x017494, new int[] { 2, 4 })); // Bank 2; Moved room 2 from bank 0
            // Flash Bank 3: Flashman fight 0x0174A6
            // Flash Bank 4: W6 Alien fight
            // Flash Bank 5: Wily defeated cutscene?
            // Flash Bank 6: Droplets

            // Metal
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Metal, 0x01B470, new int[] { 0, 1 }));
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Metal, 0x01B482, new int[] { 2 }));
            // Metal fight 0x01B494

            // Clash
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f494, new int[] { 0, 3, 4, 5 },
                new int[] { 3 }, new byte[] { 0x95, 0x03 })); // Moving platform sprites
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f482, new int[] { 2, 8, 9 }));
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4a6, new int[] { 6, 7 }));
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f470, new int[] { 10, 11, 12 }));
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4b8, new int[] { 1 })); // Slot 4, changed from empty room 13 to room 1
            RoomGroups.Add(new SpriteBankRoomGroup(EStageID.Clash, 0x01f4ca, new int[] { 14 }));
            // Clash fight 0x01F4DC

            // Get copy of enemy spawn list to save time
            List <EnemyInstance> usedInstances = new List<EnemyInstance>(EnemyInstances);

            // First, loop back through entire list of room-groups. For each, loop through entire list of 
            // enemies, match them and assign them to their room-group. Assigned enemies are removed from
            // the list, reducing the search time on each loop. For now, completely discard all enemy spawns
            // that are reserved (i.e. Yoku blocks in Heat, Lightning Goros in Air).
            foreach (SpriteBankRoomGroup sbrg in RoomGroups)
            {
                int stageNum = (int)sbrg.Stage;

                // Find index of first enemy instance in the stage
                int i = 0;
                for (i = 0; i < usedInstances.Count; i++)
                    if (usedInstances.ElementAt(i).StageNum == stageNum)
                        break;

                // Check each applicable room number for this room group
                for (int roomIndex = 0; roomIndex < sbrg.Rooms.Count; roomIndex++)
                {
                    Room room = sbrg.Rooms[roomIndex];

                    // Find first occurrence of this room num in enemy list (starting at this stage)
                    int j = 0;
                    while (i + j < usedInstances.Count)
                    {
                        EnemyInstance checkEnemy = usedInstances.ElementAt(i + j);

                        // Only add non-required enemy instances to the randomizer
                        if (checkEnemy.IsActive)
                        {
                            // Enemy instance stage/room num match one described in a SpriteBankRoomGroup
                            if (checkEnemy.RoomNum == room.RoomNum && checkEnemy.StageNum == stageNum)
                            {
                                // Add enemy to room group 
                                room.EnemyInstances.Add(checkEnemy);

                                // Remove enemy from temporary list
                                usedInstances.RemoveAt(i + j);
                            }
                            else
                            {
                                // Only inc j here since an element hasn't been removed
                                j++;
                            }
                        }
                        else
                        {
                            // Remove unrandomizable enemy from temporary list
                            usedInstances.RemoveAt(i + j); 
                        }
                    } // end while
                } // end foreach roomNum in sbrg
            } // end foreach sbrg
        }

        private List<EnemyType> GenerateEnemyCombinations(SpriteBankRoomGroup sbrg, Random r)
        {
            // Create a random enemy set
            List<EnemyType> NewEnemies = new List<EnemyType>();
            List<EnemyType> PotentialEnemies = new List<EnemyType>();
            bool done = false;
            bool hasActivator = false;
            while (!done)
            {
                foreach (EnemyType en in EnemyTypes)
                {
                    // 1. Skip enemies that have exceeded the type's maximum
                    // 2. Reduce the overall chance of certain types appearing by randomly skipping them
                    // 3. Limit a room set to having at most one Activator enemy type
                    double chance = 0.0;
                    switch (en.ID)
                    {
                        case EEnemyID.Pipi_Activator:
                            if (numPipis >= MAX_PIPIS)
                                continue;
                            chance = r.NextDouble();
                            if (chance > CHANCE_PIPI)
                                continue;
                            break;
                        case EEnemyID.Mole_Activator:
                            if (numMoles >= MAX_MOLES)
                                continue;
                            chance = r.NextDouble();
                            if (chance > CHANCE_MOLE)
                                continue;
                            break;
                        case EEnemyID.M445_Activator:
                            if (numM445s > MAX_M445S)
                                continue;
                            chance = r.NextDouble();
                            if (chance > CHANCE_M445)
                                continue;
                            break;
                        case EEnemyID.Telly:
                            chance = r.NextDouble();
                            if (chance > CHANCE_TELLY)
                                continue;
                            break;
                        case EEnemyID.Springer:
                            chance = r.NextDouble();
                            if (chance > CHANCE_SPRINGER)
                                continue;
                            break;
                        default:
                            break;
                    }

                    // Skip any additional activator enemies
                    if (en.IsActivator && hasActivator)
                    {
                        continue;
                    }

                    // Reject certain enemy types for certain stages or rooms
                    switch (sbrg.Stage)
                    {
                        case EStageID.HeatW1:
                            // Moles don't display correctly in Heat or Wily 1. Also too annoying in Heat Yoku room.
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            // Reject Pipis appearing in Yoku block room
                            if (en.ID == EEnemyID.Pipi_Activator && sbrg.ContainsRoom(2)) continue;
                            // Reject M445s appearing in Yoku block room
                            if (en.ID == EEnemyID.M445_Activator && sbrg.ContainsRoom(2)) continue;
                            // Press doesn't display correctly in Wily 1
                            if (en.ID == EEnemyID.Press && sbrg.Rooms.Last().RoomNum >= 7) continue;
                            break;
                        case EStageID.AirW2:
                            // Moles don't display correctly in Heat
                            if (en.ID == EEnemyID.Mole_Activator && sbrg.Rooms[0].RoomNum < 7) continue;
                            break;
                        case EStageID.WoodW3:
                            // Moles and Press don't display in Wood outside room
                            if (en.ID == EEnemyID.Mole_Activator && sbrg.ContainsRoom(7)) continue;
                            if (en.ID == EEnemyID.Press && sbrg.ContainsRoom(7)) continue;
                            // Don't spawn Springer, Blocky, or Press underwater
                            if (en.ID == EEnemyID.Springer && sbrg.ContainsRoom(0x11)) continue;
                            if (en.ID == EEnemyID.Blocky && sbrg.ContainsRoom(0x11)) continue;
                            if (en.ID == EEnemyID.Press && sbrg.ContainsRoom(0x11)) continue;
                            break;
                        case EStageID.BubbleW4:
                            // Moles don't display correctly in Bubble
                            if (en.ID == EEnemyID.Mole_Activator && sbrg.Rooms[0].RoomNum < 9) continue;
                            // Press doesn't display correctly in Bubble
                            if (en.ID == EEnemyID.Press && sbrg.Rooms[0].RoomNum < 9) continue;
                            // Don't spawn Springer or Blocky underwater
                            if (en.ID == EEnemyID.Springer && (sbrg.ContainsRoom(3) || sbrg.ContainsRoom(4))) continue;
                            if (en.ID == EEnemyID.Blocky && (sbrg.ContainsRoom(3) || sbrg.ContainsRoom(4))) continue;
                            break;
                        case EStageID.Clash:
                            // Mole bad GFX
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            // Press bad GFX
                            if (en.ID == EEnemyID.Press) continue;
                            break;
                        default:
                            break;
                    }

                    // If room has sprite restrictions, check if this enemy's sprite can be used
                    // (i.e. certain rooms must use certain rows on the sprite table to draw mandatory objects or effects
                    if (sbrg.IsSpriteRestricted)
                    {
                        // Check if this enemy uses the restricted row in the sprite bank
                        List<int> commonRows = en.SpriteBankRows.Intersect(sbrg.SpriteBankRowsRestriction).ToList();
                        if (commonRows.Count != 0)
                        {
                            bool reject = false;
                            for (int i = 0; i < en.SpriteBankRows.Count; i++)
                            {
                                int enemyRow = en.SpriteBankRows[i];
                                int indexOfRow = sbrg.SpriteBankRowsRestriction.IndexOf(enemyRow);

                                // For a restricted sprite bank row, see if enemy uses the same sprite pattern
                                if (indexOfRow > -1)
                                {
                                    if (en.PatternTableAddresses[i * 2] == sbrg.PatternTableAddressesRestriction[indexOfRow * 2] &&
                                        en.PatternTableAddresses[i * 2 + 1] == sbrg.PatternTableAddressesRestriction[indexOfRow * 2 + 1])
                                    {
                                        // Enemy and the restricted sprite use the same pattern table, allow it
                                        // (do nothing)
                                    }
                                    else
                                    {
                                        // Enemy draws with this row, but using a different set of graphics. reject it.
                                        reject = true;
                                        break;
                                    }
                                }
                            }

                            if (reject)
                            {
                                continue;
                            }
                        }
                            
                    }

                    // Check if this enemy would fit in the sprite bank, given other new enemies already added
                    if (CheckEnemySpriteFitInBank(NewEnemies, en))
                    {
                        // Add enemy to set of possible enemies to place in 
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
                    // Choose a new enemy to add to the set from all possible new enemies to add
                    EnemyType newEnemy = PotentialEnemies[r.Next(PotentialEnemies.Count)];
                    NewEnemies.Add(newEnemy);
                    PotentialEnemies.Clear();

                    // Increase total count of certain enemy types to limit their appearance later
                    switch (newEnemy.ID)
                    {
                        case EEnemyID.Pipi_Activator:
                            numPipis++;
                            break;
                        case EEnemyID.Mole_Activator:
                            numMoles++;
                            break;
                        case EEnemyID.M445_Activator:
                            numM445s++;
                            break;
                        default:
                            break;
                    }

                    // Flag the new enemy set as having an activator so that no more will be added
                    if (newEnemy.IsActivator)
                    {
                        hasActivator = true;
                    }
                }
            }

            return NewEnemies;
        }
    }
}

