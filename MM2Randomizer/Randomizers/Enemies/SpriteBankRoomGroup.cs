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
        public int PatternAddressStart { get; set; }
        public List<EnemyInstance> EnemyInstances { get; set; }
        public List<EnemyType> NewEnemyTypes { get; set; }

        public bool IsSpriteRestricted { get; set; }
        public List<int> SpriteBankRowsRestriction { get; set; }
        public List<byte> PatternTableAddressesRestriction { get; set; }

        public SpriteBankRoomGroup (EStageID stage, int patternAddressStart, int[] roomNums/*, params EnemyInstance[] enemyInstances*/)
        {
            this.Stage = stage;
            this.RoomNums = roomNums;
            this.PatternAddressStart = patternAddressStart;

            EnemyInstances = new List<EnemyInstance>();
            NewEnemyTypes = new List<EnemyType>();
            IsSpriteRestricted = false;
        }

        public SpriteBankRoomGroup (EStageID stage, int patternAddressStart, int[] roomNums, int[] spriteBankRowsRestriction, byte[] patternTableAddressesRestriction/*, params EnemyInstance[] enemyInstances*/)
            : this(stage, patternAddressStart, roomNums)
        {
            SpriteBankRowsRestriction = new List<int>(spriteBankRowsRestriction);
            PatternTableAddressesRestriction = new List<byte>(patternTableAddressesRestriction);
            IsSpriteRestricted = true;
        }
    }
}
