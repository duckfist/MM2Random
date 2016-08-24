using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers
{
    public class RWeaknesses
    {
        public RWeaknesses()
        {
            if (RandomMM2.Settings.IsJapanese)
            {
                RandomizeJ();
            }
            else
            {
                RandomizeU();
            }
            RandomizeWilyUJ();
        }

        /// <summary>
        /// Modify the damage values of each weapon against each Robot Master for Rockman 2 (J).
        /// </summary>
        private static void RandomizeJ()
        {
            List<WeaponTable> Weapons = new List<WeaponTable>();

            Weapons.Add(new WeaponTable()
            {
                Name = "Buster",
                ID = 0,
                Address = ERMWeaponAddress.Buster,
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
                Address = ERMWeaponAddress.AtomicFire,
                // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Air Shooter",
                ID = 2,
                Address = ERMWeaponAddress.AirShooter,
                RobotMasters = new int[8] { 2, 0, 4, 0, 2, 0, 0, 0x0A }
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
                weapon.RobotMasters.Shuffle(RandomMM2.Random);
            }

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
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
        private static void RandomizeU()
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
                // Dragon = 1
                // Byte Unused = 0
                // Gutsdozer = 1
                // Unused = 0
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Atomic Fire",
                ID = 1,
                Address = ERMWeaponAddress.Eng_AtomicFire,
                // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
                // Dragon = 8
                // Gutsdozer = 8
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Air Shooter",
                ID = 2,
                Address = ERMWeaponAddress.Eng_AirShooter,
                RobotMasters = new int[8] { 2, 0, 4, 0, 2, 0, 0, 0x0A }
                // Dragon = 0
                // Gutsdozer = 0
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Leaf Shield",
                ID = 3,
                Address = ERMWeaponAddress.Eng_LeafShield,
                RobotMasters = new int[8] { 0, 8, 0xFF, 0, 0, 0, 0, 0 }
                // Dragon = 0
                // Unused = 0
                // Gutsdozer = 0
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Bubble Lead",
                ID = 4,
                Address = ERMWeaponAddress.Eng_BubbleLead,
                RobotMasters = new int[8] { 6, 0, 0, 0xFF, 0, 2, 0, 1 }
                // Dragon = 0
                // Unused = 0
                // Gutsdozer = 1
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Quick Boomerang",
                ID = 5,
                Address = ERMWeaponAddress.Eng_QuickBoomerang,
                RobotMasters = new int[8] { 2, 2, 0, 2, 0, 0, 4, 1 }
                // Dragon = 1
                // Unused = 0
                // Gutsdozer = 2
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Time Stopper",
                ID = 6,
                Address = ERMWeaponAddress.Eng_TimeStopper,
                // NOTE: These values affect damage per tick
                // NOTE: This table only has robot masters, no wily bosses
                RobotMasters = new int[8] { 0, 0, 0, 0, 1, 0, 0, 0 }

            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Metal Blade",
                ID = 7,
                Address = ERMWeaponAddress.Eng_MetalBlade,
                RobotMasters = new int[8] { 1, 0, 2, 4, 0, 4, 0x0E, 0 }
                // Dragon = 0
                // Unused = 0
                // Gutsdozer = 0
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Clash Bomber",
                ID = 8,
                Address = ERMWeaponAddress.Eng_ClashBomber,
                RobotMasters = new int[8] { 0xFF, 0, 2, 2, 4, 3, 0, 0 }
                // Dragon = 1
                // Unused = 0
                // Gutsdozer = 1
            });

            foreach (WeaponTable weapon in Weapons)
            {
                weapon.RobotMasters.Shuffle(RandomMM2.Random);
            }

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
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
        /// TODO
        /// </summary>
        private static void RandomizeWilyUJ()
        {
            // First address for damage (buster v heatman)
            int address = (RandomMM2.Settings.IsJapanese) ? (int)ERMWeaponAddress.Buster : (int)ERMWeaponAddress.Eng_Buster;

            // Skip Atomic Fire and Time Stopper
            byte[] dragon    = new byte[] { 1, 0, 0, 0, 1, 0, 1 };
            byte[] guts      = new byte[] { 1, 0, 0, 1, 2, 0, 1 };
            byte[] machine   = new byte[] { 1, 1, 0, 0, 1, 1, 4 };
            byte[] alien     = new byte[] { 0xff, 0xff, 0xff, 1, 0xff, 0xff, 0xff };

            // TODO: Scale damage based on ammo count w/ weapon class instead of this hard-coded table
            double[] ammoUsed = new double[] { 0, 2, 3, 0.5, 0.25, 0.25, 4 };

            dragon.Shuffle(RandomMM2.Random);
            guts.Shuffle(RandomMM2.Random);
            machine.Shuffle(RandomMM2.Random);
            alien.Shuffle(RandomMM2.Random);

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                int j = 0;
                for (int i = 0; i < 9; i++)
                {
                    // Skip Atomic Fire and Time Stopper
                    if (i == 1 || i == 6) continue;

                    stream.Position = address + 14 * i + 8;
                    stream.WriteByte(dragon[j]);
                    stream.Position++; // Skip Picopico-kun byte, which does nothing
                    stream.WriteByte(guts[j]);
                    stream.Position++; // Skip Buebeam byte, which does nothing
                    stream.WriteByte(machine[j]);

                    // Scale damage against alien if using a high ammo usage weapon
                    if (alien[j] == 1)
                    {
                        if (ammoUsed[j] >= 1)
                        {
                            alien[j] = (byte)((double)ammoUsed[j] * 1.5);
                        }
                    }
                    stream.WriteByte(alien[j]);
                    j++;
                }
            }
        }
    }
}
