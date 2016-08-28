namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyInstance
    {
        public int Offset { get; set; }
        public int StageNum { get; set; }
        public int RoomNum { get; set; }
        public int ScreenNum { get; set; }
        public bool IsActive { get; set; }
        public int EnemyID { get; set; }
        public int EnemyIDPrev { get; set; } // for reference only
        public int X { get; set; }
        public int YOriginal { get; set; } // for reference only
        public int YAir { get; set; }
        public int YGround { get; set; }
        public bool IsFaceRight { get; set; }
        
        //public bool IsRandomiziable { get; set; }
        //public List<byte> PatternTableAddresses { get; set; }
        //public List<int> SpriteBankRows { get; set; }
        
        public EnemyInstance(int offset, int stage, int room, int screen, bool isActive, int id, int xOriginal, int yOriginal, int yAir, int yGround, bool faceRight)
        {
            Offset = offset;
            StageNum = stage;
            RoomNum = room;
            ScreenNum = screen;
            IsActive = isActive;
            EnemyID = id;
            EnemyIDPrev = id;
            X = xOriginal;
            YOriginal = yOriginal;
            YAir = yAir;
            YGround = yGround;
            IsFaceRight = faceRight;
        }
    }
}