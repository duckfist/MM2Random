using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enemies
{
    public class EnemyRoom
    {
        public string RoomName { get; set; }
        public int[] EnemyIDAddresses { get; set; }
        public int PatternAddressStart { get; set; }
        public List<Enemy> NewEnemyInstances { get; set; }

        public int[] SpriteBankRowsRestriction { get; set; }
        public int[] PatternTableAddressesRestriction { get; set; }

        public EnemyRoom (string roomName, int patternAddressStart, params int[] enemyIDAddresses)
        {
            this.RoomName = roomName;
            this.EnemyIDAddresses = enemyIDAddresses;
            this.PatternAddressStart = patternAddressStart;
            NewEnemyInstances = new List<Enemy>();
        }

        public EnemyRoom (string roomName, int patternAddressStart, int[] spriteBankRowsRestriction, int[] patternTableAddressesRestriction, params int[] enemyIDAddresses)
            : this(roomName, patternAddressStart, enemyIDAddresses)
        {
            SpriteBankRowsRestriction = spriteBankRowsRestriction;
            PatternTableAddressesRestriction = patternTableAddressesRestriction;
        }
    }
}
