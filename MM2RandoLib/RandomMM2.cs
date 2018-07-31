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
using MM2Randomizer.Randomizers.Stages.Components;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        public static int Seed = -1;
        public static Random Random;
        public static Random RNGCosmetic;
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
        public static List<IRandomizer> CosmeticRandomizers;

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static string RandomizerCreate(bool fromClientApp)
        {
            // List of randomizer modules to use; will add modules based on checkbox states
            Randomizers = new List<IRandomizer>();
            CosmeticRandomizers = new List<IRandomizer>();


            ///==========================
            /// "CORE" MODULES
            ///==========================
            // NOTE: Just in case, link RStages, RWeaponGet, and RTeleporter into one "Core Randomizer" module
            // Their interdependencies are too risky to separate as options, and likely nobody will want to customize this part anyways.
            // Random portrait locations on stage select
            randomStages = new RStages();
            // Random weapon awarded from each stage
            // WARNING: May be dependent on RTeleporters, verify?
            // WARNING: May be dependent on RStages
            randomWeaponGet = new RWeaponGet();
            // Random teleporter destinations in Wily 5
            randomTeleporters = new RTeleporters();


            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================
            // Caution: Depends on weaknesses, but can use default values if its not enabled.
            randomWeaponBehavior = new RWeaponBehavior();

            // Independent
            randomWeaknesses = new RWeaknesses();

            // Caution: RText depends on this, but default values will be used if not enabled.
            randomBossInBossRoom = new RBossRoom();

            // Independent
            randomBossAI = new RBossAI();

            // Independent
            randomItemGet = new RItemGet();

            // Independent
            randomEnemies = new REnemies();

            // Independent
            randomEnemyWeakness = new REnemyWeaknesses();

            // Independent
            randomTilemap = new RTilemap();


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================
            // Caution: Depends on RBossRoom, but can use default values if its not enabled.
            rWeaponNames = new RText();

            // Independent
            randomColors = new RColors();

            // Independent
            randomMusic = new RMusic();





            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================
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

            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================
            if (Settings.IsColorsRandom)
            {
                CosmeticRandomizers.Add(randomColors);
            }
            if (Settings.IsBGMRandom)
            {
                CosmeticRandomizers.Add(randomMusic);
            }
            if (Settings.IsWeaponNamesRandom)
            {
                CosmeticRandomizers.Add(rWeaponNames);
            }

                
            // Instantiate RNG object r based on RandomMM2.Seed
            InitializeSeed();

            // Create randomization patch
            Patch = new Patch();

            // Conduct randomization of Gameplay Modules
            foreach (IRandomizer randomizer in Randomizers)
            {
                randomizer.Randomize(Patch, Random);
                Debug.WriteLine(randomizer);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in CosmeticRandomizers)
            {
                RNGCosmetic = new Random(Seed);
                cosmetic.Randomize(Patch, RNGCosmetic);
                Debug.WriteLine(cosmetic);
            }

            // Apply additional required incidental modifications
            if (Settings.Is8StagesRandom || Settings.IsWeaponsRandom)
            {
                MiscHacks.FixPortraits(Patch, Settings.Is8StagesRandom, randomStages, Settings.IsWeaponsRandom, randomWeaponGet);
            }
            if (Settings.IsEnemiesRandom)
            {
                MiscHacks.FixM445PaletteGlitch(Patch);
            }

            // Apply final optional gameplay modifications
            if (Settings.FastText)
            {
                MiscHacks.SetFastText(Patch);
            }
            if (Settings.BurstChaserMode)
            {
                MiscHacks.SetBurstChaser(Patch);
            }
            MiscHacks.DrawTitleScreenChanges(Patch, Seed, Settings.IsTournamentMode);
            MiscHacks.SetWily5NoMusicChange(Patch);
            MiscHacks.FixDamageValues(Patch);
            MiscHacks.SetETankKeep(Patch);
            MiscHacks.SkipItemGetPages(Patch);

            // Create file name based on seed and game region
            string seedAlpha = SeedConvert.ConvertBase10To26(Seed);
            string newfilename = $"MM2-RNG-{seedAlpha}.nes";

            // Apply patch and deliver the ROM; different routine for client vs. web app
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
            RNGCosmetic = new Random(Seed);
        }

        /// <summary>
        /// Shuffle the elements of the provided list.
        /// </summary>
        /// <typeparam name="T">The Type of the elements in the list.</typeparam>
        /// <param name="list">The object to be shuffled.</param>
        /// <param name="rng">The seed used to perform the shuffling.</param>
        /// <returns>A reference to the shuffled list.</returns>
        public static IList<T> Shuffle<T>(this IList<T> list, Random rng)
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
            return list;
        }
    }
}
