using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;

using MM2Randomizer.Patcher;
using MM2Randomizer.Enums;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static Random Random;
        public static Patch Patch;
        public static RandoSettings Settings;
        public static readonly string TempFileName = "temp.nes";
        public static string RecentlyCreatedFileName = "";

        public static RStages randomStages;
        public static RWeaponGet randomWeaponGet;
        public static RWeaponBehavior randomWeaponBehavior;
        public static RWeaknesses randomWeaknesses;
        public static RBossAI randomBossAI;
        public static RItemGet randomItemGet;
        public static RTeleporters randomTeleporters;
        public static REnemies randomEnemies;
        public static REnemyWeaknesses randomEnemyWeakness;
        public static RBossRoom randomBossInBossRoom;
        public static RTilemap randomTilemap;
        public static RColors randomColors;
        public static RMusic randomMusic;
        public static RText rWeaponNames;
        public static List<IRandomizer> Randomizers;

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static string RandomizerCreate(bool fromClientApp)
        {
            randomStages = new RStages();
            randomWeaponGet = new RWeaponGet();
            randomWeaponBehavior = new RWeaponBehavior();
            randomWeaknesses = new RWeaknesses(true);
            randomBossAI = new RBossAI();
            randomItemGet = new RItemGet();
            randomTeleporters = new RTeleporters();
            randomEnemies = new REnemies();
            randomEnemyWeakness = new REnemyWeaknesses();
            randomBossInBossRoom = new RBossRoom();
            randomTilemap = new RTilemap();
            randomColors = new RColors();
            randomMusic = new RMusic();
            rWeaponNames = new RText();

            Randomizers = new List<IRandomizer>();

            // Add randomizers according to each flag
            if (Settings.Is8StagesRandom)
            {
                Randomizers.Add(randomStages);
            }
            if (Settings.IsWeaponsRandom)
            {
                Randomizers.Add(randomWeaponGet);
            }
            if (Settings.IsWeaponBehaviorRandom)
            {
                Randomizers.Add(randomWeaponBehavior);
            }
            if (Settings.IsWeaknessRandom)
            {
                Randomizers.Add(randomWeaknesses);
            }
            if (Settings.IsBossAIRandom)
            {
                Randomizers.Add(randomBossAI);
            }
            if (Settings.IsItemsRandom)
            {
                Randomizers.Add(randomItemGet);
            }
            if (Settings.IsTeleportersRandom)
            {
                Randomizers.Add(randomTeleporters);
            }
            if (Settings.IsEnemiesRandom)
            {
                Randomizers.Add(randomEnemies);
            }
            if (Settings.IsEnemyWeaknessRandom)
            {
                Randomizers.Add(randomEnemyWeakness);
            }
            if (Settings.IsBossInBossRoomRandom)
            {
                Randomizers.Add(randomBossInBossRoom);
            }
            if (Settings.IsTilemapChangesEnabled)
            {
                Randomizers.Add(randomTilemap);
            }
            if (Settings.IsColorsRandom)
            {
                Randomizers.Add(randomColors);
            }
            if (Settings.IsBGMRandom)
            {
                Randomizers.Add(randomMusic);
            }
            if (Settings.IsWeaponNamesRandom)
            {
                Randomizers.Add(rWeaponNames);
            }
                
            // Instantiate RNG object r based on RandomMM2.Seed
            InitializeSeed();

            // Create randomization patch
            Patch = new Patch();
            foreach (IRandomizer randomizer in Randomizers)
            {
                randomizer.Randomize(Patch, Random);
                Debug.WriteLine(randomizer);
            }

            // Create patch with additional modifications
            if (Settings.FastText)
            {
                MiscHacks.SetFastText(Patch, Settings.IsJapanese);
            }
            if (Settings.BurstChaserMode)
            {
                MiscHacks.SetBurstChaser(Patch, Settings.IsJapanese);
            }
            if (Settings.Is8StagesRandom || Settings.IsWeaponsRandom)
            {
                MiscHacks.FixPortraits(Patch, Settings.Is8StagesRandom, randomStages, Settings.IsWeaponsRandom, randomWeaponGet);
            }
            if (Settings.IsEnemiesRandom)
            {
                MiscHacks.FixM445PaletteGlitch(Patch);
            }
            if (!Settings.IsJapanese)
            {
                MiscHacks.DrawTitleScreenChanges(Patch, Seed);
            }
            MiscHacks.SetWily5NoMusicChange(Patch, Settings.IsJapanese);
            MiscHacks.FixDamageValues(Patch);

            // Create file name based on seed and game region
            string newfilename = (Settings.IsJapanese) ? "RM2" : "MM2";
            string seedAlpha = SeedConvert.ConvertBase10To26(Seed);
            newfilename = $"{newfilename}-RNG-{seedAlpha}.nes";

            var assembly = Assembly.GetExecutingAssembly();
            if (fromClientApp)
            {
                //File.Copy(Settings.SourcePath, TempFileName, true);
                using (Stream stream = assembly.GetManifestResourceStream("MM2Randomizer.Resources.MM2.nes"))
                {
                    using (Stream output = File.OpenWrite(TempFileName))
                    {
                        stream.CopyTo(output);
                    }
                }

                // Apply pre-patch changes via IPS patch (manual title screen, stage select, and stage changes)
                Patch.ApplyIPSPatch(TempFileName, Properties.Resources.mm2rng_musicpatch);
                Patch.ApplyIPSPatch(TempFileName, Properties.Resources.mm2rng_prepatch);

                // Apply patch with randomized content
                Patch.ApplyRandoPatch(TempFileName);

                // If a file of the same seed already exists, delete it
                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }

                // Finish the copy/rename and open Explorer at that location
                File.Move(TempFileName, newfilename);
                RecentlyCreatedFileName = newfilename;
                return newfilename;
            }
            else
            {
                //File.Copy(Settings.SourcePath, TempFileName, true);
                string serverDir = $@"C:\mm2rng\{seedAlpha}";
                Directory.CreateDirectory(serverDir);

                string serverPathTemp = Path.Combine(serverDir, TempFileName);
                string serverPathNew = Path.Combine(serverDir, newfilename);
                using (Stream stream = assembly.GetManifestResourceStream("MM2Randomizer.Resources.MM2.nes"))
                {
                    using (Stream output = File.OpenWrite(serverPathTemp))
                    {
                        stream.CopyTo(output);
                    }
                }

                // Apply pre-patch changes via IPS patch (manual title screen, stage select, and stage changes)
                Patch.ApplyIPSPatch(serverPathTemp, Properties.Resources.mm2rng_musicpatch);
                Patch.ApplyIPSPatch(serverPathTemp, Properties.Resources.mm2rng_prepatch);

                // Apply patch with randomized content
                Patch.ApplyRandoPatch(serverPathTemp);

                // If a file of the same seed already exists, delete it
                if (File.Exists(serverPathNew))
                {
                    File.Delete(serverPathNew);
                }

                // Finish the copy/rename and open Explorer at that location
                File.Move(serverPathTemp, serverPathNew);
                RecentlyCreatedFileName = serverPathNew;
                return serverPathNew;
            }
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
