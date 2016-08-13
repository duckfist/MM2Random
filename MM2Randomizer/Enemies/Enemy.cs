using System.Collections.Generic;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Enemies
{
    public class Enemy
    {
        public EEnemyID ID { get; set; }
        public List<byte> PatternTableAddresses { get; set; }
        public List<int> SpriteBankRows { get; set; }

        public Enemy(EEnemyID id, List<byte> patternTableAddresses, List<int> spriteBankRows)
        {
            ID = id;
            PatternTableAddresses = patternTableAddresses;
            SpriteBankRows = spriteBankRows;
        }
    }
}
