using System.Collections.Generic;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyRoom
    {
        public EStageID Stage { get; set; }
        public int[] RoomNums { get; set; }
        public int[] EnemyIDAddresses { get; set; }
        public int PatternAddressStart { get; set; }
        public List<Enemy> NewEnemyInstances { get; set; }

        public bool IsSpriteRestricted { get; set; }
        public int[] SpriteBankRowsRestriction { get; set; }
        public int[] PatternTableAddressesRestriction { get; set; }

        public EnemyRoom (EStageID stage, int patternAddressStart, int[] roomNums, params int[] enemyIDAddresses)
        {
            this.Stage = stage;
            this.RoomNums = roomNums;
            this.EnemyIDAddresses = enemyIDAddresses;
            this.PatternAddressStart = patternAddressStart;
            NewEnemyInstances = new List<Enemy>();
            IsSpriteRestricted = false;
        }

        public EnemyRoom (EStageID stage, int patternAddressStart, int[] roomNums, int[] spriteBankRowsRestriction, int[] patternTableAddressesRestriction, params int[] enemyIDAddresses)
            : this(stage, patternAddressStart, roomNums, enemyIDAddresses)
        {
            SpriteBankRowsRestriction = spriteBankRowsRestriction;
            PatternTableAddressesRestriction = patternTableAddressesRestriction;
            IsSpriteRestricted = true;
        }
    }
}
