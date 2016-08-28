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

        /// <summary>
        /// Helps determine y-position adjustment for spawning.  If height = 0, it can spawn at the top
        /// of the screen (pipis, deactivators, etc.)
        /// </summary>
        public int YAdjust { get; set; }

        public EnemyType(EEnemyID id, List<byte> patternTableAddresses, List<int> spriteBankRows, bool isYPosAir = false, int yAdjust = 0)
        {
            ID = id;
            PatternTableAddresses = patternTableAddresses;
            SpriteBankRows = spriteBankRows;
            IsYPosAir = isYPosAir;
            YAdjust = yAdjust;
        }
    }
}
