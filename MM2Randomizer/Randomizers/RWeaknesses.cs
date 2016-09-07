using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;
using System;
using System.Text;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RWeaknesses : IRandomizer
    {
        public static bool IsChaos = true;
        public static int[,] BotWeaknesses = new int[8, 9];
        public static int[,] WilyWeaknesses = new int[5, 8];
        public static char[,] WilyWeaknessInfo = new char[5, 8];

        private StringBuilder debug;
        public override string ToString()
        {
            return debug.ToString();
        }

        public RWeaknesses(bool isChaos)
        {
            debug = new StringBuilder();
            IsChaos = isChaos;
        }

        public void Randomize(Patch p, Random r)
        {
            if (RandomMM2.Settings.IsJapanese)
            {
                RandomizeJ(r);
            }
            else
            {
                RandomizeU(p, r);
            }
            RandomizeWilyUJ(p, r);
        }

        /// <summary>
        /// Modify the damage values of each weapon against each Robot Master for Rockman 2 (J).
        /// </summary>
        private void RandomizeJ(Random r)
        {
            List<WeaponTable> Weapons = new List<WeaponTable>();

            Weapons.Add(new WeaponTable()
            {
                Name = "Buster",
                ID = 0,
                Address = EDmgVsBoss.Buster,
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
                Address = EDmgVsBoss.AtomicFire,
                // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Air Shooter",
                ID = 2,
                Address = EDmgVsBoss.AirShooter,
                RobotMasters = new int[8] { 2, 0, 4, 0, 2, 0, 0, 0x0A }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Leaf Shield",
                ID = 3,
                Address = EDmgVsBoss.LeafShield,
                RobotMasters = new int[8] { 0, 8, 0xFF, 0, 0, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Bubble Lead",
                ID = 4,
                Address = EDmgVsBoss.BubbleLead,
                RobotMasters = new int[8] { 6, 0, 0, 0xFF, 0, 2, 0, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Quick Boomerang",
                ID = 5,
                Address = EDmgVsBoss.QuickBoomerang,
                RobotMasters = new int[8] { 2, 2, 0, 2, 0, 0, 4, 1 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Time Stopper",
                ID = 6,
                Address = EDmgVsBoss.TimeStopper,
                // NOTE: These values affect damage per tick
                RobotMasters = new int[8] { 0, 0, 0, 0, 1, 0, 0, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Metal Blade",
                ID = 7,
                Address = EDmgVsBoss.MetalBlade,
                RobotMasters = new int[8] { 1, 0, 2, 4, 0, 4, 0x0E, 0 }
            });

            Weapons.Add(new WeaponTable()
            {
                Name = "Clash Bomber",
                ID = 8,
                Address = EDmgVsBoss.ClashBomber,
                RobotMasters = new int[8] { 0xFF, 0, 2, 2, 4, 3, 0, 0 }
            });

            foreach (WeaponTable weapon in Weapons)
            {
                weapon.RobotMasters.Shuffle(r);
            }

            using (var stream = new FileStream(RandomMM2.TempFileName, FileMode.Open, FileAccess.ReadWrite))
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
        private void RandomizeU(Patch Patch, Random r)
        {
            // Chaos Mode Weaknesses
            if (IsChaos)
            {
                List<EDmgVsBoss> bossPrimaryWeaknessAddresses = EDmgVsBoss.GetTables(false, true);
                List<EDmgVsBoss> bossWeaknessShuffled = new List<EDmgVsBoss>(bossPrimaryWeaknessAddresses);
                bossWeaknessShuffled.Shuffle(r);

                // Preparation: Disable redundant Atomic Fire healing code
                // (Note that 0xFF in any weakness table is sufficient to heal a boss)
                Patch.Add(0x02E66D, 0xFF, "Atomic Fire Boss To Heal" ); // Normally "00" to indicate Heatman.

                // Select 2 robots to be weak against Buster
                int busterI1 = r.Next(8);
                int busterI2 = busterI1;
                while (busterI2 == busterI1)
                    busterI2 = r.Next(8);

                // Foreach boss
                for (int i = 0; i < 8; i++)
                {
                    // First, fill in special weapon tables with a 50% chance to block or do 1 damage
                    for (int j = 0; j < bossPrimaryWeaknessAddresses.Count; j++)
                    {
                        double rTestImmune = r.NextDouble();
                        byte damage = 0;
                        if (rTestImmune > 0.5)
                        {
                            if (bossPrimaryWeaknessAddresses[j] == EDmgVsBoss.U_DamageH)
                            {
                                // ...except for Atomic Fire, which will do some more damage
                                damage = (byte)(RWeaponBehavior.AmmoUsage[1] / 2);
                            }
                            else if (bossPrimaryWeaknessAddresses[j] == EDmgVsBoss.U_DamageF)
                            {
                                damage = 0x00;
                            }
                            else
                            {
                                damage = 0x01;
                            }
                        }
                        Patch.Add(bossPrimaryWeaknessAddresses[j] + i, damage, String.Format("{0} Damage to {1}", bossPrimaryWeaknessAddresses[j].WeaponName, (EDmgVsBoss.Offset)i));
                        BotWeaknesses[i, j + 1] = damage;
                    }

                    // Write the primary weakness for this boss
                    byte dmgPrimary = GetRoboDamagePrimary(r, bossWeaknessShuffled[i]);
                    Patch.Add(bossWeaknessShuffled[i] + i, dmgPrimary, String.Format("{0} Damage to {1} (Primary)", bossWeaknessShuffled[i].WeaponName, (EDmgVsBoss.Offset)i));

                    // Write the secondary weakness for this boss (next element in list)
                    // Secondary weakness will either do 2 damage or 4 if it is Atomic Fire
                    // Time Stopper cannot be a secondary weakness. Instead it will heal that boss.
                    // As a result, one Robot Master will not have a secondary weakness
                    int i2 = (i + 1 >= 8) ? 0 : i + 1;
                    EDmgVsBoss weakWeap2 = bossWeaknessShuffled[i2];
                    //stream.Position = weakWeap2 + i;
                    byte dmgSecondary = 0x02;
                    if (weakWeap2 == EDmgVsBoss.U_DamageH)
                    {
                        dmgSecondary = 0x04;
                    }
                    else if (weakWeap2 == EDmgVsBoss.U_DamageF)
                    {
                        dmgSecondary = 0x00;
                        //long prevStreamPos = stream.Position;
                        //stream.Position = 0x02C08F; 
                        //stream.WriteByte((byte)i);
                        //stream.Position = prevStreamPos;
                        // Address in Time-Stopper code that normally heals Flashman, change to heal this boss instead
                        Patch.Add(0x02C08F, (byte)i, String.Format("Time-Stopper Heals {0} (Special Code)", (EDmgVsBoss.Offset)i));
                    }
                    Patch.Add(weakWeap2 + i, dmgSecondary, String.Format("{0} Damage to {1} (Secondary)", weakWeap2.WeaponName, (EDmgVsBoss.Offset)i));
                    //stream.WriteByte(dmgSecondary);
                        
                    // Add buster damage
                    //stream.Position = EDmgVsBoss.U_DamageP + i;
                    if (i == busterI1 || i == busterI2)
                    {
                        Patch.Add(EDmgVsBoss.U_DamageP + i, 0x02, String.Format("Buster Damage to {0}", (EDmgVsBoss.Offset)i));
                        BotWeaknesses[i, 0] = 0x02;
                    }
                    else
                    {
                        Patch.Add(EDmgVsBoss.U_DamageP + i, 0x01, String.Format("Buster Damage to {0}", (EDmgVsBoss.Offset)i));
                        BotWeaknesses[i, 0] = 0x01;
                    }

                    // Save info
                    int weapIndexPrimary = GetWeaponIndexFromAddress(bossWeaknessShuffled[i]);
                    BotWeaknesses[i, weapIndexPrimary] = dmgPrimary;
                    int weapIndexSecondary = GetWeaponIndexFromAddress(weakWeap2);
                    BotWeaknesses[i, weapIndexSecondary] = dmgSecondary;
                }

                debug.AppendLine("Robot Master Weaknesses:");
                debug.AppendLine("P\tH\tA\tW\tB\tQ\tF\tM\tC:");
                debug.AppendLine("--------------------------------------------");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        debug.Append(String.Format("{0}\t", BotWeaknesses[i, j]));
                    }
                    debug.AppendLine("< " + ((EDmgVsBoss.Offset)i).ToString());
                }
                debug.AppendLine();
            }

            // Easy Mode Weaknesses
            else
            {
                List<WeaponTable> Weapons = new List<WeaponTable>();

                Weapons.Add(new WeaponTable()
                {
                    Name = "Buster",
                    ID = 0,
                    Address = EDmgVsBoss.U_DamageP,
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
                    Address = EDmgVsBoss.U_DamageH,
                    // Note: These values only affect a fully charged shot.  Partially charged shots use the Buster table.
                    RobotMasters = new int[8] { 0xFF, 6, 0x0E, 0, 0x0A, 6, 4, 6 }
                    // Dragon = 8
                    // Gutsdozer = 8
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Air Shooter",
                    ID = 2,
                    Address = EDmgVsBoss.U_DamageA,
                    RobotMasters = new int[8] { 2, 0, 4, 0, 2, 0, 0, 0x0A }
                    // Dragon = 0
                    // Gutsdozer = 0
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Leaf Shield",
                    ID = 3,
                    Address = EDmgVsBoss.U_DamageW,
                    RobotMasters = new int[8] { 0, 8, 0xFF, 0, 0, 0, 0, 0 }
                    // Dragon = 0
                    // Unused = 0
                    // Gutsdozer = 0
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Bubble Lead",
                    ID = 4,
                    Address = EDmgVsBoss.U_DamageB,
                    RobotMasters = new int[8] { 6, 0, 0, 0xFF, 0, 2, 0, 1 }
                    // Dragon = 0
                    // Unused = 0
                    // Gutsdozer = 1
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Quick Boomerang",
                    ID = 5,
                    Address = EDmgVsBoss.U_DamageQ,
                    RobotMasters = new int[8] { 2, 2, 0, 2, 0, 0, 4, 1 }
                    // Dragon = 1
                    // Unused = 0
                    // Gutsdozer = 2
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Time Stopper",
                    ID = 6,
                    Address = EDmgVsBoss.U_DamageF,
                    // NOTE: These values affect damage per tick
                    // NOTE: This table only has robot masters, no wily bosses
                    RobotMasters = new int[8] { 0, 0, 0, 0, 1, 0, 0, 0 }

                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Metal Blade",
                    ID = 7,
                    Address = EDmgVsBoss.U_DamageM,
                    RobotMasters = new int[8] { 1, 0, 2, 4, 0, 4, 0x0E, 0 }
                    // Dragon = 0
                    // Unused = 0
                    // Gutsdozer = 0
                });

                Weapons.Add(new WeaponTable()
                {
                    Name = "Clash Bomber",
                    ID = 8,
                    Address = EDmgVsBoss.U_DamageC,
                    RobotMasters = new int[8] { 0xFF, 0, 2, 2, 4, 3, 0, 0 }
                    // Dragon = 1
                    // Unused = 0
                    // Gutsdozer = 1
                });

                foreach (WeaponTable weapon in Weapons)
                {
                    weapon.RobotMasters.Shuffle(r);
                }

                foreach (WeaponTable weapon in Weapons)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Patch.Add(weapon.Address + i, (byte)weapon.RobotMasters[i], String.Format("Easy Weakness: {0} against {1}", weapon.Name, ((EDmgVsBoss.Offset)i).ToString() ));
                    }
                }
            }
        }

        /// <summary>
        /// Do 3 damage for high-ammo weapons, and ammo-damage + 1 for the others
        /// Time Stopper will always do 1 damage.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        private static byte GetRoboDamagePrimary(Random r, EDmgVsBoss weapon)
        {
            // Flat 25% chance to do 2 extra damage
            byte damage = 0;
            double rExtraDmg = r.NextDouble();
            if (rExtraDmg > 0.75)
            {
                damage = 2;
            }

            if (weapon == EDmgVsBoss.U_DamageH)
                damage += (byte)(RWeaponBehavior.AmmoUsage[1] + 1);
            else if (weapon == EDmgVsBoss.U_DamageA)
                damage += (byte)(RWeaponBehavior.AmmoUsage[2] + 1);
            else if (weapon == EDmgVsBoss.U_DamageW)
                damage += (byte)(RWeaponBehavior.AmmoUsage[3] + 1);
            else if (weapon == EDmgVsBoss.U_DamageF)
                return 1;
            else if (weapon == EDmgVsBoss.U_DamageC)
                damage += (byte)(RWeaponBehavior.AmmoUsage[7] + 1);
            
            if (damage < 3) damage = 3;
            return damage;
        }

        private static int GetWeaponIndexFromAddress(EDmgVsBoss weaponAddress)
        {
            if      (weaponAddress == EDmgVsBoss.U_DamageP)
                return 0;
            else if (weaponAddress == EDmgVsBoss.U_DamageH)
                return 1;
            else if (weaponAddress == EDmgVsBoss.U_DamageA)
                return 2;
            else if (weaponAddress == EDmgVsBoss.U_DamageW)
                return 3;
            else if (weaponAddress == EDmgVsBoss.U_DamageB)
                return 4;
            else if (weaponAddress == EDmgVsBoss.U_DamageQ)
                return 5;
            else if (weaponAddress == EDmgVsBoss.U_DamageF)
                return 6;
            else if (weaponAddress == EDmgVsBoss.U_DamageM)
                return 7;
            else if (weaponAddress == EDmgVsBoss.U_DamageC)
                return 8;
            else return -1;
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void RandomizeWilyUJ(Patch Patch, Random r)
        {
            if (IsChaos)
            {
                // List of special weapon damage tables for enemies
                List<EDmgVsEnemy> dmgPtrEnemies = EDmgVsEnemy.GetTables(false);
                EDmgVsEnemy enemyWeak1;
                EDmgVsEnemy enemyWeak2;
                EDmgVsEnemy enemyWeak3;

                // List of special weapon damage tables for bosses (no flash or buster)
                List<EDmgVsBoss> dmgPtrBosses = EDmgVsBoss.GetTables(false, false);
                EDmgVsBoss bossWeak1;
                EDmgVsBoss bossWeak2;
                EDmgVsBoss bossWeak3;
                EDmgVsBoss bossWeak4;

                #region Dragon

                // Dragon
                // 25% chance to have a buster vulnerability
                double rBuster = r.NextDouble();
                byte busterDmg = 0x00;
                if (rBuster > 0.75)
                    busterDmg = 0x01;
                Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Dragon, busterDmg, "Buster Damage to Dragon");
                WilyWeaknesses[0, 0] = busterDmg;

                // Choose 2 special weapon weaknesses
                List<EDmgVsBoss> dragon = new List<EDmgVsBoss>(dmgPtrBosses);
                int rInt = r.Next(dragon.Count);
                bossWeak1 = dragon[rInt];
                dragon.RemoveAt(rInt);
                rInt = r.Next(dragon.Count);
                bossWeak2 = dragon[rInt];

                // For each weapon, apply the weaknesses and immunities
                for (int i = 0; i < dmgPtrBosses.Count; i++)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Dragon weak
                    if (weapon == bossWeak1 || weapon == bossWeak2)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.AmmoUsage[i + 1] > 1)
                        {
                            int tryDamage = (int)RWeaponBehavior.AmmoUsage[i + 1] - 0x01;
                            damage = (tryDamage < 2) ? (byte)0x02 : (byte)tryDamage;
                        }
                        Patch.Add(weapon + EDmgVsBoss.Offset.Dragon, damage, String.Format("{0} Damage to Dragon", weapon.WeaponName));
                        WilyWeaknesses[0, i + 1] = damage;
                    }
                    // Dragon immune
                    else
                    {
                        Patch.Add(weapon + EDmgVsBoss.Offset.Dragon, 0x00, String.Format("{0} Damage to Dragon", weapon.WeaponName));
                        WilyWeaknesses[0, i + 1] = 0x00;
                    }
                }

                #endregion

                #region Picopico-kun

                // Picopico-kun
                // 20 HP each
                // 25% chance for buster to deal 3-7 damage
                rBuster = r.NextDouble();
                busterDmg = 0x00;
                if (rBuster > 0.75)
                {
                    busterDmg = (byte)(r.Next(5) + 3);
                }
                Patch.Add(EDmgVsEnemy.DamageP + EDmgVsEnemy.Offset.PicopicoKun, busterDmg, String.Format("Buster Damage to Picopico-Kun"));
                WilyWeaknesses[1, 0] = busterDmg;

                // Deal ammoUse x 6 for the main weakness
                // Deal ammoUse x 2 for another
                // Deal ammoUse x 1 for another
                List<EDmgVsEnemy> pico = new List<EDmgVsEnemy>(dmgPtrEnemies);
                rInt = r.Next(pico.Count);
                enemyWeak1 = pico[rInt];
                pico.RemoveAt(rInt);
                rInt = r.Next(pico.Count);
                enemyWeak2 = pico[rInt];
                pico.RemoveAt(rInt);
                rInt = r.Next(pico.Count);
                enemyWeak3 = pico[rInt];
                for (int i = 0; i < dmgPtrEnemies.Count; i++)
                {
                    EDmgVsEnemy weapon = dmgPtrEnemies[i];
                    byte damage = 0x00;
                    char level = ' ';

                    // Pico weakness 1, deal ammoUse x8 damage
                    if (weapon == enemyWeak1)
                    {
                        damage = (byte)(RWeaponBehavior.AmmoUsage[i + 1] * 10);
                        if (damage < 2) damage = 3;
                        level = '^';
                    }
                    // weakness 2, deal ammoUse x5 damage
                    else if (weapon == enemyWeak2)
                    {
                        damage = (byte)(RWeaponBehavior.AmmoUsage[i + 1] * 5);
                        if (damage < 2) damage = 2;
                        level = '*';
                    }
                    // weakness 3, deal ammoUse x2 damage
                    else if (weapon == enemyWeak3)
                    {
                        damage = (byte)(RWeaponBehavior.AmmoUsage[i + 1] * 2);
                        if (damage < 2) damage = 2;
                    }

                    // If any weakness is Atomic Fire, deal 20 damage
                    if (weapon == EDmgVsEnemy.DamageH && (enemyWeak1 == weapon || enemyWeak2 == weapon || enemyWeak3 == weapon))
                    {
                        damage = 20;
                    }

                    // Bump up already high damage values to 20
                    if (damage >= 14)
                    {
                        damage = 20;
                    }
                    Patch.Add(weapon + EDmgVsEnemy.Offset.PicopicoKun, damage, String.Format("{0} Damage to Picopico-Kun{1}", weapon.WeaponName, level));
                    WilyWeaknesses[1, i + 1] = damage;
                    WilyWeaknessInfo[1, i + 1] = level;
                }

                #endregion

                #region Guts

                // Guts
                // 25% chance to have a buster vulnerability
                rBuster = r.NextDouble();
                busterDmg = 0x00;
                if (rBuster > 0.75)
                    busterDmg = 0x01;
                Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Guts, busterDmg, String.Format("Buster Damage to Guts Tank"));
                WilyWeaknesses[2, 0] = busterDmg;

                // Choose 2 special weapon weaknesses
                List<EDmgVsBoss> guts = new List<EDmgVsBoss>(dmgPtrBosses);
                rInt = r.Next(guts.Count);
                bossWeak1 = guts[rInt];
                guts.RemoveAt(rInt);
                rInt = r.Next(guts.Count);
                bossWeak2 = guts[rInt];

                for (int i = 0; i < dmgPtrBosses.Count; i++)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Guts weak
                    if (weapon == bossWeak1 || weapon == bossWeak2)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.AmmoUsage[i + 1] > 1)
                        {
                            int tryDamage = (int)RWeaponBehavior.AmmoUsage[i + 1] - 0x01;
                            damage = (tryDamage < 2) ? (byte)0x02 : (byte)tryDamage;
                        }
                        Patch.Add(weapon + EDmgVsBoss.Offset.Guts, damage, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[2, i + 1] = damage;
                    }
                    // Guts immune
                    else
                    {
                        Patch.Add(weapon + EDmgVsBoss.Offset.Guts, 0x00, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[2, i + 1] = 0x00;
                    }
                }

                #endregion

                #region Wily Machine

                // Machine
                // Will have 4 weaknesses and potentially a Buster weakness
                // Phase 1 will disable 2 of the weaknesses, taking no damage
                // Phase 2 will re-enable them, but disable 1 other weakness
                // Mega Man 2 behaves in a similar fashion, disabling Q and A in phase 1, but only disabling H in phase 2

                // 75% chance to have a buster vulnerability
                rBuster = r.NextDouble();
                busterDmg = 0x00;
                if (rBuster > 0.25)
                    busterDmg = 0x01;
                Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Machine, busterDmg, String.Format("Buster Damage to Wily Machine"));
                WilyWeaknesses[2, 0] = busterDmg;

                // Choose 4 special weapon weaknesses
                List<EDmgVsBoss> machine = new List<EDmgVsBoss>(dmgPtrBosses);
                rInt = r.Next(machine.Count);
                bossWeak1 = machine[rInt];
                machine.RemoveAt(rInt);
                rInt = r.Next(machine.Count);
                bossWeak2 = machine[rInt];
                machine.RemoveAt(rInt);
                rInt = r.Next(machine.Count);
                bossWeak3 = machine[rInt];
                machine.RemoveAt(rInt);
                rInt = r.Next(machine.Count);
                bossWeak4 = machine[rInt];

                for (int i = 0; i < dmgPtrBosses.Count; i++)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Machine weak
                    if (weapon == bossWeak1 || weapon == bossWeak2 || weapon == bossWeak3 || weapon == bossWeak4)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        byte damage = 0x01;

                        // Deal damage = ammoUsage
                        if (RWeaponBehavior.AmmoUsage[i + 1] > 1)
                        {
                            damage = (byte)RWeaponBehavior.AmmoUsage[i + 1];
                        }
                        Patch.Add(weapon + EDmgVsBoss.Offset.Machine, damage, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[3, i + 1] = damage;
                    }
                    // Machine immune
                    else
                    {
                        Patch.Add(weapon + EDmgVsBoss.Offset.Machine, 0x00, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[3, i + 1] = 0x00;
                    }

                    // Get index of this weapon out of all weapons 0-8;
                    byte wIndex = (byte)(i + 1);
                    if (weapon == EDmgVsBoss.ClashBomber || weapon == EDmgVsBoss.MetalBlade)
                        wIndex++;

                    // Disable weakness 1 and 2 on Wily Machine Phase 1
                    if (weapon == bossWeak1)
                    {
                        Patch.Add(0x02DA2E, wIndex, String.Format("Wily Machine Phase 1 Resistance 1 ({0})", weapon.WeaponName));
                    }
                    if (weapon == bossWeak2)
                    {
                        Patch.Add(0x02DA32, wIndex, String.Format("Wily Machine Phase 1 Resistance 2 ({0})", weapon.WeaponName));
                    }
                    // Disable weakness 3 on Wily Machine Phase 2
                    if (weapon == bossWeak3)
                    {
                        Patch.Add(0x02DA3A, wIndex, String.Format("Wily Machine Phase 2 Resistance ({0})", weapon.WeaponName));
                    }
                }

                #endregion

                #region Alien

                // Alien
                // Buster Heat Air Wood Bubble Quick Clash Metal
                byte alienDamage = 1;
                List<EDmgVsBoss> alienWeapons = EDmgVsBoss.GetTables(true, false);
                int rWeaponIndex = r.Next(alienWeapons.Count);

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

                // Apply weakness and erase others (flash will remain 0xFF)
                for (int i = 0; i < alienWeapons.Count; i++)
                {
                    EDmgVsBoss weapon = alienWeapons[i];

                    if (i == rWeaponIndex)
                    {
                        Patch.Add(weapon + EDmgVsBoss.Offset.Alien, alienDamage, String.Format("{0} Damage to Alien", weapon.WeaponName));
                        WilyWeaknesses[4, i] = alienDamage;
                    }
                    else
                    {
                        Patch.Add(weapon + EDmgVsBoss.Offset.Alien, 0xFF, String.Format("{0} Damage to Alien", weapon.WeaponName));
                        WilyWeaknesses[4, i] = 0xFF;
                    }
                }

                #endregion

                debug.AppendLine("Wily Boss Weaknesses:");
                debug.AppendLine("P\tH\tA\tW\tB\tQ\tF\tM\tC:");
                debug.AppendLine("--------------------------------------------");
                for (int i = 0; i < WilyWeaknesses.GetLength(0); i++)
                {
                    for (int j = 0; j < WilyWeaknesses.GetLength(1); j++)
                    {
                        debug.Append(String.Format("{0}\t", WilyWeaknesses[i, j]));
                        if (j == 5) debug.Append("X\t"); // skip flash
                    }
                    string bossName = "";
                    switch (i)
                    {
                        case 0:
                            bossName = "dragon";
                            break;
                        case 1:
                            bossName = "picopico-kun";
                            break;
                        case 2:
                            bossName = "guts";
                            break;
                        case 3:
                            bossName = "machine";
                            break;
                        case 4:
                            bossName = "alien";
                            break;
                        default: break;
                    }
                    debug.AppendLine("< " + bossName);
                }
                debug.AppendLine();
            } // end if

            #region Easy Weakness

            else
            {
                // First address for damage (buster v heatman)
                int address = (RandomMM2.Settings.IsJapanese) ? (int)EDmgVsBoss.Buster : (int)EDmgVsBoss.U_DamageP;

                // Skip Time Stopper
                // Buster Air Wood Bubble Quick Clash Metal
                byte[] dragon = new byte[] { 1, 0, 0, 0, 1, 0, 1 };
                byte[] guts = new byte[] { 1, 0, 0, 1, 2, 0, 1 };
                byte[] machine = new byte[] { 1, 1, 0, 0, 1, 1, 4 };
                byte[] alien = new byte[] { 0xff, 0xff, 0xff, 1, 0xff, 0xff, 0xff };

                // TODO: Scale damage based on ammo count w/ weapon class instead of this hard-coded table
                // Buster Air Wood Bubble Quick Clash Metal
                //double[] ammoUsed = new double[] { 0, 2, 3, 0.5, 0.25, 4, 0.25 };

                dragon.Shuffle(r);
                guts.Shuffle(r);
                machine.Shuffle(r);
                alien.Shuffle(r);

                int j = 0;
                for (int i = 0; i < 8; i++) // i = Buster plus 7 weapons, Time Stopper damage is located in another table (going to ignore it anyways)
                {
                    //// Skip Atomic Fire
                    //if (i == 1) continue;

                    int posDragon = address + 14 * i + 8;

                    Patch.Add(posDragon, dragon[j], String.Format("Easy Weakness: ? against Dragon"));
                    Patch.Add(posDragon+2, guts[j], String.Format("Easy Weakness: ? against Guts"));
                    Patch.Add(posDragon+4, machine[j], String.Format("Easy Weakness: ? against Wily Machine"));

                    // Scale damage against alien if using a high ammo usage weapon
                    if (alien[j] == 1)
                    {
                        if (RWeaponBehavior.AmmoUsage[j] >= 1)
                        {
                            alien[j] = (byte)((double)RWeaponBehavior.AmmoUsage[j] * 1.3);
                        }
                    }
                    Patch.Add(posDragon+5, alien[j], String.Format("Easy Weakness: ? against Alien"));
                    j++;
                }
            }
            #endregion
        } // End method RandomizeWilyUJ


    } 
}
