using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MM2Randomizer
{
    public class RandoSettings : ObservableBase
    {
        private string seedString;
        private string sourcePath;
        private bool isSourcePathValid;
        private bool isSeedValid;
        private bool isSourcePathAndSeedValid;
        private string hashStringMD5;
        private string hashStringSHA256;
        private string hashValidationMessage;
        private bool isHashValid;
        private bool isSpoilerFree;

        public readonly string[] ExpectedMD5s = new string[]
        {
            "caaeb9ee3b52839de261fd16f93103e6", // Mega Man 2 (U)
            "8e4bc5b03ffbd4ef91400e92e50dd294", // Mega Man 2 (USA)
        };

        public readonly string[] ExpectedSHA256s = new string[]
        {
            "27b5a635df33ed57ed339dfc7fd62fc603b39c1d1603adb5cdc3562a0b0d555b", // Mega Man 2 (U)
            "49136b412ff61beac6e40d0bbcd8691a39a50cd2744fdcdde3401eed53d71edf", // Mega Man 2 (USA)
        };

        public RandoSettings()
        {
            // Rando assembly state variables
            SeedString = "";
            SourcePath = "";
            IsSeedValid = false;
            IsSourcePathValid = false;
            IsSourcePathAndSeedValid = false;
            HashStringMD5 = "";
            HashStringSHA256 = "";
            HashValidationMessage = "";
            IsHashValid = false;

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
            IsSpoilerFree = false;
        }

        #region Meta Properties

        /// <summary>
        /// Alphabetical string representation of the RandomMM2.Seed integer of the most
        /// recently generated ROM.
        /// </summary>
        public string SeedString
        {
            get => seedString;
            set
            {
                value = value.ToUpper();
                if (seedString != value)
                {
                    seedString = value;
                    NotifyPropertyChanged();

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
            get => sourcePath;
            set => SetProperty(ref sourcePath, value);
        }

        public bool IsSourcePathValid
        {
            get => isSourcePathValid;
            set => SetProperty(ref isSourcePathValid, value);
        }

        public bool IsSeedValid
        {
            get => isSeedValid;
            set => SetProperty(ref isSeedValid, value);
        }

        // TODO need this?
        public bool IsSourcePathAndSeedValid
        {
            get => isSourcePathAndSeedValid;
            set => SetProperty(ref isSourcePathAndSeedValid, value);
        }

        public bool IsSpoilerFree
        {
            get => isSpoilerFree;
            set => SetProperty(ref isSpoilerFree, value);
        }

        public string HashStringMD5
        {
            get => hashStringMD5;
            set => SetProperty(ref hashStringMD5, value);
        }

        public string HashStringSHA256
        {
            get => hashStringSHA256;
            set => SetProperty(ref hashStringSHA256, value);
        }

        public string HashValidationMessage
        {
            get => hashValidationMessage;
            set => SetProperty(ref hashValidationMessage, value);
        }

        public bool IsHashValid
        {
            get => isHashValid;
            set => SetProperty(ref isHashValid, value);
        }

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

        #endregion

        #region Randomizer Flags

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

        public bool IsStageNameHidden { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool BurstChaserMode { get; set; }

        public bool IsWeaponBehaviorRandom { get; set; }

        #endregion

        public string GetFlagsString()
        {
            StringBuilder sb = new StringBuilder();
            if (Is8StagesRandom)         sb.Append('A');    else sb.Append(' ');
            if (IsWeaponsRandom)         sb.Append('B');    else sb.Append(' ');
            if (IsTeleportersRandom)     sb.Append('C');    else sb.Append(' ');
            if (IsWeaponBehaviorRandom)  sb.Append('D');    else sb.Append(' ');
            if (IsWeaknessRandom)        sb.Append('E');    else sb.Append(' ');
            if (IsBossInBossRoomRandom)  sb.Append('F');    else sb.Append(' ');
            if (IsBossAIRandom)          sb.Append('G');    else sb.Append(' ');
            if (IsItemsRandom)           sb.Append('H');    else sb.Append(' ');
            if (IsEnemiesRandom)         sb.Append('I');    else sb.Append(' ');
            if (IsEnemyWeaknessRandom)   sb.Append('J');    else sb.Append(' ');
            if (IsTilemapChangesEnabled) sb.Append('K');    else sb.Append(' ');
            return sb.ToString();
        }

        /// <summary>
        /// This method checks that a file exists and then compares its checksum with known good Mega Man 2 ROMs.
        /// If it fails any of this, the method returns false.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ValidateFile(string path)
        {
            // Check if file even exists
            SourcePath = path;
            IsSourcePathValid = System.IO.File.Exists(SourcePath);
            IsSourcePathAndSeedValid = IsSourcePathValid && IsSeedValid;

            if (!IsSourcePathValid)
            {
                HashValidationMessage = "File does not exist.";
                IsHashValid = false;
                return false;
            }

            // Ensure file size is small so that we can take the hash
            var info = new System.IO.FileInfo(path);
            long size = info.Length;
            if (size > 2000000)
            {
                decimal MB = (size / (decimal)(1024d * 1024d));
                HashValidationMessage = $"File is {MB:0.00} MB, clearly not a NES ROM. WTF are you doing?";
                IsSourcePathValid = false;
                IsHashValid = false;
                return false;
            }

            // Calculate the file's hash
            string hashStrMd5 = "";
            string hashStrSha256 = "";

            // SHA256
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashSha256 = sha.ComputeHash(fs);
                    hashStrSha256 = BitConverter.ToString(hashSha256).Replace("-", String.Empty).ToLowerInvariant();
                }
            }

            // MD5
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var hashMd5 = md5.ComputeHash(fs);
                    hashStrMd5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                }
            }

            // Update hash strings
            HashStringSHA256 = hashStrSha256;
            HashStringMD5 = hashStrMd5;

            // Check that the hash matches a supported hash
            List<string> md5s = new List<string>(ExpectedMD5s);
            List<string> sha256s = new List<string>(ExpectedSHA256s);
            IsHashValid = (md5s.Contains(HashStringMD5) && sha256s.Contains(HashStringSHA256));
            if (IsHashValid)
            {
                HashValidationMessage = "ROM checksum is valid, good to go!";
            }
            else
            {
                HashValidationMessage = "Wrong file checksum. Please try another ROM, or it may not work.";
                return false;
            }

            // If we made it this far, the file looks good!
            return true;
        }
    }
}
