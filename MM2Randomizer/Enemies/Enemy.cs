using System.Collections.Generic;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Enemies
{
    public class Enemy
    {
        public EEnemyID ID { get; set; }
        public List<byte> PatternTableAddresses { get; set; }
        public List<int> SpriteBankRows { get; set; }

        /// <summary>
        /// Helps determine y-position adjustment for spawning.  If height = 0, it can spawn at the top
        /// of the screen (pipis, deactivators, etc.)
        /// </summary>
        public int Height { get; set; }

        public Enemy(EEnemyID id, List<byte> patternTableAddresses, List<int> spriteBankRows, int height = 0)
        {
            ID = id;
            PatternTableAddresses = patternTableAddresses;
            SpriteBankRows = spriteBankRows;
            Height = height;
        }
    }
}
