using System.Collections.Generic;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyType
    {
        public EEnemyID ID { get; set; }
        public List<byte> PatternTableAddresses { get; set; }
        public List<int> SpriteBankRows { get; set; }
        public bool IsYPosAir { get; set; }
        public bool IsActivator { get; set; }

        /// <summary>
        /// Helps determine y-position adjustment for spawning.  If height = 0, it can spawn at the top
        /// of the screen (pipis, deactivators, etc.)
        /// </summary>
        public int YAdjust { get; set; }

        public EnemyType(EEnemyID id, List<byte> patternTableAddresses, List<int> spriteBankRows, bool isActivator, bool isYPosAir = false, int yAdjust = 0)
        {
            ID = id;
            PatternTableAddresses = patternTableAddresses;
            SpriteBankRows = spriteBankRows;
            IsActivator = isActivator;
            IsYPosAir = isYPosAir;
            YAdjust = yAdjust;
        }

        public static EEnemyID GetCorrespondingDeactivator(EEnemyID activator)
        {
            switch (activator)
            {
                case EEnemyID.Pipi_Activator:
                    return EEnemyID.Pipi_Deactivator;
                case EEnemyID.Mole_Activator:
                    return EEnemyID.Mole_Deactivator;
                case EEnemyID.Claw_Activator:
                    return EEnemyID.Claw_Deactivator;
                case EEnemyID.Kukku_Activator:
                    return EEnemyID.Kukku_Deactivator;
                case EEnemyID.M445_Activator:
                    return EEnemyID.M445_Deactivator;
                default: return activator;
            }
        }

        public static EEnemyID GetCorrespondingDeactivator(int deactivatorID)
        {
            return GetCorrespondingDeactivator((EEnemyID)deactivatorID);
        }

        public static bool CheckIsActivator(int enemyID)
        {
            switch ((EEnemyID)enemyID)
            {
                case EEnemyID.Pipi_Activator:
                    return true;
                case EEnemyID.Mole_Activator:
                    return true;
                case EEnemyID.Claw_Activator:
                    return true;
                case EEnemyID.Kukku_Activator:
                    return true;
                case EEnemyID.M445_Activator:
                    return true;
                default: return false;
            }
        }

        public static bool CheckIsDeactivator(int enemyID)
        {
            switch ((EEnemyID)enemyID)
            {
                case EEnemyID.Pipi_Deactivator:
                    return true;
                case EEnemyID.Mole_Deactivator:
                    return true;
                case EEnemyID.Claw_Deactivator:
                    return true;
                case EEnemyID.Kukku_Deactivator:
                    return true;
                case EEnemyID.M445_Deactivator:
                    return true;
                default: return false;
            }
        }


    }
}
