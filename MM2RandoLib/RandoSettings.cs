using System;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MM2Randomizer
{
    public class RandoSettings : INotifyPropertyChanged
    {
        private string seedString;
        private string sourcePath;
        private bool isSourcePathValid;
        private bool isSeedValid;
        private bool isSourcePathAndSeedValid;


        public RandoSettings()
        {
            // Rando assembly state variables
            SeedString = "";
            SourcePath = "";
            IsSeedValid = false;
            IsSourcePathValid = false;
            IsSourcePathAndSeedValid = false;

            // Flags for Rando Core Modules
            Is8StagesRandom = true;
            IsWeaponsRandom = true;
            IsTeleportersRandom = true;

            // Flags for Rando Gameplay Modules
            IsWeaponBehaviorRandom = true;
            IsWeaknessRandom = true;
            IsBossInBossRoomRandom = true;
            IsBossAIRandom = true;
            IsItemsRandom = true;
            IsEnemiesRandom = true;
            IsEnemyWeaknessRandom = true;
            IsTilemapChangesEnabled = true;

            // Flags for Rando Cosmetic Modules
            IsWeaponNamesRandom = true;
            IsColorsRandom = true;
            IsBGMRandom = true;

            // Flags for Optional Gameplay Modules
            FastText = true;
            BurstChaserMode = false;
        }

        /// <summary>
        /// Alphabetical string representation of the RandomMM2.Seed integer of the most
        /// recently generated ROM.
        /// </summary>
        public string SeedString
        {
            get { return seedString; }
            set
            {
                value = value.ToUpper();
                if (seedString != value)
                {
                    seedString = value;
                    OnPropertyChanged();

                    // TODO: Check for better validity of seed
                    IsSeedValid = (seedString == "") ? false : true;
                    IsSourcePathAndSeedValid = IsSourcePathValid && IsSeedValid;
                }
            }
        }

        /// <summary>
        /// Full path to user-provided ROM to apply patch.
        /// </summary>
        public string SourcePath
        {
            get { return sourcePath; }
            set
            {
                if (sourcePath != value)
                {
                    sourcePath = value;
                    OnPropertyChanged();

                    // Check if source path is valid
                    IsSourcePathValid = System.IO.File.Exists(value);
                    IsSourcePathAndSeedValid = IsSourcePathValid && IsSeedValid;
                }
            }
        }

        public bool IsSourcePathValid
        {
            get
            {
                return isSourcePathValid;
            }
            set
            {
                if (isSourcePathValid != value)
                {
                    isSourcePathValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSeedValid
        {
            get { return isSeedValid; }
            set
            {
                if (isSeedValid != value)
                {
                    isSeedValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSourcePathAndSeedValid
        {
            get
            {
                return isSourcePathAndSeedValid;
            }
            set
            {
                if (isSourcePathAndSeedValid != value)
                {
                    isSourcePathAndSeedValid = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// If True, the Robot Master stages will be shuffled and will not be indicated by the
        /// portraits on the Stage Select screen.
        /// </summary>
        public bool Is8StagesRandom { get; set; }

        /// <summary>
        /// If True, the weapons awarded from each Robot Master is shuffled.
        /// </summary>
        public bool IsWeaponsRandom { get; set; }

        /// <summary>
        /// If True, Items 1, 2, and 3 will be awarded from random Robot Masters.
        /// </summary>
        public bool IsItemsRandom { get; set; }

        /// <summary>
        /// If true, in Wily 5, the Robot Master locations in each teleporter is randomized.
        /// </summary>
        public bool IsTeleportersRandom { get; set; }

        /// <summary>
        /// If True, the damage each weapon does against each Robot Master is changed. The manner in
        /// which it is changed depends on if IsWeaknessEasy is True or if IsWeaknessHard is True.
        /// </summary>
        public bool IsWeaknessRandom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBossAIRandom { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsColorsRandom { get;  set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsEnemiesRandom { get; set; }

        public bool IsEnemyWeaknessRandom { get; set; } 

        public bool IsBossInBossRoomRandom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTilemapChangesEnabled { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsBGMRandom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWeaponNamesRandom { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool FastText { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool BurstChaserMode { get; set; }

        public bool IsWeaponBehaviorRandom { get; set; }
        
        /// <summary>
        /// Get this assembly version as a bindable property.
        /// </summary>
        public Version AssemblyVersion
        {
            get
            {
                return Assembly.GetAssembly(typeof(RandomMM2)).GetName().Version;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise event to update bound GUI controls
        /// </summary>
        /// <param name="name">Name of updated property.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
