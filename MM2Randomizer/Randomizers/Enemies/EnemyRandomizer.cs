using MM2Randomizer.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyRandomizer
    {
        public List<Enemy> Enemies { get; set; }
        public Dictionary<EEnemyID, Enemy> EnemiesByType { get; set; }
        public List<EnemyRoom> Rooms { get; set; }

        public static int MAX_MOLES = 2;
        public static int MAX_PIPIS = 6;

        public int numMoles = 0;
        public int numPipis = 0;

        public EnemyRandomizer()
        {
            Enemies = new List<Enemy>();
            EnemiesByType = new Dictionary<EEnemyID, Enemy>();
            Rooms = new List<EnemyRoom>();

            InitializeEnemies();
            InitializeRooms();
            Randomize();
        }

        private void Randomize()
        {
            foreach (EnemyRoom room in Rooms)
            {
                // Skip processing the room if every sprite bank row is taken
                if (room.IsSpriteRestricted && room.SpriteBankRowsRestriction.Length >= 6)
                    continue;

                // Create valid random combination of enemies to place
                List<Enemy> newEnemies = GenerateEnemyCombinations(room);

                // Change each enemy ID for the room to a random enemy from the new enemy set
                for (int i = 0; i < room.EnemyIDAddresses.Length; i++)
                {
                    int randomIndex = RandomMM2.Random.Next(newEnemies.Count);
                    Enemy newEnemy = newEnemies[randomIndex];

                    room.NewEnemyInstances.Add(newEnemies[randomIndex]);

                    using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = room.EnemyIDAddresses[i];
                        stream.WriteByte((byte)newEnemies[randomIndex].ID);
                    }
                }

                // Change sprite banks for the room
                using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    stream.Position = room.PatternAddressStart;
                    foreach (Enemy e in room.NewEnemyInstances)
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

        public bool CheckEnemySpriteFitInBank(List<Enemy> currentSprites, Enemy spriteToAdd)
        {
            List<int> currentRows = new List<int>();
            List<int> currentAddresses = new List<int>();
            
            foreach (Enemy e in currentSprites)
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
            Enemies.Add(new Enemy(EEnemyID.Claw_Activator,
                new List<byte>() { 0x9D, 0x03 },
                new List<int>() { 3 }));
            Enemies.Add(new Enemy(EEnemyID.Tanishi,
                new List<byte>() { 0x9B, 0x03, 0x9C, 0x03 },
                new List<int>() { 0, 1 }, 
                20)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Kerog,
                new List<byte>() { 0x9B, 0x02, 0x9C, 0x02, 0x9D, 0x02 },
                new List<int>() { 0, 1, 2 },
                32)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Batton,
                new List<byte>() { 0x94, 0x02, 0x93, 0x02 },
                new List<int>() { 3, 4 },
                16)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Robbit,
                new List<byte>() { 0x98, 0x02, 0x99, 0x02, 0x9A, 0x02 },
                new List<int>() { 0, 1, 2 },
                32)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Monking,
                new List<byte>() { 0x98, 0x01, 0x99, 0x01, 0x9A, 0x01, 0x9B, 0x01 },
                new List<int>() { 0, 1, 2, 3 },
                32)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Kukku_Activator,
                new List<byte>() { 0x90, 0x01, 0x91, 0x01, 0x92, 0x01, 0x93, 0x01 },
                new List<int>() { 0, 1, 2, 3 }));
            Enemies.Add(new Enemy(EEnemyID.Telly,
                new List<byte>() { 0x93, 0x01 },
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(EEnemyID.Pierrobot,
                new List<byte>() { 0x96, 0x01, 0x97, 0x01 },
                new List<int>() { 0, 1 },
                32)); // Guess
            Enemies.Add(new Enemy(EEnemyID.FlyBoy,
                new List<byte>() { 0x94, 0x01, 0x95, 0x01 },
                new List<int>() { 0, 1 }));
            Enemies.Add(new Enemy(EEnemyID.Press,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 }));
            Enemies.Add(new Enemy(EEnemyID.Blocky,
                new List<byte>() { 0x9E, 0x03 },
                new List<int>() { 3 },
                64)); // Guess
            Enemies.Add(new Enemy(EEnemyID.NeoMetall,
                new List<byte>() { 0x92, 0x02, 0x9A, 0x03 },
                new List<int>() { 2, 3 },
                12)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Matasaburo,
                new List<byte>() { 0x90, 0x02, 0x91, 0x02, 0x92, 0x02 },
                new List<int>() { 0, 1, 2 },
                32)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Pipi_Activator,
                new List<byte>() { 0x9C, 0x01 },
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(EEnemyID.LightningGoro,
                new List<byte>() { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01 },
                new List<int>() { 0, 1, 2 },
                20)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Mole_Activator,
                new List<byte>() { 0x90, 0x03 },
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(EEnemyID.Shotman_Left,
                new List<byte>() { 0x98, 0x03, 0x99, 0x03 },
                new List<int>() { 0, 1 },
                20)); // Guess
            Enemies.Add(new Enemy(EEnemyID.SniperArmor,
                new List<byte>() { 0x91, 0x03, 0x92, 0x03, 0x93, 0x03, 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 0, 1, 2, 3, 4 },
                64)); // Guess
            Enemies.Add(new Enemy(EEnemyID.SniperJoe,
                new List<byte>() { 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 3, 4 },
                24)); // Guess
            Enemies.Add(new Enemy(EEnemyID.Scworm,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 },
                8)); // Guess

            // Copy enemy list to dictionary
            foreach (Enemy e in Enemies)
            {
                EnemiesByType.Add(e.ID, e);
            }
        }

        private void InitializeRooms()
        {
            // Heatman & Wily 1 stage enemies
            // Restriction: Yoku blocks, Dragon
            Rooms.Add(new EnemyRoom(EStageID.HeatW1, 0x003470, // w1 first screen = 7
                new int[] { 0, 12 },
                0x003910, 0x003911, 0x003912, 0x003913, 0x003914, 0x003915, 0x003916, 0x003917, 0x003918, 0x003919, 0x00391A, 0x00391B, 0x00391C, 0x00391D, 0x00391E,
                0x00395c, 0x00395d));
            Rooms.Add(new EnemyRoom(EStageID.HeatW1, 0x003494,
                new int[] { 1, 2 },
                new int[] { 3 }, new int[] { 0x97, 0x03 },
                0x00391F, 0x003924, 0x003925, 0x003927, 0x00392A, 0x00392B, 0x00392C, 0x00392E, 0x003931, 0x003933));
            Rooms.Add(new EnemyRoom(EStageID.HeatW1, 0x003482,
                new int[] { 3, 8, 9, 10 },
                0x00394D,
                0x003959,
                0x00395a,
                0x00395b));
            Rooms.Add(new EnemyRoom(EStageID.HeatW1, 0x0034ca,
                new int[] { 7 },
                0x00394e, 0x00394f, 0x003950, 0x003951, 0x003952, 0x003953, 0x003954, 0x003955, 0x003956, 0x003957, 0x003958));

            // Airman & Wily 2 stage enemies
            // Restriction: Goblins, Lightning Goros
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x007470, 
                new int[] { 0 },
                new int[] { 0, 1, 2, 5 }, new int[] { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01, 0x96, 0x03 },
                0x007919, 0x00791B));
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x007494,
                new int[] { 1 },
                0x00791C, 0x00791D));
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x007482,
                new int[] { 2 },
                new int[] { 3, 5 }, new int[] { 0x9A, 0x03, 0x96, 0x03 },
                0x007920, 0x007921, 0x007922, 0x007923, 0x007924, 0x007925, 0x007926));
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x0074b8,
                new int[] { 5 },
                0x007927, 0x007928, 0x007929));
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x0074ca,
                new int[] { 7 },
                0x00792a, 0x00792b));
            Rooms.Add(new EnemyRoom(EStageID.AirW2, 0x0074dc,
                new int[] { 9 },
                0x00792c, 0x00792d, 0x00792e, 0x00792f, 0x007930, 0x007931, 0x007932, 0x007933, 0x007934));

            // Woodman & Wily 3 stage enemies
            // Restriction: Wolves (but only used in wolf rooms)
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B4A6,
                new int[] { 0, 8, 9, 10 },
                0x00B910, 0x00B911, 0x00B912, 0x00B913, 0x00B914, 0x00B915, 0x00B916, 0x00B917, 0x00B918, 0x00B919, 0x00B910, 0x00B91A, 0x00B91B, 0x00B91C,
                0x00B92D,
                0x00B92E,
                0x00B92F));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B482,
                new int[] { 1, 6 },
                0x00B91D, 0x00B91E, 0x00B91F,
                0x00B923, 0x00B924));
            // Wolf rooms. When replaced with other enemies, cannot proceed due to solid tiles
            //Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B4CA,
            //    new int[] { 2, 3, 4 },
            //    0x00B920, 
            //    0x00B921, 
            //    0x00B922));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B494,
                new int[] { 7 },
                0x00B925, 0x00B926, 0x00B927, 0x00B928, 0x00B929, 0x00B92A, 0x00B92B, 0x00B92C));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00B4B8,
                new int[] { 11 },
                0x00B930, 0x00B931, 0x00B932, 0x00B933));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00b500,
                new int[] { 16 },
                0x00b934));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00b512,
                new int[] { 17 },
                0x00b935, 0x00b936));
            Rooms.Add(new EnemyRoom(EStageID.WoodW3, 0x00b470,
                new int[] { 22 },
                0x00b937, 0x00b938, 0x00b939));

            // Bubbleman & Wily 4 stage enemies
            // Restrictions: Dropping platform, Track platforms
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00F470,
                new int[] { 0, 5 },
                0x00F910, 0x00F911, 0x00F912,
                0x00F928, 0x00F929, 0x00F92A, 0x00F92B, 0x00F92C, 0x00F92D));
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00F482,
                new int[] { 1, 2, 3 },
                0x00F918, 0x00F919, 0x00F91A, 0x00F91B, 0x00F91C, 0x00F91D));
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00F494,
                new int[] { 4 },
                0x00F91E, 0x00F91F, 0x00F921, 0x00F922, 0x00F923, 0x00F924, 0x00F925, 0x00F926, 0x00F927));
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00f4b8,
                new int[] { 9, 10, 13 },
                0x00f92e,
                0x00f92f,
                0x00f930, 0x00f931));
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00f4ca, 
                new int[] { 15, 17 },
                new int[] { 3 }, new int[] { 0x95, 0x03 },
                0x00f932, 0x00f933, 0x00f935,
                0x00f937, 0x00f938, 0x00f93a));
            Rooms.Add(new EnemyRoom(EStageID.BubbleW4, 0x00f4dc,
                new int[] { 19 },
                0x00f93c, 0x00f93d, 0x00f93e, 0x00f93f));
            
            // Quick
            // Empty room, unused address table
            //Rooms.Add(new EnemyRoom("Quick_01", 0x013470,//    ));
            Rooms.Add(new EnemyRoom(EStageID.QuickW5, 0x0134CA,
                new int[] { 1, 2 },
                0x013910, 0x013911));
            Rooms.Add(new EnemyRoom(EStageID.QuickW5, 0x0134A6,
                new int[] { 3, 4, 5, 7, 8, 9, 10, 11, 12, 13 },
                0x013912, 0x013916));
            Rooms.Add(new EnemyRoom(EStageID.QuickW5, 0x013482,
                new int[] { 6 },
                0x013917, 0x013918, 0x013919, 0x01391A, 0x01391B, 0x01391C));
            Rooms.Add(new EnemyRoom(EStageID.QuickW5, 0x013494,
                new int[] { 14 },
                0x013924, 0x013925));

            // Flash
            Rooms.Add(new EnemyRoom(EStageID.FlashW6, 0x017470,
                new int[] { 0, 2, 3, 5 },
                0x017910, 0x017911, 0x017912, 0x017913, 0x017914, 0x017915,
                0x017917, // Room 4 only has a clash barrier
                0x01791A));
            Rooms.Add(new EnemyRoom(EStageID.FlashW6, 0x017482,
                new int[] { 1, 6, 7 },
                0x017916,
                0x01791B,
                0x01791C, 0x01791D, 0x01791E));
            Rooms.Add(new EnemyRoom(EStageID.FlashW6, 0x017494,
                new int[] { 4 },
                0x017918, 0x017919));

            // Metal
            Rooms.Add(new EnemyRoom(EStageID.Metal, 0x01B470,
                new int[] { 0, 1 },
                0x01b910, 0x01b911, 0x01b912, 0x01b913, 0x01b914, 0x01b915, 0x01b916, 0x01b917, 0x01b918));
            Rooms.Add(new EnemyRoom(EStageID.Metal, 0x01B482,
                new int[] { 2 },
                0x01b919, 0x01b91a, 0x01b91b, 0x01b91c, 0x01b91d, 0x01b91e, 0x01b91f, 0x01b920, 0x01b921, 0x01b922, 0x01b923, 0x01b924, 0x01b925));

            // Clash
            Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f494, 
                new int[] { 0, 1, 3, 4, 5 },
                new int[] { 3 }, new int[] { 0x95, 0x03 },
                0x01f910, 0x01f911, 0x01f912,
                0x01f913, 0x01f914, 0x01f915,
                0x01f919, 0x01f91a,
                0x01f91c, 0x01f91d,
                0x01f91f, 0x01f920, 0x01f921, 0x01f923, 0x01f924 // ??? on the last 3
                ));
            Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f482, 
                new int[] { 2, 8, 9 },
                0x01f916, 0x01f917, 0x01f918,
                0x01f927,
                0x01f929));
            Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f4a6,
                new int[] { 6, 7 },
                0x01f925));
            Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f470,
                new int[] { 10, 11, 12 },
                0x01f92b,
                0x01f92c,
                0x01f92d, 0x01f92e));
            // Empty room
            //Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f4b8,
            //    new int[] { 13 },
            //    ));
            Rooms.Add(new EnemyRoom(EStageID.Clash, 0x01f4ca,
                new int[] { 14 },
                0x01f92f, 0x01f930, 0x01f931));
        }

        private List<Enemy> GenerateEnemyCombinations(EnemyRoom room)
        {
            // Create a random enemy set
            List<Enemy> NewEnemies = new List<Enemy>();
            List<Enemy> PotentialEnemies = new List<Enemy>();
            bool done = false;
            while (!done)
            {
                foreach (Enemy en in Enemies)
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
                            break;
                        case EStageID.AirW2:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            break;
                        case EStageID.BubbleW4:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
                            break;
                        case EStageID.Clash:
                            if (en.ID == EEnemyID.Mole_Activator) continue;
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

