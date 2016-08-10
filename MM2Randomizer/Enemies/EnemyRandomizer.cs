using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enemies
{
    public class EnemyRandomizer
    {
        public List<Enemy> Enemies { get; set; }
        public Dictionary<byte, Enemy> EnemiesByType { get; set; }
        public List<EnemyRoom> Rooms { get; set; }

        public EnemyRandomizer()
        {
            Enemies = new List<Enemy>();
            EnemiesByType = new Dictionary<byte, Enemy>();
            Rooms = new List<EnemyRoom>();

            InitializeEnemies();
            CreateRooms();
        }


        private void CreateRooms()
        {
            foreach (EnemyRoom room in Rooms)
            {
                List<Enemy> newEnemies = GenerateEnemyCombinations(room);

                // Change enemy IDs for the room
                for (int i = 0; i < room.EnemyIDAddresses.Length; i++)
                {
                    int randomIndex = RandomMM2.Random.Next(newEnemies.Count);
                    room.NewEnemyInstances.Add(newEnemies[randomIndex]);

                    using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = room.EnemyIDAddresses[i];
                        stream.WriteByte(newEnemies[randomIndex].ID);
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
                // Return false if enemy is already in the list?
                if (spriteToAdd.Name == e.Name)
                {
                    return false;
                }

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
            Enemies.Add(new Enemy(
                "Claw", // Activator
                0x07,
                new List<byte>() { 0x9D, 0x03 },
                new List<int>() { 3 }));
            Enemies.Add(new Enemy(
                "Tanishi",
                0x0A,
                new List<byte>() { 0x9B, 0x03, 0x9C, 0x03 },
                new List<int>() { 0, 1 }));
            Enemies.Add(new Enemy(
                "Kerog",
                0x0C,
                new List<byte>() { 0x9B, 0x02, 0x9C, 0x02, 0x9D, 0x02 },
                new List<int>() { 0, 1, 2 }));
            Enemies.Add(new Enemy(
                "Batton",
                0x16,
                new List<byte>() { 0x93, 0x02, 0x94, 0x02 },
                new List<int>() { 3, 4 }));
            Enemies.Add(new Enemy(
                "Robbit",
                0x17,
                new List<byte>() { 0x98, 0x02, 0x99, 0x02, 0x9A, 0x02 },
                new List<int>() { 0, 1, 2 }));
            Enemies.Add(new Enemy(
                "Monking",
                0x1D,
                new List<byte>() { 0x98, 0x01, 0x99, 0x01, 0x9A, 0x01, 0x9B, 0x01 },
                new List<int>() { 0, 1, 2, 3 }));
            Enemies.Add(new Enemy(
                "Kukku", // Activator
                0x1E,
                new List<byte>() { 0x90, 0x01, 0x91, 0x01, 0x92, 0x01, 0x93, 0x01 },
                new List<int>() { 0, 1, 2, 3 }));
            Enemies.Add(new Enemy(
                "Telly", // Spawn location
                0x21,
                new List<byte>() { 0x93, 0x01 },
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(
                "Pierrobot",
                0x29,
                new List<byte>() { 0x96, 0x01, 0x97, 0x01 },
                new List<int>() { 0, 1 }));
            Enemies.Add(new Enemy(
                "FlyBoy",
                0x2B,
                new List<byte>() { 0x94, 0x01, 0x95, 0x01 },
                new List<int>() { 0, 1 }));
            Enemies.Add(new Enemy(
                "Press",
                0x30,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 }));
            Enemies.Add(new Enemy(
                "Blocky",
                0x31,
                new List<byte>() { 0x9E, 0x03 },
                new List<int>() { 3 }));
            Enemies.Add(new Enemy(
                "NeoMetall",
                0x34,
                new List<byte>() { 0x92, 0x02, 0x9A, 0x03 },
                new List<int>() { 2, 3 }));
            Enemies.Add(new Enemy(
                "Matasaburo",
                0x36,
                new List<byte>() { 0x90, 0x02, 0x91, 0x02, 0x92, 0x02 },
                new List<int>() { 0, 1, 2 }));
            Enemies.Add(new Enemy(
                "Pipi",
                0x37,
                new List<byte>() { 0x9C, 0x01 }, // Activator
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(
                "LightningGoro",
                0x3E,
                new List<byte>() { 0x9D, 0x01, 0x9E, 0x01, 0x9F, 0x01 },
                new List<int>() { 0, 1, 2 }));
            Enemies.Add(new Enemy(
                "Mole", // Activator
                0x47,
                new List<byte>() { 0x90, 0x03 },
                new List<int>() { 4 }));
            Enemies.Add(new Enemy(
                "Shotman", // Facing Left
                0x4B,
                new List<byte>() { 0x98, 0x03, 0x99, 0x03 },
                new List<int>() { 0, 1 }));
            Enemies.Add(new Enemy(
                "SniperArmor",
                0x4E,
                new List<byte>() { 0x91, 0x03, 0x92, 0x03, 0x93, 0x03, 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 0, 1, 2, 3, 4 }));
            Enemies.Add(new Enemy(
                "SniperJoe",
                0x4F,
                new List<byte>() { 0x94, 0x03, 0x95, 0x03 },
                new List<int>() { 3, 4 }));
            Enemies.Add(new Enemy(
                "Scworm",
                0x50,
                new List<byte>() { 0x9E, 0x04 },
                new List<int>() { 3 }));



            // Copy enemy list to dictionary
            foreach (Enemy e in Enemies)
            {
                EnemiesByType.Add(e.ID, e);
            }

            // Heatman stage enemies
            // Restriction: Yoku blocks
            Rooms.Add(new EnemyRoom("Heat_01", 0x003470,
                0x003910, 0x003911, 0x003912, 0x003913, 0x003914, 0x003915, 0x003916, 0x003917, 0x003918, 0x003919, 0x00391A, 0x00391B, 0x00391C, 0x00391D, 0x00391E));
            Rooms.Add(new EnemyRoom("Heat_02-03", 0x003494, new int[] { 3 }, new int[] { 0x0397 }, 
                0x00391F, 0x003924, 0x003925, 0x003927, 0x00392A, 0x00392B, 0x00392C, 0x00392E, 0x003931, 0x003933));
            Rooms.Add(new EnemyRoom("Heat_04", 0x003482,
                0x00394D));

            // Airman stage enemies
            // Restriction: Goblins, Lightning Goros
            //Rooms.Add(new EnemyRoom("Air01", new int[] { 0, 1, 2, 3, 5 }, new int[] { 0x019D, 0x019E, 0x019F, 0x039A, 0x0396 },
            //    0x007919, 0x00791B));
            //Rooms.Add(new EnemyRoom("Air02",
            //    0x00791C, 0x00791D));
            //Rooms.Add(new EnemyRoom("Air03", new int[] { 3, 5 }, new int[] { 0x039A, 0x0396 },
            //    0x00791C, 0x00791D));
        }

        private List<Enemy> GenerateEnemyCombinations(EnemyRoom room)
        {
            // Create a random enemy set
            List<Enemy> NewEnemies = new List<Enemy>();
            List<Enemy> PotentialEnemies = new List<Enemy>(Enemies);
            bool done = false;
            while (!done)
            {
                NewEnemies.Add(PotentialEnemies[RandomMM2.Random.Next(PotentialEnemies.Count)]);
                PotentialEnemies.Clear();

                foreach (Enemy en in Enemies)
                {
                    if (CheckEnemySpriteFitInBank(NewEnemies, en))
                    {
                        PotentialEnemies.Add(en);
                    }
                }

                if (PotentialEnemies.Count == 0)
                {
                    done = true;
                }
            }

            return NewEnemies;
        }
    }
}

