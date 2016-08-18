using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MM2Randomizer.Enums;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Colors;
using System.Diagnostics;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static Random Random;
        public static MainWindowViewModel Settings;
        public static string DestinationFileName = "";

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static void Randomize()
        {
            try
            {
                CopyRom();
                InitializeSeed();

                if (Settings.Is8StagesRandom)
                {
                    RandomStagePtrs();
                }
                if (Settings.IsWeaponsRandom)
                {
                    RandomWeapons();
                }
                if (Settings.IsItemsRandom)
                {
                    RandomItemNums();
                }
                if (Settings.IsTeleportersRandom)
                {
                    RandomTeleporters();
                }
                if (Settings.IsWeaknessRandom)
                {
                    // Offsets are different in Rockman 2 and Mega Man 2
                    if (Settings.IsJapanese)
                        RandomWeaknesses();
                    else
                        RandomWeaknessesMM2();
                }
                if (Settings.IsEnemiesRandom)
                {
                    RandomEnemies();
                }
                if (Settings.IsColorsRandom)
                {
                    RandomColors();
                }
                if (Settings.IsBGMRandom)
                {
                    RandomBGM();
                }
                if (Settings.IsWeaponNamesRandom)
                {
                    RandomWeaponNames();
                }
                if (Settings.FastText)
                {
                    SetFastText();
                }
                if (Settings.BurstChaserMode)
                {
                    SetBurstChaser();
                }

                string newfilename = (Settings.IsJapanese) ? "RM2" : "MM2";
                newfilename = String.Format("{0}-RNG-{1}.nes", newfilename, Seed);

                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }

                File.Move(DestinationFileName, newfilename);
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", newfilename));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private static void SetFastText()
        {
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                if (Settings.IsJapanese)
                {
                    // Set fast weapon get text (J ONLY)
                    stream.Position = 0x037C51;
                }
                else
                {
                    // Set fast weapon get text (U ONLY)
                    stream.Position = 0x037D4A;
                }
                stream.WriteByte(0x04);
            }
        }

        private static void SetBurstChaser()
        {
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Set fast "ready" text
                stream.Position = 0x038147;
                stream.WriteByte(0x60);

                // Set fast ground speed (integer)
                stream.Position = 0x038921;
                stream.WriteByte(0x03);

                // Set fast ground speed (fraction)
                stream.Position = 0x03892C;
                stream.WriteByte(0x00);

                // Set fast air speed (integer)
                stream.Position = 0x038922;
                stream.WriteByte(0x03);

                // Set fast air speed (fraction)
                stream.Position = 0x03892D;
                stream.WriteByte(0x00);

                // Set fast ladder climb up
                stream.Position = 0x0386EF;
                stream.WriteByte(0x01);

                // Set fast ladder climb down
                stream.Position = 0x03872E;
                stream.WriteByte(0xFE);

                if (Settings.IsJapanese)
                {
                    // Set fast buster projectiles (J ONLY)
                    stream.Position = 0x03D4A4;
                    stream.WriteByte(0x08);
                }
                else
                {
                    // Set fast buster projectiles (U ONLY)
                    stream.Position = 0x03D4A7;
                    stream.WriteByte(0x08);
                }
            }
        }

        private static void RandomEnemies()
        {
            REnemies er = new REnemies();
        }

        private static void RandomTeleporters()
        {
            // Create list of default teleporter position values
            List<byte[]> coords = new List<byte[]>
            {
                new byte[]{ 0x20, 0x3B }, // Teleporter X, Y (top-left)
                new byte[]{ 0x20, 0x7B },
                new byte[]{ 0x20, 0xBB },
                new byte[]{ 0x70, 0xBB },
                new byte[]{ 0x90, 0xBB },
                new byte[]{ 0xE0, 0x3B },
                new byte[]{ 0xE0, 0x7B },
                new byte[]{ 0xE0, 0xBB }
            };

            // Randomize them
            coords.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Write the new x-coordinates
                stream.Position = (long) EMiscAddresses.WarpXCoordinateStartAddress;
                foreach (byte[] location in coords)
                {
                    stream.WriteByte(location[0]);
                }

                // Write the new y-coordinates
                stream.Position = (long) EMiscAddresses.WarpYCoordinateStartAddress;
                foreach (byte[] location in coords)
                {
                    stream.WriteByte(location[1]);
                }

                // These values will be copied over to $04b0 (y) and $0470 (x), which will be checked
                // for in real time to determine where Mega will teleport to
            }
        }

        /// <summary>
        /// Create the modified ROM.
        /// </summary>
        private static void CopyRom()
        {
            string srcFile = "";
            if (Settings.IsJapanese)
            {
                srcFile = "j.dat";
            }
            else
            {
                srcFile = "u.dat";
            }
            DestinationFileName = "temp.nes";

            File.Copy(srcFile, DestinationFileName, true);
        }

        /// <summary>
        /// Create a random seed or use the user-provided seed.
        /// </summary>
        private static void InitializeSeed()
        {
            if (Seed < 0)
            {
                Random rndSeed = new Random();
                Seed = rndSeed.Next(int.MaxValue);
            }
            Random = new Random(Seed);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomColors()
        {
            RColors rColors = new RColors();
        }

        /// <summary>
        /// TODO
        /// </summary>
        private static void RandomBGM()
        {
            List<EMusicID> newBGMOrder = new List<EMusicID>();
            List<EMusicID> robos = new List<EMusicID>();

            // Select 2 replacement tracks for the 2 extra instance of the boring W3/4/5 theme
            robos.Add(EMusicID.Flash);
            robos.Add(EMusicID.Heat);
            robos.Add(EMusicID.Air);
            robos.Add(EMusicID.Wood);
            robos.Add(EMusicID.Quick);
            robos.Add(EMusicID.Metal);
            robos.Add(EMusicID.Clash);
            robos.Add(EMusicID.Bubble);
            robos.Shuffle(Random);
            
            newBGMOrder.Add(EMusicID.Flash);
            newBGMOrder.Add(EMusicID.Heat);
            newBGMOrder.Add(EMusicID.Air);
            newBGMOrder.Add(EMusicID.Wood);
            newBGMOrder.Add(EMusicID.Quick);
            newBGMOrder.Add(EMusicID.Metal);
            newBGMOrder.Add(EMusicID.Clash);
            newBGMOrder.Add(EMusicID.Bubble);
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily345);
            newBGMOrder.Add(robos[0]);
            newBGMOrder.Add(robos[1]);

            // Randomize tracks and write to ROM
            newBGMOrder.Shuffle(Random);
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = 0x0381E0; // Heatman BGM ID, both J and U
                foreach (EMusicID bgm in newBGMOrder)
                {
                    stream.WriteByte((byte)bgm);
                }
            }
        }

        private static void RandomWeaponNames()
        {
            RWeaponNames rWeaponNames = new RWeaponNames();
        }

        /// <summary>
        /// Shuffle which Robot Master awards which weapon.
        /// </summary>
        private static void RandomWeapons()
        {
            // StageBeat    Address    Value
            // -----------------------------
            // Heat Man     0x03C289   1
            // Air Man      0x03C28A   2
            // Wood Man     0x03C28B   4
            // Bubble Man   0x03C28C   8
            // Quick Man    0x03C28D   16
            // Flash Man    0x03C28E   32
            // Metal Man    0x03C28F   64
            // Crash Man    0x03C290   128

            var newWeaponOrder = new List<ERMWeaponValue>()
            {
                ERMWeaponValue.HeatMan,
                ERMWeaponValue.AirMan,
                ERMWeaponValue.WoodMan,
                ERMWeaponValue.BubbleMan,
                ERMWeaponValue.QuickMan,
                ERMWeaponValue.FlashMan,
                ERMWeaponValue.MetalMan,
                ERMWeaponValue.CrashMan
            }.Select(s => (byte)s).ToList();
            
            newWeaponOrder.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Create table for which weapon is awarded by which robot master
                // This also affects which portrait is blacked out on the stage select
                // This also affects which teleporter deactivates after defeating a Wily 5 refight boss
                stream.Position = (long) ERMStageWeaponAddress.HeatMan;// 0x03c289;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
                }

                // Create a copy of the default weapon order table to be used by teleporter function
                // This is needed to fix teleporters breaking from the new weapon order.
                //stream.Position = 0x03f2D0; // Unused space at end of bank
                stream.Position = 0x03f310; // Unused space at end of bank
                stream.WriteByte((byte) ERMWeaponValue.HeatMan);
                stream.WriteByte((byte) ERMWeaponValue.AirMan);
                stream.WriteByte((byte) ERMWeaponValue.WoodMan);
                stream.WriteByte((byte) ERMWeaponValue.BubbleMan);
                stream.WriteByte((byte) ERMWeaponValue.QuickMan);
                stream.WriteByte((byte) ERMWeaponValue.FlashMan);
                stream.WriteByte((byte) ERMWeaponValue.MetalMan);
                stream.WriteByte((byte) ERMWeaponValue.CrashMan);

                // Change function to call $f300 instead of $c279 when looking up defeated refight boss to
                // get our default weapon table, fixing the teleporter softlock
                stream.Position = 0x03843b;
                stream.WriteByte(0x00);
                stream.WriteByte(0xf3);

                // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
                stream.Position = (long) ERMStageSelect.FirstStageInMemory; // 0x0346E1;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
                }
            }
        }

        /// <summary>
        /// Shuffle which Robot Master awards Items 1, 2, and 3.
        /// </summary>
        private static void RandomItemNums()
        {
            // 0x03C291 - Item # from Heat Man
            // 0x03C292 - Item # from Air Man
            // 0x03C293 - Item # from Wood Man
            // 0x03C294 - Item # from Bubble Man
            // 0x03C295 - Item # from Quick Man
            // 0x03C296 - Item # from Flash Man
            // 0x03C297 - Item # from Metal Man
            // 0x03C298 - Item # from Crash Man

            List<EItemNumber> newItemOrder = new List<EItemNumber>();
            for (byte i = 0; i < 5; i++) newItemOrder.Add(EItemNumber.None);
            newItemOrder.Add(EItemNumber.One);
            newItemOrder.Add(EItemNumber.Two);
            newItemOrder.Add(EItemNumber.Three);

            newItemOrder.Shuffle(Random);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = (long) EItemStageAddress.HeatMan; //0x03C291;
                for (int i = 0; i < 8; i++)
                { 
                    stream.WriteByte((byte)newItemOrder[i]);
                }


            }
        }

        /// <summary>
        /// Modify the damage values of each weapon against each Robot Master for Rockman 2 (J).
        /// </summary>
        private static void RandomWeaknesses()
        {
            List<WeaponTable> Weapons = new List<WeaponTable>();

            Weapons.Add(new WeaponTable()
            {
                Name = "Buster",
                ID = 0,
                Address = ERMWeaponAddress.Buster,
                RobotMasters = new int[8] { 2,2,1,1,2,2,1,1 }
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
                RobotMasters = new int[8] {2, 0, 4, 0, 2, 0, 0, 0x0A }
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
                weapon.RobotMasters.Shuffle(Random);
            }

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
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
        private static void RandomWeaknessesMM2()
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
                weapon.RobotMasters.Shuffle(Random);
            }

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (WeaponTable weapon in Weapons)
                {
                    stream.Position = (long) weapon.Address;
                    for (int i = 0; i < 8; i++)
                    {
                        stream.WriteByte((byte)weapon.RobotMasters[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Shuffle the Robot Master stages.  This shuffling will not be indicated by the Robot Master portraits.
        /// </summary>
        private static void RandomStagePtrs()
        {
            // StageSelect  Address    Value
            // -----------------------------
            // Bubble Man   0x034670   3
            // Air Man      0x034671   1
            // Quick Man    0x034672   4
            // Wood Man     0x034673   2
            // Crash Man    0x034674   7
            // Flash Man    0x034675   5
            // Metal Man    0x034676   6
            // Heat Man     0x034677   0
            
            List<StageFromSelect> StageSelect = new List<StageFromSelect>();

            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Bubble Man",
                PortraitAddress = ERMPortraitAddress.BubbleMan,
                PortraitDestinationOriginal = 3,
                PortraitDestinationNew = 3,
                StageClearAddress = ERMStageClearAddress.BubbleMan,
                StageClearDestinationOriginal = 8,
                StageClearDestinationNew = 8
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Air Man",
                PortraitAddress = ERMPortraitAddress.AirMan,
                PortraitDestinationOriginal = 1,
                PortraitDestinationNew = 1,
                StageClearAddress = ERMStageClearAddress.AirMan,
                StageClearDestinationOriginal = 2,
                StageClearDestinationNew = 2
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Quick Man",
                PortraitAddress = ERMPortraitAddress.QuickMan,
                PortraitDestinationOriginal = 4,
                PortraitDestinationNew = 4,
                StageClearAddress = ERMStageClearAddress.QuickMan,
                StageClearDestinationOriginal = 16,
                StageClearDestinationNew = 16
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Wood Man",
                PortraitAddress = ERMPortraitAddress.WoodMan,
                PortraitDestinationOriginal = 2,
                PortraitDestinationNew = 2,
                StageClearAddress = ERMStageClearAddress.WoodMan,
                StageClearDestinationOriginal = 4,
                StageClearDestinationNew = 4
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Clash Man",
                PortraitAddress = ERMPortraitAddress.CrashMan,
                PortraitDestinationOriginal = 7,
                PortraitDestinationNew = 7,
                StageClearAddress = ERMStageClearAddress.CrashMan,
                StageClearDestinationOriginal = 128,
                StageClearDestinationNew = 128
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Flash Man",
                PortraitAddress = ERMPortraitAddress.FlashMan,
                PortraitDestinationOriginal = 5,
                PortraitDestinationNew = 5,
                StageClearAddress = ERMStageClearAddress.FlashMan,
                StageClearDestinationOriginal = 32,
                StageClearDestinationNew = 32
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Metal Man",
                PortraitAddress = ERMPortraitAddress.MetalMan,
                PortraitDestinationOriginal = 6,
                PortraitDestinationNew = 6,
                StageClearAddress = ERMStageClearAddress.MetalMan,
                StageClearDestinationOriginal = 64,
                StageClearDestinationNew = 64
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Heat Man",
                PortraitAddress = ERMPortraitAddress.HeatMan,
                PortraitDestinationOriginal = 0,
                PortraitDestinationNew = 0,
                StageClearAddress = ERMStageClearAddress.HeatMan,
                StageClearDestinationOriginal = 1,
                StageClearDestinationNew = 1
            });


            List<byte> newStageOrder = new List<byte>();
            for (byte i = 0; i < 8; i++) newStageOrder.Add(i);

            newStageOrder.Shuffle(Random);

            for (int i = 0; i < 8; i++)
            {
                string portrait = StageSelect[i].PortraitName;
                StageSelect[i].PortraitDestinationNew = StageSelect[newStageOrder[i]].PortraitDestinationOriginal;
                //StageSelect[i].StageClearDestinationNew = StageSelect[newStageOrder[i]].StageClearDestinationOriginal;
            }
            
            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (StageFromSelect stage in StageSelect)
                {
                    stream.Position = (long)stage.PortraitAddress;
                    stream.WriteByte((byte)stage.PortraitDestinationNew);
                    //stream.Position = stage.StageClearAddress;
                    //stream.WriteByte((byte)stage.StageClearDestinationNew);
                }
            }
        }

        /// <summary>
        /// Shuffle the elements of the provided list.
        /// </summary>
        /// <typeparam name="T">The Type of the elements in the list.</typeparam>
        /// <param name="list">The object to be shuffled.</param>
        /// <param name="rng">The seed used to perform the shuffling.</param>
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
