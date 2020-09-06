using MM2Randomizer;
using MM2Randomizer.Utilities;

using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace MM2RandoHost.ViewModels
{
    public class MainWindowViewModel : ObservableBase
    {
        private RandoSettings _randoSettings;
        private bool _isShowingHint = true;
        private bool _hasGeneratedAROM = false;

        public MainWindowViewModel()
        {
            RandoSettings = new RandoSettings();
            RandomMM2.Settings = RandoSettings;

            // Try to load "MM2.nes" if one is in the local directory already to save time
            string tryLocalpath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "MM2.nes");

            if (File.Exists(tryLocalpath))
            {
                RandoSettings.ValidateFile(tryLocalpath);
                IsShowingHint = false;
            }
        }

        public RandoSettings RandoSettings
        {
            get => _randoSettings;
            set => SetProperty(ref _randoSettings, value);
        }

        public bool IsShowingHint
        {
            get => _isShowingHint;
            set => SetProperty(ref _isShowingHint, value);
        }

        public bool HasGeneratedAROM
        {
            get => _hasGeneratedAROM;
            set => SetProperty(ref _hasGeneratedAROM, value);
        }

        public bool IsCoreModulesChecked
        {
            get => RandoSettings.Is8StagesRandom &&
                   RandoSettings.IsWeaponsRandom &&
                   RandoSettings.IsTeleportersRandom;
        }

        public void PerformRandomization(int seed, bool tryCreateLogFile)
        {
            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.RandomizerCreate(true, seed);

            // Get A-Z representation of seed
            string seedAlpha = SeedConvert.ConvertBase10To26(RandomMM2.Seed);
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
    }
}
