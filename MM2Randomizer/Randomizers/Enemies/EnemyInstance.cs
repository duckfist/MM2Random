using System.Collections.Generic;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyInstance
    {
        public int StageNum { get; set; }
        public int ScreenNum { get; set; }
        public int Offset { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int YPrev { get; set; }
        public int EnemyID { get; set; }
        public int EnemyIDPrev { get; set; }
        
        //public bool IsRandomiziable { get; set; }
        //public List<byte> PatternTableAddresses { get; set; }
        //public List<int> SpriteBankRows { get; set; }

        public EnemyInstance(int stage, int screen, int y, int id, int offset)
        {
            StageNum = stage;
            ScreenNum = screen;
            Y = y;
            YPrev = y;
            EnemyID = id;
            EnemyIDPrev = id;
            Offset = offset;
        }
    }
}