using System.Collections.Generic;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class SpriteBankRoomGroup
    {
        public EStageID Stage { get; set; }

        /// <summary>
        /// Room numbers in the stage that this sprite bank applies to. Will always be sorted from least to greatest.
        /// </summary>
        public int[] RoomNums { get; set; }
        public EnemyInstance[] EnemyInstances { get; set; }
        public int PatternAddressStart { get; set; }
        public List<EnemyType> NewEnemyTypes { get; set; }

        public bool IsSpriteRestricted { get; set; }
        public int[] SpriteBankRowsRestriction { get; set; }
        public int[] PatternTableAddressesRestriction { get; set; }

        public SpriteBankRoomGroup (EStageID stage, int patternAddressStart, int[] roomNums, params EnemyInstance[] enemyInstances)
        {
            this.Stage = stage;
            this.RoomNums = roomNums;
            this.EnemyInstances = enemyInstances;
            this.PatternAddressStart = patternAddressStart;
            NewEnemyTypes = new List<EnemyType>();
            IsSpriteRestricted = false;
        }

        public SpriteBankRoomGroup (EStageID stage, int patternAddressStart, int[] roomNums, int[] spriteBankRowsRestriction, int[] patternTableAddressesRestriction, params EnemyInstance[] enemyInstances)
            : this(stage, patternAddressStart, roomNums, enemyInstances)
        {
            SpriteBankRowsRestriction = spriteBankRowsRestriction;
            PatternTableAddressesRestriction = patternTableAddressesRestriction;
            IsSpriteRestricted = true;
        }
    }
}
