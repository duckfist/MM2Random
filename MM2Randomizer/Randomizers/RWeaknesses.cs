using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;
using System;

namespace MM2Randomizer.Randomizers
{
    public class RWeaknesses
    {
        public static bool IsChaos = true;
        public static int[,] BotWeaknesses = new int[8, 9];
        public static int[,] WilyWeaknesses = new int[4, 8];

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

            if (IsChaos)
            {
                using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    List<ERMWeaponAddress> primaryWeaknesses = new List<ERMWeaponAddress>(new ERMWeaponAddress[] {
                        ERMWeaponAddress.Eng_AtomicFire,
                        ERMWeaponAddress.Eng_AirShooter,
                        ERMWeaponAddress.Eng_LeafShield,
                        ERMWeaponAddress.Eng_BubbleLead,
                        ERMWeaponAddress.Eng_QuickBoomerang,
                        ERMWeaponAddress.Eng_TimeStopper,
                        ERMWeaponAddress.Eng_MetalBlade,
                        ERMWeaponAddress.Eng_ClashBomber
                    });
                    List<ERMWeaponAddress> primaryWeaknessesShuffled = new List<ERMWeaponAddress>(primaryWeaknesses);
                    primaryWeaknessesShuffled.Shuffle(RandomMM2.Random);

                    // Select 2 robots to be weak against Buster
                    int busterI1 = RandomMM2.Random.Next(8);
                    int busterI2 = busterI1;
                    while (busterI2 == busterI1)
                        busterI2 = RandomMM2.Random.Next(8);

                    // Foreach boss
                    for (int i = 0; i < 8; i++)
                    {
                        // First, fill in special weapon tables with a 50% chance to block or do 1 damage
                        for (int j = 0; j < primaryWeaknesses.Count; j++)
                        {
                            double rTestImmune = RandomMM2.Random.NextDouble();
                            byte damage = 0;
                            if (rTestImmune > 0.5)
                            {
                                if (primaryWeaknesses[j] == ERMWeaponAddress.AtomicFire)
                                {
                                    // ...except for Atomic Fire, which will do some more damage
                                    damage = (byte)(RWeaponBehavior.AmmoUsage[1] / 2);
                                }   
                                else
                                {
                                    damage = 0x01;
                                }
                            }
                            stream.Position = (int)primaryWeaknesses[j] + i;
                            stream.WriteByte(damage);

                            BotWeaknesses[i, j + 1] = damage;
                        }

                        // Write the primary weakness for this boss
                        stream.Position = (int)primaryWeaknessesShuffled[i] + i;
                        byte dmgPrimary = GetRoboDamagePrimary(primaryWeaknessesShuffled[i]);
                        stream.WriteByte(dmgPrimary);

                        // Write the secondary weakness for this boss (next element in list, will always do 2 damage, or 4 if atomic fire)
                        int i2 = (i + 1 >= 8) ? 0 : i + 1;
                        ERMWeaponAddress weakWeap2 = primaryWeaknessesShuffled[i2];
                        stream.Position = (int)weakWeap2 + i;
                        byte dmgSecondary = 0x02;
                        if (weakWeap2 == ERMWeaponAddress.AtomicFire)
                        {
                            dmgSecondary = 0x04;
                        }
                        stream.WriteByte(dmgSecondary);
                        
                        // Add buster damage
                        stream.Position = (int)ERMWeaponAddress.Eng_Buster + i;
                        if (i == busterI1 || i == busterI2)
                        {
                            stream.WriteByte(0x02);
                            BotWeaknesses[i, 0] = 0x02;
                        }
                        else
                        {
                            stream.WriteByte(0x01);
                            BotWeaknesses[i, 0] = 0x01;
                        }

                        // Save info
                        int weapIndexPrimary = GetWeaponIndexFromAddress(primaryWeaknessesShuffled[i]);
                        BotWeaknesses[i, weapIndexPrimary] = dmgPrimary;
                        int weapIndexSecondary = GetWeaponIndexFromAddress(weakWeap2);
                        BotWeaknesses[i, weapIndexSecondary] = dmgSecondary;
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            Console.Write("{0} ", BotWeaknesses[i, j]);
                        }
                        Console.WriteLine();
                    }
                }
            }
            else
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
        }

        /// <summary>
        /// Do 3 damage for high-ammo weapons, and ammo-damage + 1 for the others
        /// </summary>
        /// <param name="eRMWeaponAddress"></param>
        /// <returns></returns>
        private static byte GetRoboDamagePrimary(ERMWeaponAddress eRMWeaponAddress)
        {
            // Flat 25% chance to do 2 extra damage
            byte damage = 0;
            double rExtraDmg = RandomMM2.Random.NextDouble();
            if (rExtraDmg > 0.75)
            {
                damage = 2;
            }

            switch (eRMWeaponAddress)
            {
                case ERMWeaponAddress.Eng_AtomicFire:
                    damage += (byte)(RWeaponBehavior.AmmoUsage[1] + 1);
                    break;
                case ERMWeaponAddress.Eng_AirShooter:
                    damage += (byte)(RWeaponBehavior.AmmoUsage[2] + 1);
                    break;
                case ERMWeaponAddress.Eng_LeafShield:
                    damage += (byte)(RWeaponBehavior.AmmoUsage[3] + 1);
                    break;
                case ERMWeaponAddress.Eng_BubbleLead:
                    break;
                case ERMWeaponAddress.Eng_QuickBoomerang:
                    break;
                //case ERMWeaponAddress.Eng_TimeStopper:
                //    break;
                case ERMWeaponAddress.Eng_MetalBlade:
                    break;
                case ERMWeaponAddress.Eng_ClashBomber:
                    damage += (byte)(RWeaponBehavior.AmmoUsage[7] + 1);
                    break;
            }
            if (damage < 3) damage = 3;
            return damage;
        }

        private static int GetWeaponIndexFromAddress(ERMWeaponAddress weaponAddress)
        {
            switch (weaponAddress)
            {
                case ERMWeaponAddress.Eng_Buster:
                    return 0;
                case ERMWeaponAddress.Eng_AtomicFire:
                    return 1;
                case ERMWeaponAddress.Eng_AirShooter:
                    return 2;
                case ERMWeaponAddress.Eng_LeafShield:
                    return 3;
                case ERMWeaponAddress.Eng_BubbleLead:
                    return 4;
                case ERMWeaponAddress.Eng_QuickBoomerang:
                    return 5;
                case ERMWeaponAddress.Eng_TimeStopper:
                    return 6;
                case ERMWeaponAddress.Eng_MetalBlade:
                    return 7;
                case ERMWeaponAddress.Eng_ClashBomber:
                    return 8;
                default: return -1;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomizeWilyUJ()
        {
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                if (IsChaos)
                {
                    // List of special weapons
                    List<ERMWeaponAddress> Weapons = new List<ERMWeaponAddress>(new ERMWeaponAddress[] {
                        ERMWeaponAddress.Eng_AtomicFire,
                        ERMWeaponAddress.Eng_AirShooter,
                        ERMWeaponAddress.Eng_LeafShield,
                        ERMWeaponAddress.Eng_BubbleLead,
                        ERMWeaponAddress.Eng_QuickBoomerang,
                        ERMWeaponAddress.Eng_MetalBlade,
                        ERMWeaponAddress.Eng_ClashBomber
                    });
                    
                    // Dragon
                    // 25% chance to have a buster vulnerability
                    double rBuster = RandomMM2.Random.NextDouble();
                    byte busterDmg = 0x00;
                    if (rBuster > 0.75)
                        busterDmg = 0x01;
                    stream.Position = (int)ERMWeaponAddress.Eng_Buster + (int)ERMWeaponAddress.Offset_Dragon;
                    stream.WriteByte(busterDmg);
                    Console.Write(busterDmg);

                    // Choose 2 special weapon weaknesses
                    List<ERMWeaponAddress> dragon = new List<ERMWeaponAddress>(Weapons);
                    int rInt = RandomMM2.Random.Next(dragon.Count);
                    ERMWeaponAddress weakness1 = dragon[rInt];
                    dragon.RemoveAt(rInt);
                    rInt = RandomMM2.Random.Next(dragon.Count);
                    ERMWeaponAddress weakness2 = dragon[rInt];

                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        ERMWeaponAddress weapon = Weapons[i];
                        stream.Position = (int)weapon + (int)ERMWeaponAddress.Offset_Dragon;

                        // Dragon weak
                        if (weapon == weakness1 || weapon == weakness2)
                        {
                            // Deal 1 damage with weapons that cost 1 or less ammo
                            byte damage = 0x01;
                            
                            // Deal damage = ammoUsage - 1, minimum 2 damage
                            if (RWeaponBehavior.AmmoUsage[i+1] > 1)
                            {
                                int tryDamage = (int)RWeaponBehavior.AmmoUsage[i+1] - 0x01;
                                damage = (tryDamage < 2) ? (byte)0x02 : (byte)tryDamage;
                            }
                            stream.WriteByte(damage);
                            Console.Write(damage);
                        }
                        // Dragon immune
                        else
                        {
                            stream.WriteByte(0x00);
                            Console.Write(0x00);
                        }
                    }

                    Console.WriteLine(" < dragon");

                    // Guts
                    // 25% chance to have a buster vulnerability
                    rBuster = RandomMM2.Random.NextDouble();
                    busterDmg = 0x00;
                    if (rBuster > 0.75)
                        busterDmg = 0x01;
                    stream.Position = (int)ERMWeaponAddress.Eng_Buster + (int)ERMWeaponAddress.Offset_Guts;
                    stream.WriteByte(busterDmg);
                    Console.Write(busterDmg);

                    // Choose 2 special weapon weaknesses
                    List<ERMWeaponAddress> guts = new List<ERMWeaponAddress>(Weapons);
                    rInt = RandomMM2.Random.Next(guts.Count);
                    weakness1 = guts[rInt];
                    guts.RemoveAt(rInt);
                    rInt = RandomMM2.Random.Next(guts.Count);
                    weakness2 = guts[rInt];

                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        ERMWeaponAddress weapon = Weapons[i];
                        stream.Position = (int)weapon + (int)ERMWeaponAddress.Offset_Guts;

                        // Guts weak
                        if (weapon == weakness1 || weapon == weakness2)
                        {
                            // Deal 1 damage with weapons that cost 1 or less ammo
                            byte damage = 0x01;

                            // Deal damage = ammoUsage - 1, minimum 2 damage
                            if (RWeaponBehavior.AmmoUsage[i+1] > 1)
                            {
                                int tryDamage = (int)RWeaponBehavior.AmmoUsage[i+1] - 0x01;
                                damage = (tryDamage < 2) ? (byte)0x02 : (byte)tryDamage;
                            }
                            stream.WriteByte(damage);
                            Console.Write(damage);
                        }
                        // Guts immune
                        else
                        {
                            stream.WriteByte(0x00);
                            Console.Write(0x00);
                        }
                    }

                    Console.WriteLine(" < guts");

                    // Machine
                    // 75% chance to have a buster vulnerability
                    rBuster = RandomMM2.Random.NextDouble();
                    busterDmg = 0x00;
                    if (rBuster > 0.25)
                        busterDmg = 0x01;
                    stream.Position = (int)ERMWeaponAddress.Eng_Buster + (int)ERMWeaponAddress.Offset_Machine;
                    stream.WriteByte(busterDmg);
                    Console.Write(busterDmg);

                    // Choose 2 special weapon weaknesses
                    List<ERMWeaponAddress> machine = new List<ERMWeaponAddress>(Weapons);
                    rInt = RandomMM2.Random.Next(machine.Count);
                    weakness1 = machine[rInt];
                    machine.RemoveAt(rInt);
                    rInt = RandomMM2.Random.Next(machine.Count);
                    weakness2 = machine[rInt];

                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        ERMWeaponAddress weapon = Weapons[i];
                        stream.Position = (int)weapon + (int)ERMWeaponAddress.Offset_Machine;

                        // Machine weak
                        if (weapon == weakness1 || weapon == weakness2)
                        {
                            // Deal 1 damage with weapons that cost 1 or less ammo
                            byte damage = 0x01;

                            // Deal damage = ammoUsage
                            if (RWeaponBehavior.AmmoUsage[i+1] > 1)
                            {
                                damage = (byte)RWeaponBehavior.AmmoUsage[i+1];
                            }
                            stream.WriteByte(damage);
                            Console.Write(damage);
                        }
                        // Machine immune
                        else
                        {
                            stream.WriteByte(0x00);
                            Console.Write(0x00);
                        }
                    }

                    Console.WriteLine(" < machine");

                    // Alien
                    // Buster Heat Air Wood Bubble Quick Clash Metal
                    byte alienDamage = 1;
                    List<ERMWeaponAddress> alienWeapons = new List<ERMWeaponAddress>(new ERMWeaponAddress[] {
                        ERMWeaponAddress.Eng_Buster,
                        ERMWeaponAddress.Eng_AtomicFire,
                        ERMWeaponAddress.Eng_AirShooter,
                        ERMWeaponAddress.Eng_LeafShield,
                        ERMWeaponAddress.Eng_BubbleLead,
                        ERMWeaponAddress.Eng_QuickBoomerang,
                        ERMWeaponAddress.Eng_MetalBlade,
                        ERMWeaponAddress.Eng_ClashBomber
                    });
                    int rWeaponIndex = RandomMM2.Random.Next(alienWeapons.Count);

                    // Deal two damage for 1-ammo weapons (or buster)
                    if (RWeaponBehavior.AmmoUsage[rWeaponIndex] == 1)
                    {
                        alienDamage = 2;
                    }
                    // For 2+ ammo use weapons, deal 20% more than that in damage, rounded up
                    else if (RWeaponBehavior.AmmoUsage[rWeaponIndex] > 1)
                    {
                        alienDamage = (byte)Math.Ceiling(RWeaponBehavior.AmmoUsage[rWeaponIndex] * 1.2);
                    }
                    
                    // Apply weakness and erase others
                    for (int i = 0; i < alienWeapons.Count; i++)
                    {
                        ERMWeaponAddress weapon = alienWeapons[i];

                        stream.Position = (int)weapon + (int)ERMWeaponAddress.Offset_Alien;
                        if (i == rWeaponIndex)
                        {
                            stream.WriteByte(alienDamage);
                            Console.Write(alienDamage);
                        }
                        else
                        {
                            stream.WriteByte(0xFF);
                            Console.Write(0xFF);
                        }
                    }

                    Console.WriteLine(" < alien");

                }
                else
                {
                    // First address for damage (buster v heatman)
                    int address = (RandomMM2.Settings.IsJapanese) ? (int)ERMWeaponAddress.Buster : (int)ERMWeaponAddress.Eng_Buster;

                    // Skip Time Stopper
                    // Buster Air Wood Bubble Quick Clash Metal
                    byte[] dragon = new byte[] { 1, 0, 0, 0, 1, 0, 1 };
                    byte[] guts = new byte[] { 1, 0, 0, 1, 2, 0, 1 };
                    byte[] machine = new byte[] { 1, 1, 0, 0, 1, 1, 4 };
                    byte[] alien = new byte[] { 0xff, 0xff, 0xff, 1, 0xff, 0xff, 0xff };

                    // TODO: Scale damage based on ammo count w/ weapon class instead of this hard-coded table
                    // Buster Air Wood Bubble Quick Clash Metal
                    //double[] ammoUsed = new double[] { 0, 2, 3, 0.5, 0.25, 4, 0.25 };

                    dragon.Shuffle(RandomMM2.Random);
                    guts.Shuffle(RandomMM2.Random);
                    machine.Shuffle(RandomMM2.Random);
                    alien.Shuffle(RandomMM2.Random);
                    
                    int j = 0;
                    for (int i = 0; i < 8; i++) // i = Buster plus 7 weapons, Time Stopper damage is located in another table (going to ignore it anyways)
                    {
                        //// Skip Atomic Fire
                        //if (i == 1) continue;

                        stream.Position = address + 14 * i + 8;
                        stream.WriteByte(dragon[j]);
                        stream.Position++; // Skip Picopico-kun byte, which does nothing
                        stream.WriteByte(guts[j]);
                        stream.Position++; // Skip Buebeam byte, which does nothing
                        stream.WriteByte(machine[j]);

                        // Scale damage against alien if using a high ammo usage weapon
                        if (alien[j] == 1)
                        {
                            if (RWeaponBehavior.AmmoUsage[j] >= 1)
                            {
                                alien[j] = (byte)((double)RWeaponBehavior.AmmoUsage[j] * 1.3);
                            }
                        }
                        stream.WriteByte(alien[j]);
                        j++;
                    }
                }
            }
        }
    }
}
