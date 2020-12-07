using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MM2Randomizer
{
    public class RandoSettings : ObservableBase
    {
        //
        // Constructor
        //

        public RandoSettings()
        {
            // Flags for Rando Core Modules (Interdependent, cannot be changed from UI)
            this.Is8StagesRandom = true;
            this.IsWeaponsRandom = true;
            this.IsTeleportersRandom = true;

            // Flags for Rando Gameplay Modules
            this.IsWeaponBehaviorRandom = true;
            this.IsWeaknessRandom = true;
            this.IsBossInBossRoomRandom = true;
            this.IsBossAIRandom = true;
            this.IsItemsRandom = true;
            this.IsEnemiesRandom = true;
            this.IsEnemyWeaknessRandom = true;
            this.IsTilemapChangesEnabled = true;

            // Flags for Rando Cosmetic Modules
            this.IsWeaponNamesRandom = true;
            this.IsColorsRandom = true;
            this.IsBGMRandom = true;
            this.SelectedPlayer = PlayerSprite.Rockman;

            // Flags for Optional Gameplay Modules
            this.FastText = true;
            this.BurstChaserMode = false;
            this.IsSpoilerFree = false;
        }


        //
        // Public Properties
        //

        /// <summary>
        /// Alphabetical string representation of the RandomMM2.Seed integer of the most
        /// recently generated ROM.
        /// </summary>
        public String SeedString
        {
            get => mSeedString;

            set
            {
                value = value.ToUpper();

                if (this.mSeedString != value)
                {
                    this.mSeedString = value;
                    this.NotifyPropertyChanged();

                    // TODO: Check for better validity of seed
                    this.IsSeedValid = !String.IsNullOrEmpty(this.mSeedString);
                    this.IsSourcePathAndSeedValid = this.IsSourcePathValid && this.IsSeedValid;
                }
            }
        }

        /// <summary>
        /// Full path to user-provided ROM to apply patch.
        /// </summary>
        public String SourcePath
        {
            get => this.mSourcePath;

            set
            {
                this.ValidateFile(value);
                this.SetProperty(ref this.mSourcePath, value);
            }
        }

        public Boolean IsSourcePathValid
        {
            get => this.mIsSourcePathValid;
            set => this.SetProperty(ref this.mIsSourcePathValid, value);
        }

        public Boolean IsSeedValid
        {
            get => this.mIsSeedValid;
            set => this.SetProperty(ref this.mIsSeedValid, value);
        }

        // TODO need this?
        public Boolean IsSourcePathAndSeedValid
        {
            get => this.mIsSourcePathAndSeedValid;
            set => this.SetProperty(ref this.mIsSourcePathAndSeedValid, value);
        }

        public Boolean IsSpoilerFree
        {
            get => this.mIsSpoilerFree;
            set => this.SetProperty(ref this.mIsSpoilerFree, value);
        }

        public String HashStringMD5
        {
            get => this.mHashStringMD5;
            set => this.SetProperty(ref this.mHashStringMD5, value);
        }

        public String HashStringSHA256
        {
            get => this.mHashStringSHA256;
            set => this.SetProperty(ref this.mHashStringSHA256, value);
        }

        public String HashValidationMessage
        {
            get => this.mHashValidationMessage;
            set
            {
                Boolean result = this.SetProperty(ref this.mHashValidationMessage, value);
            }
        }

        public Boolean IsHashValid
        {
            get => this.mIsHashValid;
            set => this.SetProperty(ref this.mIsHashValid, value);
        }

        public Boolean CreateLogFile
        {
            get => this.mCreateLogFile;
            set => this.SetProperty(ref this.mCreateLogFile, value);
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


        //
        // Flags
        //

        /// <summary>
        /// If True, the Robot Master stages will be shuffled and will not be indicated by the
        /// portraits on the Stage Select screen.
        /// </summary>
        public Boolean Is8StagesRandom { get; set; }

        /// <summary>
        /// If True, the weapons awarded from each Robot Master is shuffled.
        /// </summary>
        public Boolean IsWeaponsRandom { get; set; }

        /// <summary>
        /// If True, Items 1, 2, and 3 will be awarded from random Robot Masters.
        /// </summary>
        public Boolean IsItemsRandom { get; set; }

        /// <summary>
        /// If true, in Wily 5, the Robot Master locations in each teleporter is randomized.
        /// </summary>
        public Boolean IsTeleportersRandom { get; set; }

        /// <summary>
        /// If True, the damage each weapon does against each Robot Master is changed. The manner in
        /// which it is changed depends on if IsWeaknessEasy is True or if IsWeaknessHard is True.
        /// </summary>
        public Boolean IsWeaknessRandom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsBossAIRandom { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Boolean IsColorsRandom { get;  set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Boolean IsEnemiesRandom { get; set; }

        public Boolean IsEnemyWeaknessRandom { get; set; } 

        public Boolean IsBossInBossRoomRandom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsTilemapChangesEnabled { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Boolean IsBGMRandom { get; set; }

        /// <summary>
        /// Change this value to set Mega Man's sprite graphic.
        /// </summary>
        public PlayerSprite SelectedPlayer
        {
            get => this.mSelectedPlayer;
            set => this.SetProperty(ref this.mSelectedPlayer, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsWeaponNamesRandom { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Boolean FastText { get; set; }

        public Boolean IsStageNameHidden { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Boolean BurstChaserMode { get; set; }

        public Boolean IsWeaponBehaviorRandom { get; set; }


        //
        // Public Methods
        //

        public String GetFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append((true == this.Is8StagesRandom && true == this.IsWeaponsRandom && true == this.IsTeleportersRandom) ? '!' : ' ');

            //if (Is8StagesRandom)         sb.Append('A');    else sb.Append(' ');
            //if (IsWeaponsRandom)         sb.Append('B');    else sb.Append(' ');
            //if (IsTeleportersRandom)     sb.Append('C');    else sb.Append(' ');

            sb.Append(true == this.IsWeaponBehaviorRandom ? 'A' : ' ');
            sb.Append(true == this.IsWeaknessRandom ? 'B' : ' ');
            sb.Append(true == this.IsBossInBossRoomRandom ? 'C' : ' ');
            sb.Append(true == this.IsBossAIRandom ? 'D' : ' ');
            sb.Append(true == this.IsItemsRandom ? 'E' : ' ');
            sb.Append(true == this.IsEnemiesRandom ? 'F' : ' ');
            sb.Append(true == this.IsEnemyWeaknessRandom ? 'G' : ' ');
            sb.Append(true == this.IsTilemapChangesEnabled ? 'H' : ' ');

            sb.Append(true == this.IsWeaponNamesRandom ? '1' : ' ');
            sb.Append(true == this.IsColorsRandom ? '2' : ' ');
            sb.Append(true == this.IsBGMRandom ? '3' : ' ');

            sb.Append(true == this.FastText ? 't' : ' ');
            sb.Append(true == this.BurstChaserMode ? '@' : ' ');
            sb.Append(true == this.IsStageNameHidden ? '?' : ' ');

            return sb.ToString();
        }


        /// <summary>
        /// This method checks that a file exists and then compares its checksum with known good Mega Man 2 ROMs.
        /// If it fails any of this, the method returns false.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Boolean ValidateFile(String path)
        {
            // Check if file even exists
            //SourcePath = path;
            this.IsSourcePathValid = System.IO.File.Exists(path);
            this.IsSourcePathAndSeedValid = this.IsSourcePathValid && this.IsSeedValid;

            if (false == this.IsSourcePathValid)
            {
                this.IsHashValid = false;
                this.HashValidationMessage = "File does not exist.";
                return false;
            }

            // Ensure file size is small so that we can take the hash
            FileInfo info = new System.IO.FileInfo(path);
            Int64 size = info.Length;

            if (size > 2000000)
            {
                Decimal MB = (size / (decimal)(1024d * 1024d));

                this.HashValidationMessage = $"File is {MB:0.00} MB, clearly not a NES ROM. WTF are you doing?";
                this.IsSourcePathValid = false;
                this.IsHashValid = false;
                return false;
            }

            // Calculate the file's hash
            String hashStrMd5 = "";
            String hashStrSha256 = "";

            // SHA256
            using (System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashSha256 = sha.ComputeHash(fs);
                    hashStrSha256 = BitConverter.ToString(hashSha256).Replace("-", String.Empty).ToLowerInvariant();
                }
            }

            // MD5
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashMd5 = md5.ComputeHash(fs);
                    hashStrMd5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                }
            }

            // Update hash strings
            this.HashStringSHA256 = hashStrSha256;
            this.HashStringMD5 = hashStrMd5;

            // Check that the hash matches a supported hash
            List<String> md5s = new List<String>(EXPECTED_MD5_HASH_LIST);
            List<String> sha256s = new List<String>(EXPECTED_SHA256_HASH_LIST);

            this.IsHashValid = (md5s.Contains(this.HashStringMD5) && sha256s.Contains(this.HashStringSHA256));


            if (this.IsHashValid)
            {
                this.HashValidationMessage = "ROM checksum is valid, good to go!";
            }
            else
            {
                this.HashValidationMessage = "Wrong file checksum. Please try another ROM, or it may not work.";
                return false;
            }

            // If we made it this far, the file looks good!
            return true;
        }


        //
        // Private Data Members
        //

        private String mSeedString;
        private String mSourcePath;
        private Boolean mIsSourcePathValid;
        private Boolean mIsSeedValid;
        private Boolean mIsSourcePathAndSeedValid;
        private String mHashStringMD5;
        private String mHashStringSHA256;
        private String mHashValidationMessage;
        private Boolean mIsHashValid;
        private Boolean mIsSpoilerFree;
        private Boolean mCreateLogFile = false;

        private PlayerSprite mSelectedPlayer;

        public readonly String[] EXPECTED_MD5_HASH_LIST = new String[]
        {
            "caaeb9ee3b52839de261fd16f93103e6", // Mega Man 2 (U)
            "8e4bc5b03ffbd4ef91400e92e50dd294", // Mega Man 2 (USA)
        };

        public readonly String[] EXPECTED_SHA256_HASH_LIST = new String[]
        {
            "27b5a635df33ed57ed339dfc7fd62fc603b39c1d1603adb5cdc3562a0b0d555b", // Mega Man 2 (U)
            "49136b412ff61beac6e40d0bbcd8691a39a50cd2744fdcdde3401eed53d71edf", // Mega Man 2 (USA)
        };
    }

    public enum PlayerSprite
    {
        Rockman,
        Protoman,
        Roll,
        Bass
    }
}
