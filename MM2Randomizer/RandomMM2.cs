using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static MainWindowViewModel Settings;

        public static string DestinationFileName = "";

        public static void Randomize()
        {
            try
            {
                CopyRom();
                
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


                string newfilename = (Settings.IsJapanese) ? "RM2" : "MM2";
                newfilename = String.Format("{0}-RNG-{1}.nes", newfilename, Seed);

                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }

                File.Move(DestinationFileName, newfilename);

                string str = string.Format("/select,\"{0}\\{1}\"",
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                newfilename);
                //Process.Start("explorer.exe", str);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
                
        public static void CopyRom()
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

        public static void RandomWeapons()
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

            List<byte> newWeaponOrder = new List<byte>() { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

            if (Seed < 0)
            {
                Random rndSeed = new Random();
                Seed = rndSeed.Next(int.MaxValue);
            }
            Random rng = new Random(Seed);

            newWeaponOrder.Shuffle(rng);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Create table for which weapon is awarded by which robot master
                // This also affects which portrait is blacked out on the stage select
                stream.Position = 0x03c289;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
                }

                // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
                stream.Position = 0x0346E1;
                for (int i = 0; i < 8; i++)
                {
                    stream.WriteByte((byte)newWeaponOrder[i]);
                }
            }
        }


        public static void RandomItemNums()
        {
            // 0x03C291 - Item # from Heat Man
            // 0x03C292 - Item # from Air Man
            // 0x03C293 - Item # from Wood Man
            // 0x03C294 - Item # from Bubble Man
            // 0x03C295 - Item # from Quick Man
            // 0x03C296 - Item # from Flash Man
            // 0x03C297 - Item # from Metal Man
            // 0x03C298 - Item # from Crash Man

            List<byte> newItemOrder = new List<byte>();
            for (byte i = 0; i < 5; i++) newItemOrder.Add(0);
            newItemOrder.Add(1);
            newItemOrder.Add(2);
            newItemOrder.Add(4);

            if (Seed < 0)
            {
                Random rndSeed = new Random();
                Seed = rndSeed.Next(int.MaxValue);
            }
            Random rng = new Random(Seed);

            newItemOrder.Shuffle(rng);

            using (var stream = new FileStream(DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = 0x03C291;
                for (int i = 0; i < 8; i++)
                { 
                    stream.WriteByte((byte)newItemOrder[i]);
                }


            }
        }

        public static void RandomStagePtrs()
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
                PortraitAddress = 0x034670,
                PortraitDestinationOriginal = 3,
                PortraitDestinationNew = 3,
                StageClearAddress = 0x03C28C,
                StageClearDestinationOriginal = 8,
                StageClearDestinationNew = 8
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Air Man",
                PortraitAddress = 0x034671,
                PortraitDestinationOriginal = 1,
                PortraitDestinationNew = 1,
                StageClearAddress = 0x03C28A,
                StageClearDestinationOriginal = 2,
                StageClearDestinationNew = 2
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Quick Man",
                PortraitAddress = 0x034672,
                PortraitDestinationOriginal = 4,
                PortraitDestinationNew = 4,
                StageClearAddress = 0x03C28D,
                StageClearDestinationOriginal = 16,
                StageClearDestinationNew = 16
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Wood Man",
                PortraitAddress = 0x034673,
                PortraitDestinationOriginal = 2,
                PortraitDestinationNew = 2,
                StageClearAddress = 0x03C28B,
                StageClearDestinationOriginal = 4,
                StageClearDestinationNew = 4
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Clash Man",
                PortraitAddress = 0x034674,
                PortraitDestinationOriginal = 7,
                PortraitDestinationNew = 7,
                StageClearAddress = 0x03C290,
                StageClearDestinationOriginal = 128,
                StageClearDestinationNew = 128
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Flash Man",
                PortraitAddress = 0x034675,
                PortraitDestinationOriginal = 5,
                PortraitDestinationNew = 5,
                StageClearAddress = 0x03C28E,
                StageClearDestinationOriginal = 32,
                StageClearDestinationNew = 32
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Metal Man",
                PortraitAddress = 0x034676,
                PortraitDestinationOriginal = 6,
                PortraitDestinationNew = 6,
                StageClearAddress = 0x03C28F,
                StageClearDestinationOriginal = 64,
                StageClearDestinationNew = 64
            });
            StageSelect.Add(new StageFromSelect()
            {
                PortraitName = "Heat Man",
                PortraitAddress = 0x034677,
                PortraitDestinationOriginal = 0,
                PortraitDestinationNew = 0,
                StageClearAddress = 0x03C289,
                StageClearDestinationOriginal = 1,
                StageClearDestinationNew = 1
            });


            List<byte> newStageOrder = new List<byte>();
            for (byte i = 0; i < 8; i++) newStageOrder.Add(i);
            
            if (Seed < 0)
            {
                Random rndSeed = new Random();
                Seed = rndSeed.Next(int.MaxValue);
            }
            Random rng = new Random(Seed);

            newStageOrder.Shuffle(rng);

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
                    stream.Position = stage.PortraitAddress;
                    stream.WriteByte((byte)stage.PortraitDestinationNew);
                    //stream.Position = stage.StageClearAddress;
                    //stream.WriteByte((byte)stage.StageClearDestinationNew);
                }
            }
        }

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
