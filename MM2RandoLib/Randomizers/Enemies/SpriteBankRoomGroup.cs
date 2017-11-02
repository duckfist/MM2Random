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
        public List<Room> Rooms { get; set; }
        public int PatternAddressStart { get; set; }
        public List<EnemyType> NewEnemyTypes { get; set; }

        public bool IsSpriteRestricted { get; set; }
        public List<int> SpriteBankRowsRestriction { get; set; }
        public List<byte> PatternTableAddressesRestriction { get; set; }

        public SpriteBankRoomGroup (EStageID stage, int patternAddressStart, int[] roomNums)
        {
            this.Stage = stage;
            this.PatternAddressStart = patternAddressStart;

            Rooms = new List<Room>();
            for (int i = 0; i < roomNums.Length; i++)
            {
                Rooms.Add(new Room(roomNums[i]));
            }

            //EnemyInstances = new List<EnemyInstance>();
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

        public bool ContainsRoom(int roomNum)
        {
            foreach (Room room in Rooms)
            {
                if (room.RoomNum == roomNum)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
