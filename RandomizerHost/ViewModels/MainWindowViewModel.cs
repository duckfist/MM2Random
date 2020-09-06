using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Avalonia;
using MM2Randomizer;
using MM2Randomizer.Utilities;
using ReactiveUI;

namespace RandomizerHost.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //
        // Constructor
        //

        public MainWindowViewModel()
        {
            RandoSettings = new RandoSettings();
            RandomMM2.Settings = RandoSettings;

            // Try to load "MM2.nes" if one is in the local directory already to save time
            String tryLocalpath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "MM2.nes");

            if (File.Exists(tryLocalpath))
            {
                RandoSettings.ValidateFile(tryLocalpath);
                IsShowingHint = false;
            }
        }


        //
        // Properties
        //

        public RandoSettings RandoSettings
        {
            get => this.mRandoSettings;
            set => this.RaiseAndSetIfChanged(ref this.mRandoSettings, value);
        }

        public bool IsShowingHint
        {
            get => this.mIsShowingHint;
            set => this.RaiseAndSetIfChanged(ref this.mIsShowingHint, value);
        }

        public bool HasGeneratedAROM
        {
            get => this.mHasGeneratedAROM;
            set => this.RaiseAndSetIfChanged(ref this.mHasGeneratedAROM, value);
        }

        public bool IsCoreModulesChecked
        {
            get => RandoSettings.Is8StagesRandom &&
                   RandoSettings.IsWeaponsRandom &&
                   RandoSettings.IsTeleportersRandom;
        }


        //
        // Public Methods
        //

        public void PerformRandomization(int seed, bool tryCreateLogFile)
        {
            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.RandomizerCreate(true, seed);

            // Get A-Z representation of seed
            String seedAlpha = SeedConvert.ConvertBase10To26(RandomMM2.Seed);

            if (seed < 0)
            {
                RandoSettings.SeedString = seedAlpha;
            }
            Debug.WriteLine("\nSeed: " + seedAlpha + "\n");

            // Create log file if left shift is pressed while clicking
            if (tryCreateLogFile && !RandoSettings.IsSpoilerFree)
            {
                string logFileName = $"MM2RNG-{seedAlpha}.log";
                using (StreamWriter sw = new StreamWriter(logFileName, false))
                {
                    sw.WriteLine("Mega Man 2 Randomizer");
                    sw.WriteLine($"Version {RandoSettings.AssemblyVersion.ToString()}");
                    sw.WriteLine($"Seed {seedAlpha}\n");
                    sw.WriteLine(RandomMM2.randomStages.ToString());
                    sw.WriteLine(RandomMM2.randomWeaponBehavior.ToString());
                    sw.WriteLine(RandomMM2.randomEnemyWeakness.ToString());
                    sw.WriteLine(RandomMM2.randomWeaknesses.ToString());
                    sw.Write(RandomMM2.Patch.GetStringSortedByAddress());
                }
            }

            // Flag UI as having created a ROM, enabling the "open folder" button
            HasGeneratedAROM = true;
        }


        //
        // Private Data Members
        //

        private RandoSettings mRandoSettings;
        private bool mIsShowingHint = true;
        private bool mHasGeneratedAROM = false;
    }
}
